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
using SAGAS.Ventas.Forms;
using DevExpress.XtraEditors.Popup;

namespace SAGAS.Ventas.Forms.Dialogs
{
    public partial class DialogPrecioServicio : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormListaServicios MDI;
        internal Entidad.Producto Servicio;
        internal Entidad.ServicioPrecio EntidadAnterior;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        internal bool Editable = false;
        private bool ShowMsg = false;

        private decimal total = 0;

        private int IDArea
        {
            get { return Convert.ToInt32(lkArea.EditValue); }
            set { lkArea.EditValue = value; }
        }

        private int IDServicio
        {
            get { return Convert.ToInt32(lkServicio.EditValue); }
            set { lkServicio.EditValue = value; }
        }

        private decimal PrecioSinIva
        {
            get { return Convert.ToDecimal(spPrecioSinIva.Value); }
            set { spPrecioSinIva.Value = value; }
        }

        private decimal PrecioTotal
        {
            get { return Convert.ToDecimal(spPrecioTotal.Value); }
            set { spPrecioTotal.Value = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogPrecioServicio()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
        }

        #endregion

        #region *** METODOS ***

        //Cargar datos
        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                lkArea.Properties.DataSource = from a in db.Areas
                                               join c in db.ProductoClases on a.ID equals c.AreaID
                                               where a.Activo && !c.ID.Equals(Parametros.Config.ProductoClaseCombustible())
                                               group a by new { a.ID, a.Nombre} into gr
                                               select new { ID = gr.Key.ID, Nombre = gr.Key.Nombre };

                if (Editable)
                {
                    IDServicio = EntidadAnterior.ServicioID;
                    IDArea = db.Areas.Single(a => a.ID.Equals(Convert.ToInt32(db.ProductoClases.Single(p => p.ID.Equals(Servicio.ProductoClaseID)).AreaID))).ID;
                    IDServicio = EntidadAnterior.ServicioID;
                    PrecioSinIva = EntidadAnterior.PrecioVenta;
                    PrecioTotal = EntidadAnterior.PrecioTotal;

                    lkArea.Enabled = false;
                    lkServicio.Enabled = false;
                    spPrecioTotal.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(lkArea, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkServicio, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (IDArea.Equals(0) || IDServicio.Equals(0))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (Parametros.General.ListSES.Count > 0)
            {
                if (IDSubEstacion <= 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (!Editable)
            {
                var obj = from ap in db.ServicioPrecio
                          where ap.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) & ap.SubEstacionID.Equals(Parametros.General.SubEstacionID)
                          & ap.ServicioID.Equals(IDServicio)
                          select ap;

                if (obj.Count() > 0)
                {
                    Parametros.General.DialogMsg("El Servicio: " + lkServicio.Text + ", ya esta ingresado en la lista de precios por servicios." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
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
                    Entidad.ServicioPrecio SP;

                    if (Editable)
                    { SP = db.ServicioPrecio.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        SP = new Entidad.ServicioPrecio();
                        SP.ServicioID = IDServicio;
                        SP.EstacionServicioID = Parametros.General.EstacionServicioID;
                        SP.SubEstacionID = Parametros.General.SubEstacionID;
                    }

                    SP.PrecioVenta = PrecioSinIva;
                    SP.PrecioTotal = PrecioTotal;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(SP, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                         "Se modificó El precio del Servicio: " + lkServicio.Text, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.ServicioPrecio.InsertOnSubmit(SP);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                        "Se creó El precio del Servicio: " + lkServicio.Text, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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

        //Identificar el Area
        private void lkArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (IDArea > 0)
                {
                    lkServicio.EditValue = null;
                    lkServicio.Properties.DataSource = null;

                    lkServicio.Properties.DataSource = from s in db.Productos
                                                       join c in db.ProductoClases on s.ProductoClaseID equals c.ID
                                                       join a in db.Areas on c.AreaID equals a.ID
                                                       where s.Activo && s.EsServicio && a.ID.Equals(IDArea) 
                                                       select new
                                                       {
                                                           Display = s.Codigo + " | " + s.Nombre,
                                                           ID = s.ID
                                                       };
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());               
            }
        }

        //Identificar el Servicio
        private void lkServicio_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                Servicio = null;
                PrecioTotal = 0;
                PrecioSinIva = 0;
                spPrecioTotal.Enabled = false;

                if (IDServicio > 0)
                {
                    Servicio = db.Productos.Single(s => s.ID.Equals(IDServicio));
                    spPrecioTotal.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void spPrecioTotal_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                decimal vPrecio;
                vPrecio = Decimal.Round(Convert.ToDecimal(spPrecioTotal.Value), 2, MidpointRounding.AwayFromZero);

                if (!Servicio.ExentoIVA)
                    PrecioSinIva = Decimal.Round((vPrecio / 1.15m), 6, MidpointRounding.AwayFromZero);
                else
                    PrecioSinIva = Decimal.Round(vPrecio, 6, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void lkArea_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        #endregion

    }


}