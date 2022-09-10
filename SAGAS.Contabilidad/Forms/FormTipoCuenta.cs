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
    public partial class FormTipoCuenta : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogTipoCuenta nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormTipoCuenta()
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

                bdsManejadorDatos.DataSource = from tc in db.TipoCuentas
                                               select tc;

                treeTipoCuenta.DataSource = bdsManejadorDatos;

                treeTipoCuenta.ExpandAll();

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
                    nf = new Forms.Dialogs.DialogTipoCuenta();
                    nf.Text = "Crear Tipo de Cuenta";
                    nf.Owner = this;

                    if (treeTipoCuenta.FocusedNode != null)
                        nf.IdPadre = Convert.ToInt32(treeTipoCuenta.FocusedNode.GetValue(colID));
                     
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
                    if (treeTipoCuenta.FocusedNode != null)
                    {
                        if (treeTipoCuenta.FocusedNode.GetValue(colID) == null)
                            return;

                        nf = new Forms.Dialogs.DialogTipoCuenta();
                        nf.Text = "Editar Tipo de Cuenta";
                        nf.EntidadAnterior = db.TipoCuentas.Single(e => e.ID.Equals(Convert.ToInt32(treeTipoCuenta.FocusedNode.GetValue(colID))));
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
                nf = null;
            }
        }

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    if (treeTipoCuenta.FocusedNode != null)
                    {
                        if (treeTipoCuenta.FocusedNode.GetValue(colID) == null)
                            return;

                        int id = Convert.ToInt32(treeTipoCuenta.FocusedNode.GetValue(colID));
                        var tc = db.TipoCuentas.Single(c => c.ID == id);

                        if (treeTipoCuenta.FocusedNode.GetValue(colActivo) != null)
                        {
                            if (treeTipoCuenta.FocusedNode.GetValue(colActivo).Equals(true))
                            {
                                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                                {
                                    if (Convert.ToInt32(db.CuentaContables.Where(o => o.IDTipoCuenta.Equals(tc.ID) && o.Activo).Count()) > 0)
                                    {
                                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                        return;
                                    }

                                    //Desactivar Registro 
                                    tc.Activo = false;
                                    db.SubmitChanges();

                                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad, "Se Inactivó el Tipo de Cuenta: " + tc.Nombre, this.Name);
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
                                    tc.Activo = true;
                                    db.SubmitChanges();

                                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad, "Se activó el Tipo de Cuenta: " + tc.Nombre, this.Name);
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
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                nf = null;
            }
        }
              
        #endregion

        private void treeTipoCuenta_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (treeTipoCuenta.FocusedNode != null)
                if(treeTipoCuenta.FocusedNode.GetValue(colActivo) != null)
                    Parametros.General.CambiarActivoTreeData(this, treeTipoCuenta, colActivo);
        }

        
    }
}
