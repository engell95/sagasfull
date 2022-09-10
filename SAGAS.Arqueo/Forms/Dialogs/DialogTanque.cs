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
    public partial class DialogTanque : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTanque MDI;
        internal Entidad.Tanque EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;
        private int CombustibleID;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Marca
        {
            get { return txtMarca.Text; }
            set { txtMarca.Text = value; }
        }

        public string Serie
        {
            get { return txtSerie.Text; }
            set { txtSerie.Text = value; }
        }

        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public int IDProducto
        {
            get { return Convert.ToInt32(lkeProducto.EditValue); }
            set { lkeProducto.EditValue = value; }
        }

        public int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkeEstacionServicio.EditValue); }
            set { lkeEstacionServicio.EditValue = value; }
        }

        public int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }

        public string Proveedor
        {
            get { return txtProveedor.Text; }
            set { txtProveedor.Text = value; }
        }

        public int IDTipoTanque
        {
            get { return Convert.ToInt32(lkeTipoTanque.EditValue); }
            set { lkeTipoTanque.EditValue = value; }
        }         

        public decimal Litros
        {
            get { return Convert.ToDecimal(speLitros.EditValue); }
            set { speLitros.EditValue = value; }
        }

        public decimal Capacidad
        {
            get { return Convert.ToDecimal(speCapacidad.EditValue); }
            set { speCapacidad.EditValue = value; }
        }

        public decimal Diametro
        {
            get { return Convert.ToDecimal(speDiametro.EditValue); }
            set { speDiametro.EditValue = value; }
        }

        public decimal Longitud
        {
            get { return Convert.ToDecimal(speLongitud.EditValue); }
            set { speLongitud.EditValue = value; }
        }

        public string Altura
        {
            get { return txtAltura.Text; }
            set { txtAltura.Text = value; }
        }

        public int colorCombustible
        {
            get { return Convert.ToInt32(this.gridLookUpEdit1.EditValue); }
            set { this.gridLookUpEdit1.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogTanque(int UserID, int ComID)
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

                //**EstacionServicio
                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                lkeEstacionServicio.Properties.DisplayMember = "Nombre";
                lkeEstacionServicio.Properties.ValueMember = "ID";
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;

                //**SubEstación
                lkSES.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).ToList();
                
                lkSES.Properties.DisplayMember = "Nombre";
                lkSES.Properties.ValueMember = "ID";
            
                //**TipoTanque
                lkeTipoTanque.Properties.DataSource = from tp in db.TipoTanques
                                           where tp.Activo
                                           select new { tp.ID, tp.Nombre };

                lkeTipoTanque.Properties.DisplayMember = "Nombre";
                lkeTipoTanque.Properties.ValueMember = "ID";

                //**COLOR
                gridLookUpEdit1.Properties.DataSource = from c in db.Colors
                                                        where c.Activo
                                                        select new { c.Colored };
                gridLookUpEdit1.Properties.DisplayMember = "Colored";
                gridLookUpEdit1.Properties.ValueMember = "Colored";


                if (db.Tanques.Count(t => t.Activo && t.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)) > 0)
                {
                    int numero = Parametros.General.ValorConsecutivo(db.Tanques.Where(t => t.Activo && t.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)).OrderByDescending(o => o.Nombre).First().Nombre);
                    Nombre = "T " + numero.ToString();
                }
              
                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Marca = EntidadAnterior.Marca;
                    Serie = EntidadAnterior.Serie;
                    Comentario = EntidadAnterior.Comentario;
                    IDProducto = EntidadAnterior.ProductoID;
                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    Proveedor = EntidadAnterior.Proveedor;
                    IDTipoTanque = EntidadAnterior.TipoTanqueID;
                    Litros = EntidadAnterior.Litros;
                    Capacidad = EntidadAnterior.Capacidad;
                    Diametro = EntidadAnterior.Diametro;
                    Longitud = EntidadAnterior.Longitud;
                    IDSubEstacion = EntidadAnterior.SubEstacionID;
                    Altura = EntidadAnterior.Altura;
                    colorCombustible = EntidadAnterior.Color;
                    
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeProducto, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtMarca, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtSerie, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtProveedor, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeTipoTanque, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkeProducto.EditValue == null || lkeEstacionServicio.EditValue == null || txtMarca.Text == "" || txtSerie.Text == "" || txtProveedor.Text == "" || lkeTipoTanque.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
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
                    Entidad.Tanque T;

                    if (Editable)
                    { T = db.Tanques.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        T = new Entidad.Tanque();
                        T.Activo = true;
                    }

                    T.Nombre = Nombre;
                    T.Marca = Marca;
                    T.Serie = Serie;
                    T.Comentario = Comentario; 
                    T.ProductoID = IDProducto;
                    T.Proveedor = Proveedor;
                    T.EstacionServicioID = IDEstacionServicio;
                    T.TipoTanqueID = IDTipoTanque;
                    T.Litros = Litros; 
                    T.Capacidad = Capacidad;
                    T.Diametro = Diametro;
                    T.Longitud = Longitud;
                    T.Altura = Altura;
                    T.SubEstacionID = IDSubEstacion;
                    T.Color = colorCombustible;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(T, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                         "Se modificó el Tanque: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Tanques.InsertOnSubmit(T);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se creó el Tanque: " + T.Nombre, this.Name);

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

        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.gridLookUpEdit1.ForeColor = Color.FromArgb(Convert.ToInt32(this.gridLookUpEdit1.EditValue));
                this.gridLookUpEdit1.BackColor = Color.FromArgb(Convert.ToInt32(this.gridLookUpEdit1.EditValue));
            }
            catch { }
        }

    }
}