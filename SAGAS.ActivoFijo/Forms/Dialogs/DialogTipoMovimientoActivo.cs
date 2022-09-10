using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    public partial class DialogTipoMovimientoActivo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTipoMovimientoActivo MDI;
        internal Entidad.TipoMovimientoActivo EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        DXErrorProvider errRequiredField = new DXErrorProvider();


        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public bool AplicaDepreciacion 
        {
            get { return ckAplicaDepreciacion.Checked; }
            set { ckAplicaDepreciacion.Checked = value; }
        
        }

        public int RadioTipo 
        {
            get { return radioTipo.SelectedIndex; }
            set { radioTipo.SelectedIndex = value; }        
        }

        public int RadioEstado 
        {
            get { return radioEstado.SelectedIndex; }
            set { radioEstado.SelectedIndex = value; }            
        }

        #endregion
        
        
        public DialogTipoMovimientoActivo()
        {
            InitializeComponent();
        }

        private void MyFormActivo_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField(); 
        }

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    AplicaDepreciacion = EntidadAnterior.AplicaDepreciacion;
                    RadioTipo = EntidadAnterior.AplicaProveedor ? 0 : 1;//EntidadAnterior.AplicaCliente ? 1 :  2;
                    RadioEstado = 0;//EntidadAnterior.EsAlta ? 0 : EntidadAnterior.EsBaja ? 1 : 2;
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
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarNombre(txtNombre.Text, EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Activo '" + txtNombre.Text + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarNombre(string nombre, int? ID)
        {
            var result = (from i in db.Activo
                          where (ID.HasValue ? i.Nombre == nombre && i.ID != Convert.ToInt32(ID) : i.Nombre == nombre)
                          select i);

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
                    Entidad.TipoMovimientoActivo TipoMovimientoActivo;

                    if (Editable)
                    {
                        TipoMovimientoActivo = db.TipoMovimientoActivo.Single(e => e.ID == EntidadAnterior.ID);
                    }

                    else
                    {
                        TipoMovimientoActivo = new Entidad.TipoMovimientoActivo();
                        TipoMovimientoActivo.Activo = true;
                    }

                    TipoMovimientoActivo.Nombre = Nombre;
                    TipoMovimientoActivo.AplicaDepreciacion = AplicaDepreciacion;
                    TipoMovimientoActivo.AplicaProveedor = RadioTipo == 0 ? true : false;
                    TipoMovimientoActivo.AplicaCliente = RadioTipo == 1 ? true : false;
                    TipoMovimientoActivo.EsAlta = RadioEstado == 0 ? true : false;
                    TipoMovimientoActivo.EsBaja = RadioEstado == 1 ? true : false;
                    TipoMovimientoActivo.EsTraslado = RadioEstado == 2 ? true : false;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(TipoMovimientoActivo, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo, "Se modificó el Tipo de Movimiento Activo: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.TipoMovimientoActivo.InsertOnSubmit(TipoMovimientoActivo);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo, "Se creó el Tipo de Movimiento Activo: " + TipoMovimientoActivo.Nombre, this.Name);

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

        private void btnGuardar_Click(object sender, EventArgs e)
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
            Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.TextEdit)sender, errRequiredField);
        }

        #endregion 
    }
}
