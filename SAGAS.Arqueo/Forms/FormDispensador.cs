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
    public partial class FormDispensador : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogDispensador nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormDispensador()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Inactivar";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnCambiarLectura.Enabled = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnCambiarLectura");
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

                bdsManejadorDatos.DataSource = from D in db.Dispensadors
                                               join I in db.Islas on D.IslaID equals I.ID
                                               where D.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == I.EstacionServicioID))
                                               select new
                                               {
                                                  D.ID,
                                                  D.Nombre,
                                                  D.IslaID,
                                                  I.EstacionServicioID,
                                                  I.SubEstacionID,
                                                  D.Modelo,
                                                  D.Serie,
                                                  D.Marca,
                                                  D.CodigoDNP,
                                                  D.LecturaMecanica,
                                                  D.LecturaEfectivo,
                                                  D.LecturaElectronica,
                                                  D.Comentario,
                                                  D.Activo
                                               };

                this.grid.DataSource = bdsManejadorDatos;

                //**Islas
                lkrIsla.DataSource = from I in db.Islas
                                                 where I.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == I.EstacionServicioID))
                                                 select new { I.ID, I.Nombre };

                lkrIsla.DisplayMember = "Nombre";
                lkrIsla.ValueMember = "ID";

                //**Estaciones de Servicio
                lkEstacionServicio.DataSource = from es in db.EstacionServicios
                                                 where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                                                 select new { es.ID, es.Nombre };

                lkEstacionServicio.DisplayMember = "Nombre";
                lkEstacionServicio.ValueMember = "ID";

                //**SubEstacionServicio
                lkSubEstacion.DataSource = db.SubEstacions.Where(sus => sus.Activo).Select(s => new { s.ID, s.Nombre });

                lkSubEstacion.DisplayMember = "Nombre";
                lkSubEstacion.ValueMember = "ID";

                gvData_FocusedRowChanged(null, null);

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
                    nf = new Forms.Dialogs.DialogDispensador(Usuario);
                    nf.Text = "Crear Dispensador";
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
                        nf = new Forms.Dialogs.DialogDispensador(Usuario);
                        nf.Text = "Editar Dispensador";
                        nf.EntidadAnterior = db.Dispensadors.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        //nf.lkeEstacionServicio = Convert.ToInt32(nf.)
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
                        var D = db.Dispensadors.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {

                                //if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == I.ID && o.Activo).Count()) > 0)
                                //{
                                //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEBORRAR + Environment.NewLine, Parametros.MsgType.warning);
                                //    return;
                                //}

                                //Desactivar Registro 
                                D.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Inactivó la Dispensador: " + D.Nombre, this.Name);
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
                                D.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se activó la Dispensador: " + D.Nombre, this.Name);
                                this.btnAnular.Caption = "Activar";

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

        #region <<< EVENTOS >>>

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                Parametros.General.CambiarActivogvData(this, gvData, colActivo.FieldName);
                try
                {
                    int IDDispensador = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));


                    var obj = (from M in db.Mangueras
                               join t in db.Tanques on M.TanqueID equals t.ID
                               where M.DispensadorID == IDDispensador && M.Activo
                               select new {
                                   IDManguera = M.ID,
                                   MangueraNombre = M.Numero,
                                   M.Lado,
                                   t.Color,
                                   LecturaMecanica = M.LecturaMecanica,
                                   LecturaEfectivo = M.LecturaEfectivo,
                                   LecturaElectronica = M.LecturaElectronica
                               }).OrderBy(o => o.MangueraNombre);

                    gridMangueras.DataSource = obj;

                                      
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        private void btnCambiarLectura_Click(object sender, EventArgs e)
        {   
            if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    int IDDispensador = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                    using (Forms.Dialogs.DialogCambiarLectura dg = new Forms.Dialogs.DialogCambiarLectura(IDDispensador))
                    {
                        dg.Text = "Cambiar Lecturas";

                         var obj = (from M in db.Mangueras
                                    join t in db.Tanques on M.TanqueID equals t.ID
                                       where M.DispensadorID == IDDispensador && M.Activo
                                       select new
                                       {
                                           IDManguera = M.ID,
                                           MangueraNombre = M.Numero,
                                           t.Color,
                                           LecturaMecanica = M.LecturaMecanica,
                                           LecturaEfectivo = M.LecturaEfectivo,
                                           LecturaElectronica = M.LecturaElectronica
                                       }).OrderBy(o => o.MangueraNombre);

                            DataTable dtAnterior = Parametros.General.LINQToDataTable(obj);
                                 
                        if (dg.ShowDialog() == DialogResult.OK)
                        {
                           

                            foreach (DataRow fila in dg.dtMangueras.Rows)
                            {
                                Entidad.Manguera M = db.Mangueras.Single(m => m.ID == Convert.ToInt32(fila["IDManguera"]));

                                M.LecturaMecanica = Convert.ToDecimal(fila["LecturaMecanica"]);
                                M.LecturaEfectivo = Convert.ToDecimal(fila["LecturaEfectivo"]);
                                M.LecturaElectronica = Convert.ToDecimal(fila["LecturaElectronica"]);

                                db.SubmitChanges();

                            }

                            

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                             "Se cambió las lecturas de mangueras en el dispensador: " + db.Dispensadors.Single(d => d.ID == IDDispensador).Nombre, this.Name, dg.dtMangueras, dtAnterior);
                                                        
                            if (ShowMsgDialog)
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            else
                                this.timerMSG.Start();


                            FillControl();
                        }
                    } 

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

        }
        
        private void simpleButton1_Click(object sender, EventArgs e)
        {
             if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    var AIAnterior = db.ArqueoIslas.Where(d => d.IslaID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colIslaID)))).OrderByDescending(o => o.ID).First();

                    if (!AIAnterior.Cerrado || AIAnterior.Estado.Equals(Parametros.Estados.Modificado))
                    {
                        using (Forms.Dialogs.DialogCambiarLectura dg = new Forms.Dialogs.DialogCambiarLectura(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))))
                        {
                            dg.Text = "Actualizar lecturas Iniciales";
                            dg.ShowDialog();
                        }
                        //if (Parametros.General.DialogMsg("Desea cambiar las lecturas del Arqueo de Isla Nro. " + AIAnterior.Numero + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        //{




                        //}
                    }
                    else
                        Parametros.General.DialogMsg("No se pueden actualizar las lecturas del Dispensador seleccionado, El Arqueo de la Isla relacionada se encuentra cerrada", Parametros.MsgType.warning);

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        private void btnCambiarSello_Click(object sender, EventArgs e)
        {
             if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    var Disp = db.Dispensadors.Single(d => d.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    using (Forms.Dialogs.DialogSello dg = new Forms.Dialogs.DialogSello())
                    {
                        dg.Text = "Cambiar Sello Marchamo";
                        dg._ID = Disp.ID;
                        dg.Anterior = Disp.Sello;
                        dg.ShowDialog();
                    }
                    FillControl();
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
