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
    public partial class DialogCambiarLectura : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        public int IDDisp;
        public DataTable dtMangueras;

        #endregion

        #region *** INICIO ***

        public DialogCambiarLectura(int Dispensador)
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

                var obj = (from M in db.Mangueras
                           join t in db.Tanques on M.TanqueID equals t.ID
                           where M.DispensadorID == IDDisp && M.Activo
                           select new
                           {
                               IDManguera = M.ID,
                               M.Lado,
                               MangueraNombre = M.Numero,
                               t.Color,
                               LecturaMecanica = M.LecturaMecanica,
                               LecturaEfectivo = M.LecturaEfectivo,
                               LecturaElectronica = M.LecturaElectronica
                           }).OrderBy(o => o.MangueraNombre);

                dtMangueras = ToDataTable(obj);
                gridMangueras.DataSource = dtMangueras;
                gvDataMangueras.RefreshData();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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

        private void ShowPupUpMenu()
        {
            try
            {

                DevExpress.XtraBars.BarManager barManager1;
                PopupMenu popupMenuCells;
                DevExpress.XtraBars.BarButtonItem menuButtonCopiar = new DevExpress.XtraBars.BarButtonItem();
                DevExpress.XtraBars.BarButtonItem menuButtonPegar = new DevExpress.XtraBars.BarButtonItem();

                barManager1 = new BarManager();
                barManager1.Form = this;
                popupMenuCells = new DevExpress.XtraBars.PopupMenu(barManager1);

                menuButtonCopiar.Caption = "C&opiar";
                menuButtonCopiar.Glyph = Properties.Resources.page_white_stack;
                menuButtonCopiar.Id = 1;
                menuButtonCopiar.ItemClick += new ItemClickEventHandler(menuButtonCopiar_ItemClick);

                menuButtonPegar.Caption = "P&egar";
                menuButtonPegar.Glyph = Properties.Resources.paste_plain;
                menuButtonPegar.Id = 2;
                menuButtonPegar.ItemClick += new ItemClickEventHandler(menuButtonPegar_ItemClick);
                barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { menuButtonCopiar, menuButtonPegar });

                popupMenuCells.ItemLinks.Add(barManager1.Items[0]);
                popupMenuCells.ItemLinks.Add(barManager1.Items[1]);
                barManager1.SetPopupContextMenu(this.gridMangueras, popupMenuCells);

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
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

        private void gvDataMangueras_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colLecturaMecanica)
                {
                    if (String.IsNullOrEmpty(Convert.ToString(gvDataMangueras.GetFocusedRowCellValue(colLecturaMecanica))))
                        gvDataMangueras.SetFocusedRowCellValue(colLecturaMecanica, 0m);
                }

                if (e.Column == colLecturaEfectivo)
                {
                    if (String.IsNullOrEmpty(Convert.ToString(gvDataMangueras.GetFocusedRowCellValue(colLecturaEfectivo))))
                        gvDataMangueras.SetFocusedRowCellValue(colLecturaEfectivo, 0m);
                }

                if (e.Column == colLecturaElectronica)
                {
                    if (String.IsNullOrEmpty(Convert.ToString(gvDataMangueras.GetFocusedRowCellValue(colLecturaElectronica))))
                        gvDataMangueras.SetFocusedRowCellValue(colLecturaElectronica, 0m);
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void gvDataMangueras_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            ShowPupUpMenu();  
        }

        private void menuButtonCopiar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gvDataMangueras.CopyToClipboard();
        }

        private void menuButtonPegar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
            var filas = gvDataMangueras.GetSelectedRows();
            string lista = Clipboard.GetText();

            string[] lineas = Regex.Split(lista, "\r\n");


            int i = 0;
            foreach (int obj in filas)
            {
                    int j = 0;
                    foreach (string item in lineas.ElementAt(i).Split('\t'))
                    {

                        gvDataMangueras.SetRowCellValue(obj, gvDataMangueras.GetSelectedCells(obj).ElementAt(j), item.ToString());
                        j++;
                    }
                i++;
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