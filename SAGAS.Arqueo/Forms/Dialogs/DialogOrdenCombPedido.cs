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
using SAGAS.Arqueo.Forms;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogOrdenCombPedido : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormOrdenCombPedido MDI;
        internal Entidad.OrdenCombustiblePedido EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;
        private int CombustibleID;

        public int IDProducto
        {
            get { return Convert.ToInt32(lkeProducto.EditValue); }
            set { lkeProducto.EditValue = value; }
        }

        public int Capacidad
        {
            get { return Convert.ToInt32(speCapacidad.EditValue); }
            set { speCapacidad.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogOrdenCombPedido(int UserID, int ComID)
        {
            InitializeComponent();
            UsuarioID = UserID;
            CombustibleID = ComID;
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

                //**Produto
                lkeProducto.Properties.DataSource = from p in db.Productos
                                                    where p.Activo && p.ProductoClaseID == CombustibleID
                                                    select new { p.ID, p.Nombre };
                lkeProducto.Properties.DisplayMember = "Nombre";
                lkeProducto.Properties.ValueMember = "ID";

                if (Editable)
                {
                    IDProducto = EntidadAnterior.ProductoID;
                    Capacidad = EntidadAnterior.Orden;
                    
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(lkeProducto, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if ( lkeProducto.EditValue == null )
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (db.OrdenCombustiblePedidos.Count( c => c.ProductoID.Equals(IDProducto)) > 0 && !Editable)
            {
                Parametros.General.DialogMsg("El producto seleccionado ya existe en los registros del sistema." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (db.OrdenCombustiblePedidos.Count(c => c.Orden.Equals(Capacidad)) > 0)
            {
                Parametros.General.DialogMsg("El orden seleccionado ya existe en los registros del sistema." + Environment.NewLine, Parametros.MsgType.warning);
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
                    Entidad.OrdenCombustiblePedido T;

                    if (Editable)
                    { T = db.OrdenCombustiblePedidos.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        T = new Entidad.OrdenCombustiblePedido();
                    }

                    T.ProductoID = IDProducto;
                    T.Orden = Capacidad;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(T, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        // "Se modificó el Tanque: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.OrdenCombustiblePedidos.InsertOnSubmit(T);
                        //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        //"Se creó el Tanque: " + T.Nombre, this.Name);

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
            MDI.CleanDialog(ShowMsg);
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
           Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
            Parametros.General.ValidateDifES((BaseEdit)sender, infDifES);
        }

        #endregion  


    }
}