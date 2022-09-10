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
    public partial class FormUsuario : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Wizards.wizAddUsuario nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormUsuario()
        {
            InitializeComponent();
            //Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Caption = "Inactivar";
            this.btnResetearPass.Enabled = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnResetearPass");
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

                bdsManejadorDatos.DataSource = from U in db.Usuarios
                                               select U;

                
                this.gridUser.DataSource = bdsManejadorDatos;


            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(gridUser);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(gridUser);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(gridUser);
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
            this.gvDataUser.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Wizards.wizAddUsuario();
                    nf.Text = "Agregar un nuevo usuario al sistema";
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
                    if (gvDataUser.FocusedRowHandle >= 0)
                    {
                        nf = new Forms.Wizards.wizAddUsuario();
                        nf.Text = "Editar el usuario " + Convert.ToString(gvDataUser.GetFocusedRowCellValue(colNombre));
                        nf.EntidadAnterior = db.Usuarios.Single(e => e.ID == Convert.ToInt32(gvDataUser.GetFocusedRowCellValue(colID)));
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
                    if (gvDataUser.FocusedRowHandle >= 0)
                    {
                        int id = Convert.ToInt32(gvDataUser.GetFocusedRowCellValue(colID));
                        var U = db.Usuarios.Single(u => u.ID == id);

                        if (gvDataUser.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                if (Parametros.General.UserID == U.ID)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                if (db.Usuarios.Count() == 1)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGULTIMOREGISTROINACTIVO + Environment.NewLine, Parametros.MsgType.warning);
                                    return;

                                }

                                //Desactivar Registro
                                U.Contrasena = Parametros.Security.Encrypt("inicio", Parametros.Config.MagicWord);
                                U.IsReset = true;
                                U.Login += "INACTIVO" + U.ID.ToString();
                                U.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó el Usuario: " + U.Nombre, this.Name);
                                this.btnAnular.Caption = "Activar";
                                this.btnAnular.Glyph = Parametros.Properties.Resources.Ok20;

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
                                if (Parametros.General.UserID == U.ID)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                if (db.Usuarios.Count() == 1)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGULTIMOREGISTROINACTIVO + Environment.NewLine, Parametros.MsgType.warning);
                                    return;

                                }

                                //Activar Registro
                                U.Contrasena = Parametros.Security.Encrypt("inicio", Parametros.Config.MagicWord);
                                U.IsReset = true;
                                U.Login += "[Crear nuevo login]";
                                U.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se activó el Usuario: " + U.Nombre, this.Name);
                                this.btnAnular.Caption = "Inactivar";
                                this.btnAnular.Glyph = Parametros.Properties.Resources.Anular24;
                                Parametros.General.DialogMsg("Por motivos seguridad, al reactivar al usuario: " + U.Nombre + ", deberá crear su nuevo login.", Parametros.MsgType.warning);
                                
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

        //RESETEAR CONTRASEÑNA
        private void btnResetearPass_Click(object sender, EventArgs e)
        {
            if (nf == null)
            {
                if (gvDataUser.FocusedRowHandle >= 0)
                {
                    int id = Convert.ToInt32(gvDataUser.GetFocusedRowCellValue(colID));  

                    try
                    {
                        var U = db.Usuarios.Single(u => u.ID == id);

                        if (Parametros.General.DialogMsg("¿Esta seguro de resetear la contraseña del usuario: " + U.Nombre + "?"  + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {

                            //Resetear Contraseña
                            U.Contrasena = Parametros.Security.Encrypt("inicio", Parametros.Config.MagicWord);
                            U.IsReset = true;
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se reseteó la contraseña del Usuario: " + U.Nombre, this.Name);

                            if (ShowMsgDialog)
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            else
                                this.timerMSG.Start();

                            FillControl();

                        }

                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
            }
        }
 
        //VISTA PREVIA AL LADO DE LOS REGISTROS
        private void gvDataUser_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvDataUser.FocusedRowHandle >= 0)
                {
                    int user = Convert.ToInt32(gvDataUser.GetFocusedRowCellValue(colID));

                    Parametros.General.CambiarActivogvData(this, gvDataUser, "Activo");

                    //CARGANDO ESTACIONES
                    var obj = (from gvs in db.GetViewEstacionesServicioByUsers
                               join es in db.EstacionServicios on gvs.EstacionServicioID equals es.ID
                               where gvs.UsuarioID == user
                               select new { gvs.EstacionServicioID, es.Nombre }).OrderBy(o => o.Nombre);


                    gridES.DataSource = obj;
                    gvDataES.RefreshData();

                    //CARGANDO PERMISOS
                    this.gridAccesos.DataSource = null;
                    this.gridAccTareas.DataSource = null;



                    var objDM = from asis in db.AccesoSistemas
                                join a in db.Accesos on asis.AccesoID equals a.ID
                                join m in db.Modulos on a.ModuloID equals m.ID
                                where asis.UsuarioID == user && a.PermiteMenu
                                group asis by new {Acceso = a.Nombre,Modulo = m.Nombre, asis.Agregar, asis.Modificar, asis.Anular, asis.Imprimir, asis.Exportar } into g
                                select new { g.Key.Modulo, g.Key.Acceso, g.Key.Agregar, g.Key.Modificar, g.Key.Anular, g.Key.Imprimir, g.Key.Exportar };

                    var objAS = from asis in db.AccesoSistemas
                                join a in db.Accesos on asis.AccesoID equals a.ID
                                join m in db.Modulos on a.ModuloID equals m.ID
                                where asis.UsuarioID == user && !a.PermiteMenu
                                group asis by new { Acceso = a.Nombre, Modulo = m.Nombre} into g
                                select new { g.Key.Modulo, g.Key.Acceso, };


                    gridAccesos.DataSource = objDM;
                    gridAccTareas.DataSource = objAS;
                    this.gvDataAccesos.RefreshData();
                    this.gvDataTareas.RefreshData();


                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Copiar Usuario
        private void barCopiarUsuario_Click(object sender, EventArgs e)
        {
            if (nf == null)
            {
                if (gvDataUser.FocusedRowHandle >= 0)
                {
                    try
                    {
                        if (gvDataUser.FocusedRowHandle >= 0)
                        {
                            nf = new Forms.Wizards.wizAddUsuario();
                            nf.Text = "Editar el usuario " + Convert.ToString(gvDataUser.GetFocusedRowCellValue(colNombre));
                            nf.EntidadAnterior = db.Usuarios.Single(s => s.ID == Convert.ToInt32(gvDataUser.GetFocusedRowCellValue(colID)));
                            nf.Owner = this;
                            nf.Editable = true;
                            nf.Clonar = true;
                            nf.MDI = this;
                            nf.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
            }
        }

        #endregion

    }
}
