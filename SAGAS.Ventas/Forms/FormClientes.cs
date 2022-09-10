using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Ventas.Forms
{                                
    public partial class FormClientes : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogClientes nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormClientes()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Caption = "Inactivar";
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

                //**Departamento
                lkDepartamento.DataSource = from d in db.Departamentos
                                             where d.Activo
                                             select new { d.ID, d.Nombre };

                lkDepartamento.DisplayMember = "Nombre";
                lkDepartamento.ValueMember = "ID";

                lkTipoCliente.DataSource = from tc in db.TipoClientes
                                           where tc.Activo
                                           select new { tc.ID, tc.Nombre };

                bdsManejadorDatos.DataSource = (from C in db.Clientes
                                               join cc in db.CuentaContables on C.CuentaContableID equals cc.ID into DefaultCuentas
                                               from cuentas in DefaultCuentas.DefaultIfEmpty()
                                               join es in db.EstacionServicios on C.EstacionPagoID equals es.ID into DefaultEstaciones
                                               from estaciones in DefaultEstaciones.DefaultIfEmpty()
                                                select new
                                               {
                                                   ID = C.ID,
                                                   Nombre = C.Nombre,
                                                   Codigo = C.Codigo,
                                                   RazonSocial = C.RazonSocial,
                                                   RUC = C.RUC,
                                                   CuentaCodigo = cuentas.Codigo,
                                                   CuentaNombre = cuentas.Nombre,
                                                   C.DepartamentoID,
                                                   C.Contactos,
                                                   C.Direccion,
                                                   C.Telefono1,
                                                   C.Telefono2,
                                                   C.Telefono3,
                                                   C.Email,
                                                   C.WebSite,
                                                   C.LimiteCredito,
                                                   C.Plazo,
                                                   C.Municipio,
                                                   C.DescuentoXLitro,
                                                   C.Interes,
                                                   C.ExentoIVA,
                                                   C.TipoClienteID,
                                                   C.Activo,
                                                   C.AplicaCreditoLubricentro,
                                                   EstacionPago = estaciones.Nombre,
                                                   C.RealizaPagoGrupoES
                                               }).OrderBy(O => O.Codigo);

                this.grid.DataSource = bdsManejadorDatos;

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
                    nf = new Forms.Dialogs.DialogClientes();
                    nf.Text = "Crear Cliente";
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
                        nf = new Forms.Dialogs.DialogClientes();
                        nf.Text = "Editar Cliente";
                        nf.EntidadAnterior = db.Clientes.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                        var C = db.Clientes.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                var mov = from m in db.Movimientos
                                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                          where m.ClienteID.Equals(C) && !mt.EsAnulado
                                          select new { m.ID };

                                if (mov.Count() > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                //Desactivar Registro 
                                C.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó el Cliente: " + C.Nombre, this.Name);
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
                                C.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se activó el Cliente: " + C.Nombre, this.Name);
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
            {
                Parametros.General.CambiarActivogvData(this, gvData, colActivo.FieldName);
                try
                {
                    int ID = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                    var obj = (from ce in db.ClienteEstacions
                               join c in db.Clientes on ce.Cliente equals c
                               join es in db.EstacionServicios on ce.EstacionServicioID equals es.ID
                               where c.ID.Equals(ID)
                               select new { EstacionServicioID = es.ID, es.Nombre }).OrderBy(o => o.Nombre);

                    gridES.DataSource = obj;

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        
    }
}
