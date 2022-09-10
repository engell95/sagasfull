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
using SAGAS.Administracion.Forms;

namespace SAGAS.Administracion.Forms.Dialogs
{
    public partial class DialogReportes : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormReportes MDI;
        internal Entidad.Reporte EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private string route;
        private List<Entidad.ReportesEstructura> EtEstructura = new List<Entidad.ReportesEstructura>();
        private string reporteName = "";
        private string ExtensionName = "";
        //internal List<Parametros.ListIdDisplay> ListaEstructura;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }


        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public int IDModulo
        {
            get { return Convert.ToInt32(lkeModulo.EditValue); }
            set { lkeModulo.EditValue = value; }
        }

        public int IDEstructura
        {
            get { return Convert.ToInt32(lkEstructura.EditValue); }
            set { lkEstructura.EditValue = value; }
        }

        public int Orden
        {
            get { return Convert.ToInt32(speOrden.Value); }
            set { speOrden.EditValue = value; }
        }

        //private Byte[] Archivo
        //{
        //    get
        //    {
        //        if (picImagen.Image == null) return null;
        //        else return Parametros.General.ImageToBytes(picImagen.Image);
        //    }
        //    set
        //    {
        //        if (value == null) picImagen.Image = null;
        //        else picImagen.Image = Parametros.General.BytesToImage(value);
        //    }
        //}


        #endregion

        #region *** INICIO ***

        public DialogReportes()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl(); 
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                //ListaEstructura = new List<Parametros.ListIdDisplay>();
                //ListaEstructura.Add(new Parametros.ListIdDisplay { ID = 0, Display = "Vía Tunel" });
                //ListaEstructura.Add(new Parametros.ListIdDisplay { ID = 1, Display = "Vía Internet" });
                //lkEstructura.Properties.DataSource = ListaEstructura;
                lkeModulo.Properties.DataSource = from m in db.Modulos where m.Activo select new { m.ID, m.Nombre };
                lkeModulo.Properties.DisplayMember = "Nombre";
                lkeModulo.Properties.ValueMember = "ID";

                EtEstructura = db.ReportesEstructuras.ToList();

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Comentario = EntidadAnterior.Comentario;
                    IDModulo = EntidadAnterior.ModuloID;
                    Orden = EntidadAnterior.Orden;
                    chkSubReporte.Checked = EntidadAnterior.EsSubReporte;
                    IDEstructura = EntidadAnterior.EstructuraID;
                    this.btnUpload.Text = EntidadAnterior.ArchivoNombre;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
             }

        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkeModulo.EditValue == null || speOrden.Value < 1 || lkEstructura.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!Editable && String.IsNullOrEmpty(route))
            {
                Parametros.General.DialogMsg("Debe de seleccionar el archivo de REPORTE" + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (Editable && !String.IsNullOrEmpty(route))
            {
                if (Parametros.General.DialogMsg("Un archivo de REPORTE fue seleccionado, ¿Desea reemplazarlo?" + Environment.NewLine, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.Cancel)
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
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                    Entidad.Reporte R;

                    if (Editable)
                    {
                        R = db.Reportes.Single(e => e.ID == EntidadAnterior.ID);
                        if (!String.IsNullOrEmpty(route))
                        {
                            R.Archivo = File.ReadAllBytes(route);
                            R.Fecha = Convert.ToDateTime(db.GetDateServer());
                            R.ArchivoNombre = reporteName;
                            R.Extension = ExtensionName;
                        }
                    }
                    else
                    {
                        R = new Entidad.Reporte();
                        R.Archivo = File.ReadAllBytes(route);
                        R.Fecha = Convert.ToDateTime(db.GetDateServer());
                        R.ArchivoNombre = reporteName;
                        R.Extension = ExtensionName;
                    }

                    R.Nombre = Nombre;
                    R.Comentario = Comentario;
                    R.ModuloID = IDModulo;
                    R.Orden = Orden;
                    R.EstructuraID = IDEstructura;
                    R.EsSubReporte = Convert.ToBoolean(chkSubReporte.Checked);
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(R, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se editó el Reporte: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Reportes.InsertOnSubmit(R);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se subió el Reporte: " + R.Nombre, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //ofdReport.Filter = "Reportes |*.repx";

                if (ofdReport.ShowDialog() == DialogResult.OK)
                {
                    this.btnUpload.Text = Path.GetFileName(@ofdReport.FileName);
                    this.reporteName  = Path.GetFileNameWithoutExtension(@ofdReport.FileName);
                    this.ExtensionName = Path.GetExtension(@ofdReport.FileName);
                    route = @ofdReport.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("INTERRUPCION AL CARGAR EL ARCHIVO" + ex.Message);
            }
        }

        private void lkeModulo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkeModulo.EditValue != null)
                {
                    lkEstructura.EditValue = null;
                    lkEstructura.Properties.DataSource = db.ReportesEstructuras.Where(o => o.ModuloID.Equals(Convert.ToInt32(lkeModulo.EditValue))).Select(s => new { s.ID, s.Nombre }).ToList();
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