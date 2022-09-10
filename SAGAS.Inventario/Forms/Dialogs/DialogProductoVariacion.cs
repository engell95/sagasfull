using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using SAGAS.Contabilidad.Forms;
using DevExpress.XtraEditors.Popup;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogProductoVariacion : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormProductoVariacion MDI;
        internal Entidad.ProductoVariacion EntidadAnterior;
        internal bool Editable = false;
        public int IdPadre = 0;
        private int UsuarioID = 0;
        private string chequeName = "";
        private string ExtensionName = "";
        private bool ShowMsg = false;

        private int Estacion
        {
            get { return Convert.ToInt32(this.lkeEstacionServicio.EditValue); }
            set { this.lkeEstacionServicio.EditValue = value; }
        }

        private int SubEstacion
        {
            get { return Convert.ToInt32(this.lkeSubEstacion.EditValue); }
            set { this.lkeSubEstacion.EditValue = value; }
        }

        private int ProductoID
        {
            get { return Convert.ToInt32(this.lkProducto.EditValue); }
            set { this.lkProducto.EditValue = value; }
        }

        private decimal Variacion
        {
            get { return Convert.ToDecimal(this.spVariacion.EditValue); }
            set { this.spVariacion.EditValue = value; }
        }

        private decimal Diferencia
        {
            get { return Convert.ToDecimal(this.spDiferencia.EditValue); }
            set { this.spDiferencia.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogProductoVariacion(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                
                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;

                lkProducto.Properties.DataSource = db.Productos.Where(p => p.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()) && p.Activo).Select(s => new { s.ID, s.Nombre });
            
                if (Editable)
                {
                    
                    Estacion = EntidadAnterior.EstacionServicioID;
                    SubEstacion = EntidadAnterior.SubEstacionServicioID;
                    ProductoID = EntidadAnterior.ProductoID;
                    Variacion = EntidadAnterior.PermisibleActaVariacion;
                    Diferencia = EntidadAnterior.PermisibleActaDiferencia;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkProducto, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (lkeEstacionServicio.EditValue == null || lkProducto.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarProducto((EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
            {
                Parametros.General.DialogMsg("La Estación de Servicio " + lkeEstacionServicio.Text + (lkeSubEstacion.EditValue == null ? "" : "Sub E/S " + lkeSubEstacion.Text) + ", ya tiene asignada variaciones para " + lkProducto.Text + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// VALIDAR SERIE REPETIDA
        /// </summary>
        /// <param name="ID">ID de la Entidad Anterior</param>
        /// <returns></returns>
        private bool ValidarProducto(int? ID)
        {
            var result = (from s in db.ProductoVariacions
                          where (ID.HasValue ? s.EstacionServicioID.Equals(Estacion) && s.SubEstacionServicioID.Equals(SubEstacion) && s.ProductoID.Equals(ProductoID)
                          && s.ID != Convert.ToInt32(ID) : s.EstacionServicioID.Equals(Estacion) && s.SubEstacionServicioID.Equals(SubEstacion) && s.ProductoID.Equals(ProductoID))
                          select s);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Entidad.ProductoVariacion V;

                    if (Editable)
                    {
                        V = db.ProductoVariacions.Single(e => e.ID.Equals(EntidadAnterior.ID));
                        
                    }
                    else 
                    {
                        V = new Entidad.ProductoVariacion();
                    }

                    V.EstacionServicioID = Estacion;
                    V.SubEstacionServicioID = SubEstacion;
                    V.ProductoID = ProductoID;
                    V.PermisibleActaVariacion = Variacion;
                    V.PermisibleActaDiferencia = Diferencia;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(V, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se modificó la Variación : " + lkeEstacionServicio.Text, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.ProductoVariacions.InsertOnSubmit(V);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se creó la Variación: " + lkeEstacionServicio.Text, this.Name);
                    }

                    db.SubmitChanges();
                    
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void lkeEstacionServicio_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkeEstacionServicio.EditValue != null)
                {
                    if (db.SubEstacions.Count(sus => sus.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue))) > 0)
                    {
                        //**SubEstación
                        lkeSubEstacion.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue))).ToList();
                        lkeSubEstacion.Properties.DisplayMember = "Nombre";
                        lkeSubEstacion.Properties.ValueMember = "ID";
                    }
                    else
                    {
                        lkeSubEstacion.Properties.DataSource = null;
                        lkeSubEstacion.EditValue = null;
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