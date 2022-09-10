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
    public partial class FormEstruckOrgani : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogEO nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormEstruckOrgani()
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

                bdsManejadorDatos.DataSource = from eo in db.EstructuraOrganizativa
                                               select eo;

                //EMPRESAS
                rpLkEmpresa.DataSource = db.Empresas.Where(o => o.Activo).Select(s => new { s.ID, Display = s.Nombre });

                treeEstrukOrgani.DataSource = bdsManejadorDatos;

                treeEstrukOrgani.ExpandAll();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        } 

        protected override void Imprimir()
        {
           //this.PrintList(grid);
        }

        protected override void ExportarExcel()
        {
            //this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            //this.ExportListToPDF(grid);
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
            //this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogEO();
                    nf.Text = "Crear Estructura Organizativa";
                    nf.Owner = this;

                    if (treeEstrukOrgani.FocusedNode != null)
                        nf.PadreID = Convert.ToInt32(treeEstrukOrgani.FocusedNode.GetValue(colID));
                     
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
                    if (treeEstrukOrgani.FocusedNode != null)
                    {
                        nf = new Forms.Dialogs.DialogEO();
                        nf.Text = "Editar Estructura Organizativa";
                        nf.EntidadAnterior = db.EstructuraOrganizativa.Single(e => e.ID.Equals(Convert.ToInt32(treeEstrukOrgani.FocusedNode.GetValue(colID))));
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
                    if (treeEstrukOrgani.FocusedNode != null)
                    {
                        int id = Convert.ToInt32(treeEstrukOrgani.FocusedNode.GetValue(colID));
                        var eo = db.EstructuraOrganizativa.Single(e => e.ID == id);

                        if (treeEstrukOrgani.FocusedNode.GetValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {


                                //--------descomentariar cuando planilla en new hashmap-------\\
                                /*if (Convert.ToInt32(db.Planilla.Where(o => o.EstructuraOrganizativaID.Equals(eo.ID) && o.Activo).Count()) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }*/

                                //Desactivar Registro 
                                eo.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina, "Se Inactivó la Estructura Organizacional: " + eo.Nombre, this.Name);
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
                                eo.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina, "Se activó la Estructura Organizacional: " + eo.Nombre, this.Name);
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
              
        #endregion

        private void treeEstrukOrgani_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (treeEstrukOrgani.FocusedNode != null)
                Parametros.General.CambiarActivoTreeData(this, treeEstrukOrgani, colActivo);
        }

        
    }
}
