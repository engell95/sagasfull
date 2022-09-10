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
    public partial class FormEmpleado : SAGAS.Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogEmpleado nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        
        #endregion

        #region <<< INICIO >>>

        public FormEmpleado()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
        }

        private void FormSucursal_Load(object sender, EventArgs e)
        {
            this.FillControl();
            this.btnAnular.Caption = "Desactivar";
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                List<Parametros.ListIdDisplay> Generos = new List<Parametros.ListIdDisplay>();
                Generos.Add(new Parametros.ListIdDisplay { ID = 0, Display = "M" });
                Generos.Add(new Parametros.ListIdDisplay { ID = 1, Display = "F" });
                lkGenero.DataSource = Generos.Select(s => new { ID = s.ID, Nombre = s.Display }).ToList();
                bdsManejadorDatos.DataSource = from v in dv.VistaEmleados
                                               select v;

                this.gvData.RefreshData();
                
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
                    nf = new Forms.Dialogs.DialogEmpleado();
                    nf.Text = "Crear Empleado";
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
                        nf = new Forms.Dialogs.DialogEmpleado();
                        nf.Text = "Editar Empleado";
                        nf.EntidadAnterior = db.Empleados.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue(colActivo)).Equals(true))
                        {
                            int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {

                                var E = db.Empleados.Single(c => c.ID == id);

                                if (Convert.ToInt32(db.EstacionServicios.Where(o => (o.AdministradorID.Equals(E.ID) || o.ArqueadorID.Equals(E.ID) || o.ResponsableContableID.Equals(E.ID)) && o.Activo).Count()) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine + "El empleado " + E.Nombres + " " + E.Apellidos + " esta asignado como firmante en una Estación de Servicio.", Parametros.MsgType.warning);
                                    return;
                                }

                                if (Convert.ToInt32(db.SubEstacions.Where(o => o.ArqueadorID.Equals(E.ID) && o.Activo).Count()) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine + "El empleado " + E.Nombres + " " + E.Apellidos + " esta asignado como firmante en una Sub Estación de Servicio.", Parametros.MsgType.warning);
                                    return;
                                }

                                //Desactivar Registro 
                                E.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina, "Se Inactivó el Empleado: " + E.Nombres + " " + E.Apellidos, this.Name);

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                FillControl();
                            }
                        }
                        else
                            Parametros.General.DialogMsg("El empleado " + Convert.ToString(gvData.GetFocusedRowCellValue(colCodigo)) + " | " + Convert.ToString(gvData.GetFocusedRowCellValue(colNombres)) + " " + Convert.ToString(gvData.GetFocusedRowCellValue(colApellidos)) + ", se encuentra desactivado.", Parametros.MsgType.warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
  
        #endregion        
        

    }
}
