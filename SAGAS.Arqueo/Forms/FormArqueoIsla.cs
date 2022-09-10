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
    public partial class FormArqueoIsla : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogArqueoIsla nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;


        #endregion

        #region <<< INICIO >>>

        public FormArqueoIsla()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
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

                bdsManejadorDatos.DataSource = from ai in db.ArqueoIslas
                                               join t in db.Turnos on ai.TurnoID equals t.ID
                                               join rd in db.ResumenDias on t.ResumenDiaID equals rd.ID
                                               where rd.EstacionServicioID.Equals(IDES)
                                               select new
                                               {
                                                      ai.ID,
                                                      Turno = t.Numero,
                                                      ai.FechaCreado,
                                                      Year = ai.FechaCreado.Year,
                                                      Mes = String.Format("{0:MMMM}", ai.FechaCreado),
                                                      ai.IslaID,
                                                      ai.Numero,
                                                      ai.TecnicoID,  
                                                      ai.Estado,
                                                      rd.EstacionServicioID,
                                                      rd.SubEstacionID,
                                                      ai.ArqueoEspecial,
                                                      ai.Oficial
                                               };

                this.grid.DataSource = bdsManejadorDatos;

                //**Estaciones de Servicio
                rpEstacionServicio.DataSource = from es in db.EstacionServicios
                                                where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                                                select new { es.ID, es.Nombre };

                rpEstacionServicio.DisplayMember = "Nombre";
                rpEstacionServicio.ValueMember = "ID";

                //**SubEstacionServicio
                lkSES.DataSource = db.SubEstacions.Where(sus => sus.Activo).Select(s => new { s.ID, s.Nombre });

                lkSES.DisplayMember = "Nombre";
                lkSES.ValueMember = "ID";

                //**Islas
                rpIslas.DataSource = from I in db.Islas
                                     where I.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == I.EstacionServicioID))
                                     select new { I.ID, I.Nombre };

                rpIslas.DisplayMember = "Nombre";
                rpIslas.ValueMember = "ID";     

                //**Tecnicos
                IQueryable<Parametros.ListIdDisplay> listEm = ((from em in db.Empleados
                              join pl in db.Planillas on em.PlanillaID equals pl.ID
                              where em.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == pl.EstacionServicioID))
                              select new Parametros.ListIdDisplay { ID = em.ID, Display = em.Nombres + " " + em.Apellidos }).OrderBy(o => o.Display));

                List<Parametros.ListIdDisplay> listadoDisplay = new List<Parametros.ListIdDisplay>(listEm);
                
                listadoDisplay.Add(new Parametros.ListIdDisplay(0, "Isla Cerrada")); 
                
                rpTecnico.DataSource = listadoDisplay;                   
                rpTecnico.DisplayMember = "Display";
                rpTecnico.ValueMember = "ID";

                //**Estados
                Parametros.ListEstados listado = new Parametros.ListEstados();
                rpEstado.DataSource = listado.GetListEstadosArqueo();
                rpEstado.DisplayMember = "Name";
                rpEstado.ValueMember = "ID";
                rpEstado.Columns.Clear();
                this.rpEstado.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
                new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Estado")}); 

                //FILTROS  
                if (gvData.RowCount > 0)
                    gvData.ActiveFilterString = "[Estado] = 0 AND [FechaCreado] > '" + Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID == IDES).OrderByDescending(o => o.ID).First().FechaInicial).ToString("dd/MM/yyyy") + "'";

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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, int IDAI)
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

            if (NextRegistro)
                Add();

            if (IDAI > 0)
                UpdateArqueoIsla(IDAI);
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
                    nf = new Forms.Dialogs.DialogArqueoIsla(Usuario);
                    nf.Text = "Crear Arqueo de Isla";
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
                        Entidad.ArqueoIsla AI = db.ArqueoIslas.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        if (!AI.Cerrado)
                        {
                            nf = new Forms.Dialogs.DialogArqueoIsla(Usuario);
                            nf.Text = "Editar Arqueo de Isla";
                            nf.EntidadAnterior = AI;
                            nf.Owner = this;
                            nf.Editable = true;
                            if (AI.Estado.Equals(Convert.ToInt32(Parametros.Estados.Modificado)))
                                nf.Modificando = true;

                            if (Parametros.General.ListSES.Count > 0)
                                nf.IDSUS = Convert.ToInt32(gvData.GetFocusedRowCellValue(colSubEstacion));

                            nf.MDI = this;
                            nf.Show();
                        }
                        else
                            Parametros.General.DialogMsg("El Arqueo " + AI.Numero.ToString() + " ya esta cerrado, no se puede editar.", Parametros.MsgType.warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void UpdateArqueoIsla(int IDAI)
        {
            try
            {
                if (nf == null && !IDAI.Equals(0))
                {
                    Entidad.ArqueoIsla AI = db.ArqueoIslas.Single(e => e.ID == IDAI);
                    nf = new Forms.Dialogs.DialogArqueoIsla(Usuario);
                    nf.Text = "Editar Arqueo de Isla";
                    nf.EntidadAnterior = AI;
                    nf.IDSUS = AI.Turno.ResumenDia.SubEstacionID;
                    nf.Owner = this;
                    nf.Editable = true; //nf.EntidadAnterior.Anulado;
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

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            this.Edit();
        }
              
        #endregion    

        
        
    }
}
