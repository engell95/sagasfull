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
using SAGAS.Inventario.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DevExpress.Data.Linq;
using System.Data.Linq.SqlClient;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogInventarioCombustible : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormInventarioCombustible MDI;
        internal Entidad.InventarioCombustible EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private bool NextDialog = false;
        private bool _Changes = false;
        private DataTable dtInventario = new DataTable();
        private List<Parametros.ListIdTIDTanqueDisplayPriceValue> Equivalente = new List<Parametros.ListIdTIDTanqueDisplayPriceValue>();

        private bool EsInventarioCombustibleCero = false;
        private string Referencia
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Observacion
        {
            get { return memoObservacion.Text; }
            set { memoObservacion.Text = value; }
        }


        private int Cantidad
        {
            get { return Convert.ToInt32(spCantidad.Value); }
            set { spCantidad.Value = (Decimal)value; }
        }

        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogInventarioCombustible(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;

            if (Editable)
            { //-- Bloquear Controles --//    
                txtNumero.Properties.ReadOnly = true;
            }
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            //rgCreditoContado.SelectedIndex = 0;
            //IDProveedor = 186;
            //Referencia = "123";
            //IDAlmacen = 1;
        }
        
        private void DialogCompras_Shown(object sender, EventArgs e)
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

                int number = 1;
                if (db.InventarioCombustibles.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.InventarioCombustibles.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                Fecha = Convert.ToDateTime(db.GetDateServer()).Date;

                EsInventarioCombustibleCero = db.EstacionServicios.Single(s => s.ID.Equals(IDEstacionServicio)).InventarioCombustibleCero;

                txtNumero.Text = number.ToString("000000000");

                if (Editable)
                {
                    Referencia = EntidadAnterior.Numero.ToString("000000000");
                    Fecha = EntidadAnterior.FechaInventario;
                    Cantidad = EntidadAnterior.InventarioCombustibleDetalles.GroupBy(g => g.MedicionNumero).Count();
                    Observacion = EntidadAnterior.Observacion;

                    btnLoad_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoObservacion, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNumero.EditValue == null || dateFecha.EditValue == null || spCantidad.Value <= 0 || String.IsNullOrEmpty(Observacion))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la compra.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
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
                if (db.InventarioCombustibles.Count(c => c.EstacionServicioID.Equals(IDEstacionServicio) && c.SubEstacionID.Equals(IDSubEstacion) && c.FechaInventario.Date.Equals(Fecha.Date)) > 0)
                {
                    Parametros.General.DialogMsg("El Inventario de Combustible para esta fecha ya esta creado.", Parametros.MsgType.warning);
                    return false;
                }
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            bool IsNull = false;

            foreach (DataRow row in dtInventario.Rows)
            {
                foreach (DataColumn col in dtInventario.Columns)
                {
                    object value = row[col.ColumnName];

                    if (String.IsNullOrEmpty(Convert.ToString(value)))
                    {
                        IsNull = true;
                        break;
                    }

                }

                if (IsNull)
                    break;
            }

            if (IsNull)
            {
                Parametros.General.DialogMsg("Debe completar la tabla de lecturas.", Parametros.MsgType.warning);
                return false;
            }

            if (!EsInventarioCombustibleCero)
            {
                if (Equivalente.Sum(s => s.Price).Equals(0) || Equivalente.Sum(s => s.Value).Equals(0))
                {
                    Parametros.General.DialogMsg("Debe Ingresar los Litros o Galones.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.InventarioCombustible INV;

                    if (Editable)
                    {
                        INV = db.InventarioCombustibles.Single(e => e.ID == EntidadAnterior.ID);

                        if (INV.Finalizado.Equals(true))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("Este Inventario de Combustible ya fue finalizado." + Environment.NewLine, Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }

                        INV.InventarioCombustibleDetalles.Clear();
                        INV.InventarioCombustibleValores.Clear();
                    }
                    else
                    {
                        INV = new Entidad.InventarioCombustible();
                        INV.FechaCreado = Convert.ToDateTime(db.GetDateServer());

                        int number = 1;
                        if (db.InventarioCombustibles.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.InventarioCombustibles.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        INV.Numero = number;
                    }

                    INV.EstacionServicioID = IDEstacionServicio;
                    INV.SubEstacionID = IDSubEstacion;                    
                    INV.FechaInventario = Fecha;
                    INV.Observacion = Observacion;
                   
                    INV.UserID = UsuarioID;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(INV, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó el Inventario de Combustible: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.InventarioCombustibles.InsertOnSubmit(INV);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el Inventario de Combustible: " + INV.Numero, this.Name);

                    }

                    db.SubmitChanges();

                    #region ::: REGISTRANDO EXISTENCIAS EN DETALLE :::

                    //INVENTARIO COMBUSTIBLE DETALLE
                    foreach (DataRow row in dtInventario.Rows)
                    {
                        int IDT = 0;
                        foreach (DataColumn col in row.Table.Columns)
                        {                            
                            if (col.ColumnName.Equals("ID"))
                                IDT = Convert.ToInt32(row["ID"]);

                            if (!col.ColumnName.Equals("ID") && !col.ColumnName.Equals("Datos"))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(row[col])))
                                {
                                    INV.InventarioCombustibleDetalles.Add(new Entidad.InventarioCombustibleDetalle { TanqueID = IDT, MedicionNumero = Convert.ToInt32(col.ColumnName), MedicionValor = Convert.ToString(row[col]) });
                                    db.SubmitChanges();
                                }
                            }
                        }

                    }
                    db.SubmitChanges();
                    //INVENTARIO COMBUSTIBLE VALORES

                    Equivalente.ForEach(line =>
                        {
                            INV.InventarioCombustibleValores.Add(new Entidad.InventarioCombustibleValore { ProductoID = line.ID, TanqueID = line.TID, Litros = line.Price, Galones = line.Value });
                            db.SubmitChanges();
                        });
                    db.SubmitChanges();
                    #endregion

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    return true;

                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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

        private DevExpress.XtraGrid.StyleFormatCondition styleFormatConditioner(List<int> lista)
        {
            try
            {
                DevExpress.XtraGrid.StyleFormatCondition style = new DevExpress.XtraGrid.StyleFormatCondition();

                //lista.ForEach(line =>
                //    {
                        Color c = Color.FromArgb(lista.First());

                        style.Appearance.BackColor = c;// System.Drawing.Color.FromArgb(line);
                        style.Appearance.Options.UseBackColor = true;
                        style.ApplyToRow = false;
                        style.Column = colDatos;
                        style.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                        style.Expression = "[" + colColor.FieldName + "] = " + lista.First().ToString();
                        //colColor
                    //});

                
                
                
                
                return style;
            }
            catch
            { return null; }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI, NextDialog);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (btnLoad.Enabled.Equals(false))
                {
                    if (Parametros.General.DialogMsg("El Inventario de Combustible actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        NextDialog = false;
                        e.Cancel = true;
                    }
                }
            }

            EntidadAnterior = null;
        }

        private void bntNew_Click(object sender, EventArgs e)
        {
             NextDialog = true;
             this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }
       
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {

                    Entidad.ResumenDia RD = db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDEstacionServicio) && r.SubEstacionID.Equals(IDSubEstacion) && r.FechaInicial.Date.Equals(Fecha.Date)).FirstOrDefault();

                    if (RD != null)
                    {
                        Entidad.Turno T = RD.Turnos.OrderByDescending(t => t.Numero).FirstOrDefault();

                        if (T != null)
                        {

                            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (Editable ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));

                            var query = (from am in db.ArqueoMangueras
                                         join m in db.Mangueras on am.MangueraID equals m.ID
                                         join d in db.Dispensadors on m.DispensadorID equals d.ID
                                         join i in db.Islas on d.IslaID equals i.ID
                                         join ap in db.ArqueoProductos on am.ArqueoProductoID equals ap.ID
                                         join p in db.Productos on ap.ProductoID equals p.ID
                                         join ai in db.ArqueoIslas on ap.ArqueoIslaID equals ai.ID
                                         join t in db.Turnos on ai.TurnoID equals t.ID
                                         join r in db.ResumenDias on t.ResumenDiaID equals r.ID
                                         where r.ID.Equals(RD.ID) && t.ID.Equals(T.ID) && d.Activo
                                         && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && t.Especial))
                                         select new
                                         {
                                             ResumenDiaID = r.ID,
                                             r.FechaInicial,
                                             Turno = t.ID,
                                             Combustible = p.Codigo + " | " + p.Nombre,
                                             Isla = i.Nombre,
                                             Manguera = m.Numero,
                                             Sello = d.Sello,
                                             Lado = "Lado " + m.Lado,
                                             Dispensador = i.Nombre + " | " + d.Nombre,
                                             am.LecturaElectronicaFinal,
                                             am.LecturaMecanicaFinal,
                                             am.LecturaEfectivoFinal
                                         }).ToList();

                            this.pgData.DataSource = query;

                            dtInventario.Columns.Add("ID", typeof(Int32));
                            dtInventario.Columns.Add("Datos", typeof(String));

                            var Lecturas = (from p in db.Productos
                                            join t in db.Tanques on p.ID equals t.ProductoID
                                            where t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion) && t.Activo
                                            select new
                                            {
                                                ID = t.ID,
                                                IDP = p.ID,
                                                Tanque = t.Nombre,
                                                Datos = t.Nombre + " => " + p.Nombre,
                                                Combustible = p.Nombre,
                                                t.Color
                                            }).ToList();

                            for (int i = 1; i <= Cantidad; i++)
                            {
                                dtInventario.Columns.Add(new DataColumn { ColumnName = i.ToString(), DataType = typeof(String), DefaultValue = "" });

                                var colAlma = new Parametros.MyBandColumn("Medición " + i.ToString(), i.ToString(), null);
                                colAlma.Tag = i;
                                colAlma.OptionsFilter.AllowAutoFilter = false;
                                colAlma.OptionsFilter.AllowFilter = false;
                                colAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                colAlma.OptionsColumn.AllowEdit = true;

                                BandDatos.Columns.Add(colAlma);
                            }

                            colDatos.Width = 125;

                            Lecturas.ForEach(line =>
                            {
                                dtInventario.Rows.Add(line.ID, line.Datos);
                                                                
                                Equivalente.Add(new Parametros.ListIdTIDTanqueDisplayPriceValue { ID = line.IDP, TID = line.ID, Tanque = line.Tanque, Display = line.Combustible, Price = 0.000m, Value = 0.000m });
                                
                            });

                            if (Editable)
                            {
                                EntidadAnterior.InventarioCombustibleDetalles.ToList().ForEach(line =>
                                    {
                                        DataRow[] ESRow = dtInventario.Select("ID = " + line.TanqueID);
                                        DataRow row = ESRow.FirstOrDefault();

                                        if (row != null)
                                            row[line.MedicionNumero.ToString()] = line.MedicionValor;
                                    });

                                foreach (var obj in Equivalente)
                                {
                                    Entidad.InventarioCombustibleValore icv = EntidadAnterior.InventarioCombustibleValores.SingleOrDefault(i => i.ProductoID.Equals(obj.ID) && i.TanqueID.Equals(obj.TID));

                                    if (icv != null)
                                    {
                                        obj.Price = icv.Litros;
                                        obj.Value = icv.Galones;
                                    }
                                }
                            }

                            bdsDetalle.DataSource = dtInventario;
                            gridEquivalentes.DataSource = Equivalente;

                            gvDataEquivalente.RefreshData();
                            bgvDataLecturas.RefreshData();
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            this.dateFecha.Enabled = false;
                            this.spCantidad.Enabled = false;
                            btnLoad.Enabled = false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    if (Editable)
                        this.BeginInvoke(new MethodInvoker(this.Close)); 
                }
            }
            else
            {
                if (Editable)
                    this.BeginInvoke(new MethodInvoker(this.Close)); 
            }
        }

        private void gvDataEquivalente_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region <<< CALCULOS_MONTOS >>>
            try
            {
                //--  Calcular las montos de la Venta
                if (!_Changes && (e.Column == colLitros || e.Column == colGalones))
                {
                    decimal vLitros = 0, vGalones = 0;

                    if (gvDataEquivalente.GetRowCellValue(e.RowHandle, "Price") != null)
                        vLitros = Convert.ToDecimal(gvDataEquivalente.GetRowCellValue(e.RowHandle, "Price"));

                    if (gvDataEquivalente.GetRowCellValue(e.RowHandle, "Value") != null)
                        vGalones = Convert.ToDecimal(gvDataEquivalente.GetRowCellValue(e.RowHandle, "Value"));

                    _Changes = true;
                    decimal cons = 3.7854m;

                    if (e.Column == colLitros)
                        gvDataEquivalente.SetRowCellValue(e.RowHandle, "Value", Decimal.Round(vLitros / cons, 3, MidpointRounding.AwayFromZero));
                    else if (e.Column == colGalones)
                        gvDataEquivalente.SetRowCellValue(e.RowHandle, "Price", Decimal.Round(vGalones * cons, 3, MidpointRounding.AwayFromZero));
                   
                    _Changes = false;
                    gvDataEquivalente.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            #endregion

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F7))
            {
                btnOK_Click_1(null, null);
                return true;
            }

            if (keyData == (Keys.Control | Keys.N))
            {
                bntNew_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }        

        #endregion

    }
}