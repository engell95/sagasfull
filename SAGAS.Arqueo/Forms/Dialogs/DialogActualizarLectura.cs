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
using DevExpress.XtraBars;
using System.Text.RegularExpressions;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogActualizarLectura : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        public int IDDisp;
        public DataTable dtMangueras;

        #endregion

        #region *** INICIO ***

        public DialogActualizarLectura(int Dispensador)
        {
            InitializeComponent();
            IDDisp = Dispensador;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private System.Data.DataTable ToDataTable(object query)
        {
            if (query == null)
                throw new ArgumentNullException("Consulta no especificada!");

            System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            System.Data.DataTable dt = new System.Data.DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, System.Data.SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                var obj = (from m in db.Mangueras
                           join d in db.Dispensadors on m.DispensadorID equals d.ID
                           join t in db.Tanques on m.TanqueID equals t.ID
                           join VAI in
                               (
                                from am in db.ArqueoMangueras
                                join ap in db.ArqueoProductos on am.ArqueoProductoID equals ap.ID
                                join ai in db.ArqueoIslas on ap.ArqueoIslaID equals ai.ID
                                where ai.IslaID == 18 && ai.ID == 5561
                                select new
                                {
                                    ai.ID,
                                    ai.IslaID,
                                    am.MangueraID,
                                    am.LecturaMecanicaInicial,
                                    am.LecturaEfectivoInicial,
                                    am.LecturaElectronicaInicial
                                }
                               ) on m.ID equals VAI.MangueraID
                           where d.ID == 16
                           select new
                           {
                               IDManguera = m.ID,
                               m.Numero,
                               t.Color,
                               IDDispensador = d.ID,
                               d.IslaID,
                               LecturaMecanicaPosterior = m.LecturaMecanica,
                               LecturaEfectivoPosterior = m.LecturaEfectivo,
                               LecturaElectronicaPosterior = m.LecturaElectronica,
                               LecturaMecanicaAnterior = VAI.LecturaMecanicaInicial,
                               LecturaEfectivoAnterior = VAI.LecturaEfectivoInicial,
                               LecturaElectronicaAnterior = VAI.LecturaElectronicaInicial
                           }).OrderBy(o => o.Numero);

                dtMangueras = ToDataTable(obj);
                gridMangueras.DataSource = dtMangueras;
                bandedGridView1.RefreshData();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }
         
        private bool Guardar()
        {

            return true;

            //if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            //using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            //{
            //    db.Transaction = trans;
            //    try
            //    {
            //        Entidad.Dispensador D;

            //        if (Editable)
            //        { D = db.Dispensadors.Single(e => e.ID == EntidadAnterior.ID); }
            //        else 
            //        {
            //            D = new Entidad.Dispensador();
            //            D.Activo = true;
            //        }

            //        D.Nombre = Nombre;

            //        D.Modelo = Modelo;
            //        D.Serie = Serie;
            //        D.Marca = Marca;
            //        D.CodigoDNP = CodigoDNP;
            //        D.Comentario = Comentario;
            //        D.IslaID = IDIsla;
            //        D.LecturaEfectivo = Efectivo;
            //        D.LecturaElectronica = Electronica;
            //        D.LecturaMecanica = Mecanica;

            //        if (Editable)
            //        {
            //            DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(D, 1));
            //            DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

            //            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
            //             "Se modificó el Dispensador: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

            //        }
            //        else
            //        {
            //            db.Dispensadors.InsertOnSubmit(D);
            //            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
            //            "Se creó el Dispensador: " + D.Nombre, this.Name);

            //        }

            //        db.SubmitChanges();
            //        trans.Commit();

            //        ShowMsg = true;
            //        return true;
            //    }

            //    catch (Exception ex)
            //    {
            //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            //        return false;
            //    }
            //    finally
            //    {
            //        if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
            //    }
            //}
        }

      

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }
     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  
        #endregion
    }
}