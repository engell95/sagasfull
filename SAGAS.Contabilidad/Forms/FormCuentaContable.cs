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
    public partial class FormCuentaContable : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogCuentaContable nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormCuentaContable()
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

                bdsManejadorDatos.DataSource = from cc in db.CuentaContables
                                               orderby cc.Codigo
                                               select cc;

                treeCC.DataSource = bdsManejadorDatos;

                treeCC.ExpandAll();

                //**TipoDeCuenta
                lkIDTipoCuenta.DataSource = from tc in db.TipoCuentas
                                              where tc.Activo
                                              select new { tc.ID, tc.Nombre };

                lkIDTipoCuenta.DisplayMember = "Nombre";
                lkIDTipoCuenta.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(treeCC);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(treeCC);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(treeCC);
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
            this.treeCC.FilterConditions.Clear(); //FilterConditions.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogCuentaContable();
                    nf.Text = "Crear Cuenta Contable";
                    nf.Owner = this;

                    if (treeCC.FocusedNode != null)
                        nf.IdPadre = Convert.ToInt32(treeCC.FocusedNode.GetValue(colID));
                     
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
                    if (treeCC.FocusedNode != null)
                    {
                        if (treeCC.FocusedNode.GetValue(colID) != null)
                        {
                            nf = new Forms.Dialogs.DialogCuentaContable();
                            nf.Text = "Editar Cuenta Contable";
                            nf.EntidadAnterior = db.CuentaContables.Single(e => e.ID.Equals(Convert.ToInt32(treeCC.FocusedNode.GetValue(colID))));
                            nf.Owner = this;
                            nf.Editable = true;
                            nf.MDI = this;
                            nf.Show();
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

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    if (treeCC.FocusedNode != null)
                    {
                        if (treeCC.FocusedNode.GetValue(colID) == null)
                            return;

                        int id = Convert.ToInt32(treeCC.FocusedNode.GetValue(colID));
                        var cc = db.CuentaContables.Single(c => c.ID == id);

                        if (treeCC.FocusedNode.GetValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                //Desactivar Registro 
                                cc.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad, "Se Inactivó la Cuenta Contable: " + cc.Nombre, this.Name);
                                this.btnAnular.Caption = "Activar";
                                //this.btnAnular.Glyph = SAGAS.Parametros.Properties.Resources.Ok24;

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
                                cc.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad, "Se activó la Cuenta Contable: " + cc.Nombre, this.Name);
                                this.btnAnular.Caption = "Inactivar";
                                this.btnAnular.Glyph = SAGAS.Parametros.Properties.Resources.Anular24;

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

        private void treeCC_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (treeCC.FocusedNode != null)
            {
                if (treeCC.FocusedNode.GetValue(colActivo) != null)
                {
                    Parametros.General.CambiarActivoTreeData(this, treeCC, colActivo);

                    try
                    {
                        int ID = Convert.ToInt32(treeCC.FocusedNode.GetValue(colID));


                        var obj = (from cces in db.CuentaContableEstacions
                                   join cc in db.CuentaContables on cces.CuentaContable equals cc
                                   join es in db.EstacionServicios on cces.EstacionID equals es.ID
                                   where cc.ID.Equals(ID)
                                   select new { EstacionServicioID = es.ID, es.Nombre }).OrderBy(o => o.Nombre);


                        gridES.DataSource = obj;

                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                    }
                }
            }
        }

        
    }
}
