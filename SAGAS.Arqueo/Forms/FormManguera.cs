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
    public partial class FormManguera : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogManguera nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDCombustible = Parametros.Config.ProductoClaseCombustible();

        #endregion

        #region <<< INICIO >>>

        public FormManguera()
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

                bdsManejadorDatos.DataSource = from M in db.Mangueras
                                               join T in db.Tanques on M.TanqueID equals T.ID
                                               join D in db.Dispensadors on M.DispensadorID equals D.ID
                                               join I in db.Islas on D.IslaID equals I.ID
                                               where T.Activo && D.Activo && I.Activo
                                               && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && (ges.EstacionServicioID == T.EstacionServicioID && ges.EstacionServicioID == I.EstacionServicioID)))
                                               select new
                                               {
                                                   M.ID,
                                                   M.Numero,
                                                   M.Comentario,
                                                   M.Descuento,
                                                   M.DispensadorID,
                                                   I.EstacionServicioID,
                                                   I.SubEstacionID,
                                                   M.FlujoRapido,
                                                   M.Lado,
                                                   M.PrecioDiferenciado,
                                                   M.Serie,
                                                   M.TanqueID,
                                                   M.LecturaEfectivo,
                                                   M.LecturaElectronica,
                                                   M.LecturaMecanica,
                                                   T.Color,
                                                   M.Activo
                                               };
   
                this.grid.DataSource = bdsManejadorDatos;
              
                //**UnidadMedida
                lkrDispensador.DataSource = from D in db.Dispensadors
                                            join I in db.Islas on D.IslaID equals I.ID
                                            where D.Activo && I.Activo
                                            && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == I.EstacionServicioID))
                                            select D;

                lkrDispensador.DisplayMember = "Nombre";
                lkrDispensador.ValueMember = "ID";

                //**Tanque-Producto
                rglTanque.DataSource = from T in db.Tanques
                                            join P in db.Productos on T.ProductoID equals P.ID
                                       where T.Activo && P.Activo && P.ProductoClaseID == IDCombustible
                                            && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == T.EstacionServicioID))
                                            select new
                                                  {
                                                     IDTanque = T.ID,
                                                     TanqueNombre = T.Nombre,
                                                     ProductoNombre = P.Nombre,
                                                     Display = T.Nombre + " => " + P.Nombre
                                                  };

                rglTanque.DisplayMember = "Display";
                rglTanque.ValueMember = "IDTanque";

                //**Estaciones de Servicio
                lkEstacionServicio.DataSource = from es in db.EstacionServicios
                                                 where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                                                 select new { es.ID, es.Nombre };

                lkEstacionServicio.DisplayMember = "Nombre";
                lkEstacionServicio.ValueMember = "ID";

                //**SubEstacionServicio
                lkSES.DataSource = db.SubEstacions.Where(sus => sus.Activo).Select(s => new { s.ID, s.Nombre });

                lkSES.DisplayMember = "Nombre";
                lkSES.ValueMember = "ID";

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
                    nf = new Forms.Dialogs.DialogManguera(Usuario, IDCombustible);
                    nf.Text = "Crear Manguera";
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
                        nf = new Forms.Dialogs.DialogManguera(Usuario, IDCombustible);
                        nf.Text = "Editar Manguera";
                        nf.EntidadAnterior = db.Mangueras.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                        var M = db.Mangueras.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {

                                //if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == P.ID && o.Activo).Count()) > 0)
                                //{
                                //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEBORRAR + Environment.NewLine, Parametros.MsgType.warning);
                                //    return;
                                //}

                                //Desactivar Registro 
                                M.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Inactivó la Manguera: " + M.Numero, this.Name);
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
                                M.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se activó la Manguera: " + M.Numero, this.Name);
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

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
                Parametros.General.CambiarActivogvData(this, gvData, colActivo.FieldName);
        }

        
    }
}
