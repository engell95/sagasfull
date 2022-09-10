using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAGAS.Administracion.Forms;

namespace SAGAS.Administracion.Forms.Wizards
{
	public partial class wizAddUsuario : Form
	{

		#region <<< DECLARACIONES >>>
		
		private Entidad.SAGASDataClassesDataContext db;
		internal Forms.FormUsuario MDI;
		internal Entidad.Usuario EntidadAnterior;
		internal bool Editable = false;
        internal bool Clonar = false;
        internal protected bool loaded = false;
		private bool ShowMsg = false;
		private DataTable dtEstacionesServicios;
		private DataTable dtAccesos;
		public DataTable dtAccesosPermitidos;
		private DataTable dtReportes;
		public DataTable dtReportesPermitidos;
		private DataTable dtSES;
		private List<Parametros.ListIdDisplay> listadoES;

		private string Nombre
		{
			get { return txtNombre.Text; }
			set { txtNombre.Text = value; }
		}

		private string Login
		{
			get { return txtLogin.Text; }
			set { txtLogin.Text = value; }
		}

		public int IDEmpleado
		{
			get { return Convert.ToInt32(lkeEmpleado.EditValue); }
			set { lkeEmpleado.EditValue = value; }
		}

		public DataTable AccesoPermitidos
		{
			get
			{
				return dtAccesosPermitidos;
			}
			set
			{
				dtAccesosPermitidos = value;
			}
		}

		public DataTable ReportesPermitidos
		{
			get
			{
				return dtReportesPermitidos;
			}
			set
			{
				dtReportesPermitidos = value;
			}
		}

		#endregion

		#region <<< INICIO >>>

		public wizAddUsuario()
		{
			InitializeComponent();
		}

		private void wizGraduated_Load(object sender, EventArgs e)
		{
			FillControl();
			ValidarerrRequiredField();
		}
		  
		#endregion

		#region Metodos   

		private void FillControl()
		{
			try
			{
				db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
				lkeEmpleado.Properties.DataSource = from ep in db.Empleados where ep.Activo select new { ep.ID, Nombre = ep.Nombres + " " + ep.Apellidos };
				lkeEmpleado.Properties.DisplayMember = "Nombre";
				lkeEmpleado.Properties.ValueMember = "ID";


				var listSucursales = (from ES in db.EstacionServicios
									 where ES.Activo
									 select new { ES.ID, ES.Nombre, SelectedES = ES.Activo }).OrderBy(o => o.Nombre);

				this.dtEstacionesServicios = ToDataTable(listSucursales);

				this.grid.DataSource = dtEstacionesServicios;

				dtSES = new DataTable();
				dtSES.Columns.Add("IDES", typeof(Int32));
				dtSES.Columns.Add("IDSES", typeof(Int32));
				dtSES.Columns.Add("SelectedSES", typeof(Boolean));
				dtSES.Columns.Add("NombreSES", typeof(String));
				   
				if (Editable)
				{
					layoutControlItemESUse.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

					IQueryable<Parametros.ListIdDisplay> listEm = ((from es in db.EstacionServicios
																	where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == EntidadAnterior.ID && ges.EstacionServicioID == es.ID))
																	select new Parametros.ListIdDisplay { ID = es.ID, Display = es.Nombre}).OrderBy(o => o.Display));

					listadoES = new List<Parametros.ListIdDisplay>(listEm);

					lkESUse.Properties.DataSource = listadoES;

					lkESUse.Properties.DisplayMember = "Display";
					lkESUse.Properties.ValueMember = "ID";

                    if (!Clonar)
                    {
                        IDEmpleado = EntidadAnterior.EmpleadoID;
                        Nombre = EntidadAnterior.Nombre;
                        Login = EntidadAnterior.Login;
                        lkESUse.EditValue = EntidadAnterior.EstacionServicioID;
                    }

					foreach (var obj in listSucursales.Where(ls => !db.GetViewEstacionesServicioByUsers.Where(ges => ges.EstacionServicioID == ls.ID && ges.UsuarioID == EntidadAnterior.ID).Any()))
					{
						
						DataRow[] ESRow = dtEstacionesServicios.Select("ID = " + obj.ID);
						DataRow row = ESRow.First();
						row["SelectedES"] = false;
					}
			
					var accesosPermitidos = (from u in db.Usuarios
											 join asis in db.AccesoSistemas on u.ID equals asis.UsuarioID
											 join a in db.Accesos on asis.AccesoID equals a.ID
											 join M in db.Modulos on a.ModuloID equals M.ID
											 where u.ID == EntidadAnterior.ID
											 select new { asis.AccesoID, AccesoNombre = a.Nombre, a.ModuloID, ModuloNombre = M.Nombre, a.PermiteMenu, asis.Agregar, asis.Modificar, asis.Anular, asis.Imprimir, asis.Exportar }
											 ).Distinct();

					dtAccesosPermitidos = ToDataTable(accesosPermitidos);

					var reportePermitidos = (from u in db.Usuarios
											 join ar in db.AccesosReportes on u.ID equals ar.UsuarioID
											 join r in db.ReportesSSRs on ar.ReportesSSRSID equals r.ID
											 join M in db.Modulos on r.ModuloID equals M.ID
											 where u.ID == EntidadAnterior.ID
											 select new { ar.ReportesSSRSID, ReporteNombre = r.Nombre, r.ModuloID, ModuloNombre = M.Nombre, r.EsSubReporte, ar.Agregar, ar.Modificar, ar.Anular, ar.Imprimir, ar.Exportar }
											 ).Distinct();

					dtReportesPermitidos = ToDataTable(reportePermitidos);

					var listaSES = from us in db.UsuarioSubEstacions
								   join u in db.Usuarios on us.UsuarioID equals u.ID
								   join ses in db.SubEstacions on us.SubEstacionID equals ses.ID
								   join es in db.EstacionServicios on ses.EstacionServicioID equals es.ID
								   where u.ID.Equals(EntidadAnterior.ID)
								   select new
								   {
									   IDES = es.ID,
									   IDSES = ses.ID,
									   SelectedSES = ses.Activo,
									   NombreSES = ses.Nombre
								   };

					listaSES.ToList().ForEach(us => { dtSES.Rows.Add(us.IDES, us.IDSES, us.SelectedSES, us.NombreSES); });
					//dtSES = Parametros.General.LINQToDataTable(listaSES);

				}
				else
				{
					for (int i = 0; i < gvData.RowCount; i++)
					{
						this.gvData.SetRowCellValue(i, "SelectedES", false);
					}

					dtAccesosPermitidos = new DataTable();

					if (dtAccesosPermitidos.Columns.Count == 0)
					{  
						//dtAccesosPermitidos.Columns.Add("ID", typeof(Int32));
						dtAccesosPermitidos.Columns.Add("AccesoID", typeof(Int32));
						dtAccesosPermitidos.Columns.Add("AccesoNombre", typeof(String));
						dtAccesosPermitidos.Columns.Add("ModuloID", typeof(Int32));
						dtAccesosPermitidos.Columns.Add("ModuloNombre", typeof(String));
						dtAccesosPermitidos.Columns.Add("PermiteMenu", typeof(Boolean));
						dtAccesosPermitidos.Columns.Add("Agregar", typeof(Boolean));
						dtAccesosPermitidos.Columns.Add("Modificar", typeof(Boolean));
						dtAccesosPermitidos.Columns.Add("Anular", typeof(Boolean));
						dtAccesosPermitidos.Columns.Add("Imprimir", typeof(Boolean));
						dtAccesosPermitidos.Columns.Add("Exportar", typeof(Boolean));

					}

					dtReportesPermitidos = new DataTable();

					if (dtReportesPermitidos.Columns.Count == 0)
					{
						//dtReportesPermitidos.Columns.Add("ID", typeof(Int32));
						dtReportesPermitidos.Columns.Add("ReportesSSRSID", typeof(Int32));
						dtReportesPermitidos.Columns.Add("ReporteNombre", typeof(String));
						dtReportesPermitidos.Columns.Add("ModuloID", typeof(Int32));
						dtReportesPermitidos.Columns.Add("ModuloNombre", typeof(String));
						dtReportesPermitidos.Columns.Add("EsSubReporte", typeof(Boolean));
						dtReportesPermitidos.Columns.Add("Agregar", typeof(Boolean));
						dtReportesPermitidos.Columns.Add("Modificar", typeof(Boolean));
						dtReportesPermitidos.Columns.Add("Anular", typeof(Boolean));
						dtReportesPermitidos.Columns.Add("Imprimir", typeof(Boolean));
						dtReportesPermitidos.Columns.Add("Exportar", typeof(Boolean));

					}

				}

				this.gridSES.DataSource = dtSES;
				this.gvDataSES.RefreshData();

				this.gridAccesosPermitidos.DataSource = dtAccesosPermitidos;
				this.gvDataAccesosPermitidos.RefreshData();
				
				this.gvData.RefreshData();
				
				this.gridReportesPermitidos.DataSource = dtReportesPermitidos;
				this.gvDataReportesPermitidos.RefreshData();
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}

		}
				
		private void FillAccesos()
		{
			try
			{
				if (Editable)
				{
					#region <<<ACCESO>>>
					var queryA = (from A in db.Accesos
								 join M in db.Modulos on A.ModuloID equals M.ID
								 where M.Activo
								 select new
								 {
									 IDModulo = M.ID,
									 NombreModulo = M.Nombre,
									 IDAcceso = A.ID,
									 NombreAcceso = A.Nombre,
									 SelectedAcceso = M.Activo,
									 A.PermiteMenu

								 }).Distinct();

						   dtAccesos = ToDataTable(queryA);
								 

					if (dtAccesosPermitidos.Rows.Count > 0)
					{

						foreach (DataRow drap in dtAccesosPermitidos.Rows)
						{
							foreach (DataRow dra in dtAccesos.Rows)
							{
								if (drap["AccesoID"].Equals(dra["IDAcceso"]))
								{ dtAccesos.Rows.Remove(dra); break; }
							}
						}
					}                      

					gridAccesos.DataSource = dtAccesos;

					for (int i = 0; i < gvDataAccesos.RowCount; i++)
					{
						this.gvDataAccesos.SetRowCellValue(i, "SelectedAcceso", false);
					}
					gvDataAccesos.RefreshData();
					#endregion

					#region <<<REPORTES>>>
					var queryR = (from A in db.ReportesSSRs
								 join M in db.Modulos on A.ModuloID equals M.ID
								 where M.Activo
								 select new
								 {
									 IDModulo = M.ID,
									 NombreModulo = M.Nombre,
									 IDReportesSSRS = A.ID,
									 NombreReporte = A.Nombre,
									 SelectedReporte = M.Activo,
									 A.EsSubReporte

								 }).Distinct();

					dtReportes = ToDataTable(queryR);

					if (dtReportesPermitidos.Rows.Count > 0)
					{

						foreach (DataRow drrp in dtReportesPermitidos.Rows)
						{
							foreach (DataRow drr in dtReportes.Rows)
							{
								if (drrp["ReportesSSRSID"].Equals(drr["IDReportesSSRS"]))
								{ dtReportes.Rows.Remove(drr); break; }
							}
						}
					}

					gridReportes.DataSource = dtReportes;

					for (int i = 0; i < gvDataReportes.RowCount; i++)
					{
						this.gvDataReportes.SetRowCellValue(i, "SelectedReporte", false);
					}
					gvDataReportes.RefreshData();
					#endregion

				}
				else
				{
					#region <<<ACCESOS>>>
					var queryA = (from A in db.Accesos
								 join M in db.Modulos on A.ModuloID equals M.ID
								 where M.Activo
								 select new
								 {
									 IDModulo = M.ID,
									 NombreModulo = M.Nombre,
									 IDAcceso = A.ID,
									 NombreAcceso = A.Nombre,
									 SelectedAcceso = M.Activo,
									 A.PermiteMenu

								 }).Distinct();

					dtAccesos = ToDataTable(queryA);
					gridAccesos.DataSource = dtAccesos;

					for (int i = 0; i < gvDataAccesos.RowCount; i++)
					{
						this.gvDataAccesos.SetRowCellValue(i, "SelectedAcceso", false);
					}
					gvDataAccesos.RefreshData();
					#endregion

					#region <<<REPORTES>>>
					var queryR = (from A in db.ReportesSSRs
								 join M in db.Modulos on A.ModuloID equals M.ID
								 where M.Activo
								 select new
								 {
									 IDModulo = M.ID,
									 NombreModulo = M.Nombre,
									 IDReportesSSRS = A.ID,
									 NombreReporte = A.Nombre,
									 SelectedReporte = M.Activo,
									 A.EsSubReporte

								 }).Distinct();

					dtReportes = ToDataTable(queryR);
					gridReportes.DataSource = dtReportes;

					for (int i = 0; i < gvDataReportes.RowCount; i++)
					{
						this.gvDataReportes.SetRowCellValue(i, "SelectedReporte", false);
					}
					gvDataReportes.RefreshData();
					#endregion
				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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

		private void ValidarerrRequiredField()
		{
			Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
			Parametros.General.ValidateEmptyStringRule(txtLogin, errRequiredField);
		}

		public bool ValidarCamposUsuario()
		{
			if (txtNombre.Text == "" || txtLogin.Text == "" || lkeEmpleado.EditValue == null)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
				return false;
			}


			if (!ValidarLogin(Convert.ToString(txtLogin.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
			{
				Parametros.General.DialogMsg("El nombre de usuario '" + Convert.ToString(txtLogin.Text) + "' ya esta registrado en el sistema, por favor seleccione otro nombre de usuario.", Parametros.MsgType.warning);
				return false;
			}
			
		   
			dtEstacionesServicios.DefaultView.RowFilter = "SelectedES = 'true'";            
			if (dtEstacionesServicios.DefaultView.Count <= 0)
			{
				dtEstacionesServicios.DefaultView.RowFilter = "";
				Parametros.General.DialogMsg("Debe seleccionar al menos una Estación de Servicio", Parametros.MsgType.warning);
				return false;
			}

			this.dtEstacionesServicios.DefaultView.RowFilter = "";
			this.dtSES.DefaultView.RowFilter = "";

			bool s = false;
			foreach(DataRow dres in this.dtEstacionesServicios.DefaultView.Table.Rows)	
			{
				foreach (DataRow drses in this.dtSES.Rows)
				{
					if (!s && drses["IDES"].Equals(dres["ID"]))
					{
						if ((from DataRow r in this.dtSES.Rows
						where Convert.ToInt32(r.ItemArray[0]).Equals(Convert.ToInt32(dres["ID"])) && Convert.ToBoolean(r.ItemArray[2]).Equals(true)
						select r).ToList<DataRow>().Count <= 0)
						{
							s = true;
							Parametros.General.DialogMsg("Debe seleccionar al menos una Subestación de Servicio para:" + Environment.NewLine + dres["Nombre"], Parametros.MsgType.warning);
							return false;
						}
					}
				}
			}

			//IEnumerator enumerator = this.dtEstacionesServicios.DefaultView.Table.Rows.GetEnumerator();
			
			//while (enumerator.MoveNext())
			//{
			//    DataRow dres = (DataRow)enumerator.Current;
			//    foreach (DataRow drses in this.dtSES.Rows)
			//    {
			//        if (!false && drses["IDES"].Equals(dres["ID"]))
			//        {
			//            if ((from DataRow r in this.dtSES.Rows
			//                    where Convert.ToInt32(r.ItemArray[0]).Equals(Convert.ToInt32(dres["ID"])) && Convert.ToBoolean(r.ItemArray[2]).Equals(true)
			//                    select r).ToList<DataRow>().Count <= 0)
			//            {
			//                Parametros.General.DialogMsg("Debe seleccionar al menos una Subestación de Servicio para:" + Environment.NewLine + dres["Nombre"], Parametros.MsgType.warning);
			//                return false;
			//            }
			//        }
			//    }
			//}
			
			bool Exist = false;
			if (Editable)
			{
				foreach (DataRow fila in dtEstacionesServicios.DefaultView.Table.Rows)
				{
					if (fila["ID"].Equals(Convert.ToInt32(lkESUse.EditValue)))
					{
						Exist = true;
						break;
					}
				}

				if (Exist.Equals(false))
				{
					Parametros.General.DialogMsg("La Estación de Servicio en uso no existe en las Estaciones de Servicios Asignadas para el usuario actual." + Environment.NewLine + "Debe de seleccionar otra Estación que corresponda a las Estaciones de Servicios asignadas.", Parametros.MsgType.warning);
					dtEstacionesServicios.DefaultView.RowFilter = "";
					return false;
				}
			}

			dtEstacionesServicios.DefaultView.RowFilter = "";
			dtSES.DefaultView.RowFilter = "";

			return true;
		}

		private bool ValidarLogin(string login, int? ID)
		{
			var result = (from i in db.Usuarios
						  where (ID.HasValue ? i.Login == login && i.ID != Convert.ToInt32(ID) : i.Login == login)
						  select i);

			if (result.Count() > 0)
			{
				return false;
			}
			return true;

		}

		private void RefreshAddSES(int rowHandle)
		{
			try
			{
				db.SubEstacions.Where(s => s.EstacionServicioID.Equals(Convert.ToInt32(gvData.GetRowCellValue(rowHandle, "ID")))).ToList().ForEach(ses =>
				{
					dtSES.Rows.Add(ses.EstacionServicioID, ses.ID, false, ses.Nombre);
				});

				if (listadoES != null)
				{
					if (listadoES.Count(l => l.ID.Equals(Convert.ToInt32(gvData.GetRowCellValue(rowHandle, "ID")))) <= 0)
						listadoES.Add(new Parametros.ListIdDisplay(Convert.ToInt32(gvData.GetRowCellValue(rowHandle, "ID")), Convert.ToString(gvData.GetRowCellValue(rowHandle, "Nombre"))));
				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(SAGAS.Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}

		private void RefreshDeleteSES(int rowHandle)
		{
			try
			{
				dtSES.Rows.Cast<DataRow>().Where(r => Convert.ToInt32(r.ItemArray[0]).Equals(Convert.ToInt32(gvData.GetRowCellValue(rowHandle, "ID")))).ToList().ForEach(r => r.Delete());
				dtSES.AcceptChanges();

				if (listadoES != null)
				{
					if (listadoES.Count(l => l.ID.Equals(Convert.ToInt32(gvData.GetRowCellValue(rowHandle, "ID")))) > 0)
						listadoES.RemoveAll(r => r.ID.Equals(Convert.ToInt32(gvData.GetRowCellValue(rowHandle, "ID"))));

					if (lkESUse.EditValue != null)
					{
						if (Convert.ToInt32(lkESUse.EditValue).Equals(Convert.ToInt32(gvData.GetRowCellValue(rowHandle, "ID"))))
							lkESUse.EditValue = null;
					}
				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(SAGAS.Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}

		private bool GuardarAll()
		{
			 if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

			using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
			{
			  db.Transaction = trans;
			  db.CommandTimeout = 300; 
				try
				{
					Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
					Entidad.Usuario U;

					if (Editable && !Clonar)
					{
						U = db.Usuarios.Single(u => u.ID == EntidadAnterior.ID);

						var AccesosDelete = from asis in db.AccesoSistemas
											join a in db.Accesos on asis.AccesoID equals a.ID
											where asis.UsuarioID == U.ID
											select asis;

                        var EstacionesDelete = from asis in db.AccesoSistemas
                                               where asis.EstacionServicioID > 0 && asis.UsuarioID == U.ID
                                               select asis;

						var ReportesDelete = from ar in db.AccesosReportes
											join r in db.ReportesSSRs on ar.ReportesSSRSID equals r.ID
											where ar.UsuarioID == U.ID
											select ar;

                        db.AccesoSistemas.DeleteAllOnSubmit(AccesosDelete);
                        db.AccesoSistemas.DeleteAllOnSubmit(EstacionesDelete);
                        db.AccesosReportes.DeleteAllOnSubmit(ReportesDelete);
                        db.UsuarioSubEstacions.DeleteAllOnSubmit(db.UsuarioSubEstacions.Where(s => s.UsuarioID.Equals(U.ID)));
                        db.SubmitChanges();
					}
					else
					{
						U = new Entidad.Usuario();
						U.Contrasena = Parametros.Security.Encrypt("inicio", Parametros.Config.MagicWord);
						U.IsReset = true;
						U.Activo = true;
						
						db.SubmitChanges();

					}

					U.Nombre = Nombre;
					U.Login = Login;                     
					U.EmpresaID = Parametros.General.EmpresaID;
					U.EmpleadoID = lkeEmpleado.EditValue == null ? 0 : IDEmpleado;
					U.EstacionServicioID = lkESUse.EditValue == null ? 0 : Convert.ToInt32(lkESUse.EditValue);
                    U.SubEstacionID = 0;

                    if (Editable && !Clonar)
					{
						DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(U, 1));
						DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

						Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
						 "Se modificó el Usuario: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

					}
					else
					{
						db.Usuarios.InsertOnSubmit(U);
						Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
						"Se creó el Usuario: " + U.Nombre, this.Name);

					}

					dtAccesosPermitidos.DefaultView.RowFilter = "";
					dtReportesPermitidos.DefaultView.RowFilter = "";

					//foreach (DataRow currentDataRow in allRows)
					//{
					//    newDataTable.ImportRow(currentDataRow);
					//}

					DataRow[] allRows = dtEstacionesServicios.Select();
					DataRow[] allRowsAccesos = dtAccesosPermitidos.Select();
					DataRow[] allRowsReportes = dtReportesPermitidos.Select();

                    Entidad.AccesoSistema AS = new Entidad.AccesoSistema();

                    //ASIGNACION DE ESTACIONES
                    foreach (DataRow drES in dtEstacionesServicios.Rows)
                    {
                        //MessageBox.Show(drES["SelectedES"].ToString());
                        if (drES["SelectedES"].Equals(true))
                        {
                            AS = new Entidad.AccesoSistema();
                            AS.EstacionServicioID = Convert.ToInt32(drES["ID"]);
                            AS.UsuarioID = U.ID;
                            db.AccesoSistemas.InsertOnSubmit(AS);
                        }
                    }


							//foreach (DataRow dr in dtAccesosPermitidos.Rows)
                    foreach (DataRow dr in dtAccesosPermitidos.Rows)
							{
								int acceso = Convert.ToInt32(dr["AccesoID"]);
								bool agregar = Convert.ToBoolean(dr["Agregar"]), modificar = Convert.ToBoolean(dr["Modificar"])
									, anular = Convert.ToBoolean(dr["Anular"]), imprimir = Convert.ToBoolean(dr["Imprimir"])
									, exportar = Convert.ToBoolean(dr["Exportar"]);

								AS = new Entidad.AccesoSistema();

								AS.EstacionServicioID = 0; 
								AS.AccesoID = acceso;
								AS.UsuarioID = U.ID;
								AS.Agregar = agregar;
								AS.Modificar = modificar;
								AS.Anular = anular;
								AS.Imprimir = imprimir;
								AS.Exportar = exportar;

								db.AccesoSistemas.InsertOnSubmit(AS);


							}

							foreach (DataRow dr in allRowsReportes)
							{
								int reporte = Convert.ToInt32(dr["ReportesSSRSID"]);
								bool agregar = Convert.ToBoolean(dr["Agregar"]), modificar = Convert.ToBoolean(dr["Modificar"])
									, anular = Convert.ToBoolean(dr["Anular"]), imprimir = Convert.ToBoolean(dr["Imprimir"])
									, exportar = Convert.ToBoolean(dr["Exportar"]);

								Entidad.AccesosReporte AR = new Entidad.AccesosReporte();

								AR.EstacionServicioID = 0;
								AR.ReportesSSRSID = reporte;
								AR.UsuarioID = U.ID;
								AR.Agregar = agregar;
								AR.Modificar = modificar;
								AR.Anular = anular;
								AR.Imprimir = imprimir;
								AR.Exportar = exportar;

								db.AccesosReportes.InsertOnSubmit(AR);


							}
						
					

					dtSES.Rows.Cast<DataRow>().Where(r => Convert.ToBoolean(r.ItemArray[2]).Equals(true)).ToList().ForEach(r => { 
						Entidad.UsuarioSubEstacion US = new Entidad.UsuarioSubEstacion();
						US.UsuarioID = U.ID;
						US.SubEstacionID = Convert.ToInt32(r["IDSES"]);
						db.UsuarioSubEstacions.InsertOnSubmit(US);
						db.SubmitChanges();
					});
					

			db.SubmitChanges();
			trans.Commit();

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
					Parametros.General.splashScreenManagerMain.CloseWaitForm();
					if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
				}
	 

			}

		}

		#endregion

		#region Eventos 
		
		private void wizControl_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
		{
			try
			{
				if (e.Page == wpUser)
				{
					if (!ValidarCamposUsuario()) 
					{
						e.Handled = true;
						return;
					}
					else
					{
						if(!loaded)
                        {
                            FillAccesos();
                            loaded = true;
                        }
					} 

				}

				else if (e.Page == wpAccesosGenerales)
				{
					if (dtAccesosPermitidos.Rows.Count <= 0)
					{
						Parametros.General.DialogMsg("Debe asignar al menos un acceso al usuario", Parametros.MsgType.warning);
						e.Handled = true;
						return;
					}

					dtAccesos.DefaultView.RowFilter = "SelectedAcceso = 'True'";
					if (dtAccesos.DefaultView.Count > 0)
					{
					  Parametros.General.DialogMsg("Existen accesos seleccionados para ser otorgados al usuario.", Parametros.MsgType.warning);
					  dtAccesos.DefaultView.RowFilter = "";
					  gvDataAccesos.RefreshData();
					  e.Handled = true;
					  return;
					}

					dtAccesos.DefaultView.RowFilter = "";
					gvDataAccesos.RefreshData();
									  
				}
				else if (e.Page == wpAccesoReportes)
				{
					//if (dtReportesPermitidos.Rows.Count <= 0)
					//{
					//    Parametros.General.DialogMsg("Debe asignar al menos un reporte al usuario", Parametros.MsgType.warning);
					//    e.Handled = true;
					//    return;
					//}

					dtReportes.DefaultView.RowFilter = "SelectedReporte = 'True'";
					if (dtReportes.DefaultView.Count > 0)
					{
						Parametros.General.DialogMsg("Existen reportes seleccionados para ser otorgados al usuario.", Parametros.MsgType.warning);
						dtReportes.DefaultView.RowFilter = "";
						gvDataReportes.RefreshData();
						e.Handled = true;
						return;
					}

					dtReportes.DefaultView.RowFilter = "";
					gvDataReportes.RefreshData();
				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}
		
		private void wizControl_FinishClick(object sender, CancelEventArgs e)
		{
			if (!GuardarAll()) e.Cancel = true;

			this.Close();
		}
	   
		private void wizControl_CancelClick(object sender, CancelEventArgs e)
		{
			this.Close();
		}

		private void wizAddUsuario_FormClosed(object sender, FormClosedEventArgs e)
		{
			MDI.CleanDialog(ShowMsg);
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			try
			{
				this.gvDataAccesos.ActiveFilter.Clear();
				for (int i = 0; i < this.gvDataAccesos.RowCount; i++)
				{
					this.gvDataAccesos.SetRowCellValue(i, "SelectedAcceso", true);
				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}

		private void btnUnselectAll_Click(object sender, EventArgs e)
		{
			try
			{
				gvDataAccesos.ActiveFilter.Clear();
				for (int i = 0; i < gvDataAccesos.RowCount; i++)
				{
					gvDataAccesos.SetRowCellValue(i, "SelectedAcceso", false);
				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}

		private void btnSelectAllRep_Click(object sender, EventArgs e)
		{
			gvDataReportes.ActiveFilter.Clear();
			for (int i = 0; i < gvDataReportes.RowCount; i++)
			{
				gvDataReportes.SetRowCellValue(i, "SelectedReporte", true);
			}
		}

		private void btnUnselectAllRep_Click(object sender, EventArgs e)
		{
			gvDataReportes.ActiveFilter.Clear();
			for (int i = 0; i < gvDataReportes.RowCount; i++)
			{
				gvDataReportes.SetRowCellValue(i, "SelectedReporte", false);
			}
		}

		private void btnPermitirReporte_Click(object sender, EventArgs e)
		{
			try
			{
				gvDataReportes.ActiveFilter.Clear();

				dtReportes.DefaultView.RowFilter = "SelectedReporte = 'true'";
				if (dtEstacionesServicios.DefaultView.Count <= 0)
				{
					Parametros.General.DialogMsg("Debe seleccionar al menos un reporte para ser otorgado al usuario", Parametros.MsgType.warning);
					dtReportes.DefaultView.RowFilter = "";
					return;
				}
				dtReportes.DefaultView.RowFilter = "";


				int x = Convert.ToInt32(gvDataReportes.RowCount);

				for (int i = 0; i < x; i++)
				{

					foreach (DataRow drr in dtReportes.Rows)
					{
						if (drr["SelectedReporte"].Equals(true))
						{
							DataRow drrp = dtReportesPermitidos.NewRow();
							drrp["ReportesSSRSID"] = drr["IDReportesSSRS"];
							drrp["ReporteNombre"] = drr["NombreReporte"];
							drrp["ModuloID"] = drr["IDModulo"];
							drrp["ModuloNombre"] = drr["NombreModulo"];
							drrp["EsSubReporte"] = drr["EsSubReporte"];
							drrp["Agregar"] = false;
							drrp["Modificar"] = false;
							drrp["Anular"] = false;
							drrp["Imprimir"] = false;
							drrp["Exportar"] = false;

							dtReportesPermitidos.Rows.Add(drrp);
							dtReportes.Rows.Remove(drr);
							break;
						}
					}

				}

			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());

			}
		}

		private void btnSelectAllES_Click(object sender, EventArgs e)
		{
			try
			{
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTACTUALIZANDO);
				gvData.ActiveFilter.Clear();
				for (int i = 0; i < gvData.RowCount; i++)
				{
					gvData.SetRowCellValue(i, "SelectedES", true);
					this.RefreshAddSES(i);
				}
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
			finally
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();
			}
		}

		private void btnUnselectAllES_Click(object sender, EventArgs e)
		{
			try
			{
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTACTUALIZANDO);
				gvData.ActiveFilter.Clear();
				for (int i = 0; i < gvData.RowCount; i++)
				{
					gvData.SetRowCellValue(i, "SelectedES", false);
					this.RefreshDeleteSES(i);
				}
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
			finally
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();
			}
		}

		private void btnPermitirAcceso_Click(object sender, EventArgs e)
		{
			try
			{
				gvDataAccesos.ActiveFilter.Clear();

				dtAccesos.DefaultView.RowFilter = "SelectedAcceso = 'true'";
				if (dtEstacionesServicios.DefaultView.Count <= 0)
				{
					Parametros.General.DialogMsg("Debe seleccionar al menos un acceso para ser otorgado al usuario", Parametros.MsgType.warning);
					dtAccesos.DefaultView.RowFilter = "";
					return;
				}
				dtAccesos.DefaultView.RowFilter = "";


				int x = Convert.ToInt32(gvDataAccesos.RowCount);

				for (int i = 0; i < x; i++)
				{

				foreach (DataRow dra in dtAccesos.Rows)
					{
						if (dra["SelectedAcceso"].Equals(true))
						{
							DataRow drap = dtAccesosPermitidos.NewRow();
							drap["AccesoID"] = dra["IDAcceso"];
							drap["AccesoNombre"] = dra["NombreAcceso"];
							drap["ModuloID"] = dra["IDModulo"];
							drap["ModuloNombre"] = dra["NombreModulo"];
							drap["PermiteMenu"] = dra["PermiteMenu"];
							drap["Agregar"] = false;
							drap["Modificar"] = false;
							drap["Anular"] = false;
							drap["Imprimir"] = false;
							drap["Exportar"] = false;

							dtAccesosPermitidos.Rows.Add(drap);
							dtAccesos.Rows.Remove(dra);
							break;
						}
					}

				}
					
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());

			}
		}

		private void btrRemoveAcces_Click(object sender, EventArgs e)
		{
			try
			{

				if (gvDataAccesosPermitidos.FocusedRowHandle >= 0)
				{
					DataRow drAccesoPermitido = dtAccesosPermitidos.Select("AccesoID = " + gvDataAccesosPermitidos.GetFocusedRowCellValue(colIDAccesoPermitido).ToString()).FirstOrDefault();
										  
					
					DataRow drAcceso = dtAccesos.NewRow();

					drAcceso["IDModulo"] = drAccesoPermitido["ModuloID"];
					drAcceso["NombreModulo"] = drAccesoPermitido["ModuloNombre"];
					drAcceso["IDAcceso"] = drAccesoPermitido["AccesoID"];
					drAcceso["NombreAcceso"] = drAccesoPermitido["AccesoNombre"];
					drAcceso["SelectedAcceso"] = false;
					drAcceso["PermiteMenu"] = drAccesoPermitido["PermiteMenu"];
					dtAccesos.Rows.Add(drAcceso);

					dtAccesosPermitidos.Rows.Remove(drAccesoPermitido);

				}

			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}

		private void btrRemoveReport_Click(object sender, EventArgs e)
		{
			try
			{
				if (gvDataAccesosPermitidos.FocusedRowHandle >= 0)
				{
					DataRow drReportePermitido = dtReportesPermitidos.Select("ReportesSSRSID = " + gvDataReportesPermitidos.GetFocusedRowCellValue(colIDReportesSSRS).ToString()).FirstOrDefault();


					DataRow drReporte = dtReportes.NewRow();

					drReporte["IDModulo"] = drReportePermitido["ModuloID"];
					drReporte["NombreModulo"] = drReportePermitido["ModuloNombre"];
					drReporte["IDReportesSSRS"] = drReportePermitido["ReportesSSRSID"];
					drReporte["NombreReporte"] = drReportePermitido["ReporteNombre"];
					drReporte["SelectedReporte"] = false;
					drReporte["EsSubReporte"] = drReportePermitido["EsSubReporte"];
					dtReportes.Rows.Add(drReporte);

					dtReportesPermitidos.Rows.Remove(drReportePermitido);

				}

			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}

		private void btnUnselectAllMenu_Click(object sender, EventArgs e)
		{

			foreach (DataRowView dr in dtAccesosPermitidos.DefaultView)
			{
				dr["Agregar"] = false;
				dr["Modificar"] = false;
				dr["Anular"] = false;
				dr["Imprimir"] = false;
				dr["Exportar"] = false;
			}
		}

		private void btnSelectAllMenu_Click(object sender, EventArgs e)
		{
			foreach (DataRowView dr in dtAccesosPermitidos.DefaultView)
			{
				dr["Agregar"] = true;
				dr["Modificar"] = true;
				dr["Anular"] = true;
				dr["Imprimir"] = true;
				dr["Exportar"] = true;

			}
		}

		private void wizControl_SelectedPageChanging(object sender, DevExpress.XtraWizard.WizardPageChangingEventArgs e)
		{
			try
			{
				if (e.Direction == DevExpress.XtraWizard.Direction.Forward)
				{
					if (e.PrevPage == wpAccesosGenerales)
					{
						//Mostros solo los accesos que permiten menu
						dtAccesosPermitidos.DefaultView.RowFilter = "PermiteMenu = 'True'";
						gridMenu.DataSource = dtAccesosPermitidos.DefaultView;
					}
					//if (e.PrevPage == wpAccesoReportes)
					//{
					//    //Mostros solo los reportes que no son SubReporte
					//    dtReportesPermitidos.DefaultView.RowFilter = "EsSubReporte = 'False'";
					//    gridMenu.DataSource = dtReportesPermitidos.DefaultView;

					//}
				}
				else
				{
					if (e.PrevPage == wpAccesoReportes)
					{
						dtAccesosPermitidos.DefaultView.RowFilter = "";
						gvDataMenu.RefreshData();
					}
					else if (e.PrevPage == wpFinish)
					{
						dtReportesPermitidos.DefaultView.RowFilter = "";
						gvDataMenu.RefreshData();
					}
				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}

		}
		
		private void txtNombre_Validated(object sender, EventArgs e)
		{
			Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.BaseEdit)sender, errRequiredField);
		}

		private void lkeEmpleado_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (lkeEmpleado.EditValue != null && Convert.ToInt32(lkeEmpleado.EditValue) > 0)
				{
					Entidad.Empleado E = db.Empleados.Single(em => em.ID == Convert.ToInt32(lkeEmpleado.EditValue));

					string texto;

					string nombre;
					nombre = E.Nombres.Substring(0, 1).ToString();

					string[] apellido = E.Apellidos.Split(' ');

					texto = nombre + apellido[0];

					if (db.Usuarios.Where(u => u.Login == texto).Count() <= 0)
					{
						txtLogin.Text = texto;
					}
					else
					{
						string[] nombre2 = E.Nombres.Split(' ');

						texto = (nombre2.Count() > 1 ? 
							(!String.IsNullOrEmpty(nombre2[1]) ? nombre2[1].Substring(0, 1) : "")
							: "") + apellido[0];

						if (db.Usuarios.Where(u => u.Login == texto).Count() <= 0)
						{
							txtLogin.Text = texto;
						}
						else
						{
							texto = nombre2[0].Substring(0, 1) + (nombre2.Count() > 1 ?
								(!String.IsNullOrEmpty(nombre2[1]) ? nombre2[1].Substring(0, 1) : "")
								: "") + apellido[0];
							txtLogin.Text = texto;
						}
					}

					txtNombre.Text = E.Nombres + " " + E.Apellidos;

					ValidarerrRequiredField();

				}
			}
			catch (Exception ex)
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}
		}
		 
		private void chkBtnAgregar_CheckedChanged(object sender, EventArgs e)
		{
			var btn = (DevExpress.XtraEditors.CheckButton)sender;
			string info = btn.Name.Substring(6);

			if (btn.Checked == true)
			{
				btn.Image = Properties.Resources.check18;              
				
				foreach (DataRowView dr in dtAccesosPermitidos.DefaultView)
				{
					dr[info] = true;

				}

			}
			else if (btn.Checked == false)
			{
				btn.Image = Properties.Resources.Uncheck18;

				foreach (DataRowView dr in dtAccesosPermitidos.DefaultView)
				{
					dr[info] = false;

				}
			}

		}

		private void gvData_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
		{
			try
			{
				if (e.Column.Equals(gvData.Columns["SelectedES"]))
				{
					if (Convert.ToBoolean(e.Value).Equals(true))
					{
						this.RefreshAddSES(e.RowHandle);
					}
					else if (Convert.ToBoolean(e.Value).Equals(false))
					{
						this.RefreshDeleteSES(e.RowHandle);
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
