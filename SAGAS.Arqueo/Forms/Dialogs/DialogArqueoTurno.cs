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
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;
using DevExpress.XtraGrid.Views.Grid;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogArqueoTurno : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        Entidad.SAGASDataViewsDataContext dbView;
       
        private Entidad.Turno EtTurno;
        private Entidad.ResumenDia EtResumenDia;
        private int IDES = Parametros.General.EstacionServicioID;

        #endregion

        #region *** INICIO ***

        public DialogArqueoTurno()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                int IDSUS = 0;

                if (Parametros.General.ListSES.Count > 0)
                {
                    if (Parametros.General.ListSES.Count.Equals(1))
                        IDSUS = (Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault()));
                    else if (Parametros.General.ListSES.Count > 1)
                    {
                        using (Forms.Dialogs.DialogGetFecha df = new Forms.Dialogs.DialogGetFecha(true))
                        {
                            df.layoutControlItemCaption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                            if (df.ShowDialog().Equals(DialogResult.OK))
                            {
                                IDSUS = df.IDSubEstacion;
                            }
                            else
                                this.Close();
                        }
                    }
                }

                if (db.ResumenDias.Count(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSUS)) > 0)
                {
                    EtResumenDia = db.ResumenDias.Where(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSUS)).First();

                    if (db.Turnos.Count(t => !t.Cerrada && t.ResumenDiaID.Equals(EtResumenDia.ID) && !t.Especial) > 0)
                    {
                        EtTurno = db.Turnos.Where(t => !t.Cerrada && t.ResumenDiaID.Equals(EtResumenDia.ID) && !t.Especial).OrderByDescending(o => o.ID).First();

                        if (db.ArqueoIslas.Count(ae => ae.TurnoID.Equals(EtTurno.ID)) > 0)
                        {

                            Reportes.Util.PrintArqueoFast(this, dbView, db, 0, EtTurno.ID, EtResumenDia, Parametros.TiposImpresion.Vista_Previa, false, Parametros.TiposArqueo.Turno, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);

                        }
                        else
                        {
                            Parametros.General.DialogMsg("No existe ningún Arqeuo de Isla creado.", Parametros.MsgType.warning);
                            this.Close();
                        }

                    }
                    else
                    {
                        Parametros.General.DialogMsg("No Existe un Turno Abierto.", Parametros.MsgType.warning);
                        this.Close();
                    }
                }
                else
                {
                    Parametros.General.DialogMsg("No Existe un Resumen de Día Abierto.", Parametros.MsgType.warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** METODOS ***
       

      
        #endregion

        #region *** EVENTOS ***
         
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         
        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                if (db.GetArqueosIslasPendientes(IDES, EtTurno.ID, EtResumenDia.SubEstacionID) <= 0)
                {
                    if (db.ArqueoEfectivos.Count(ae => ae.TurnoID.Equals(EtTurno.ID) && ae.Cerrado) > 0)
                    {
                        Reportes.Util.PrintArqueoFast(this, dbView, db, 0, EtTurno.ID, EtResumenDia, (((Parametros.Estados)(EtTurno.Estado)).Equals(Parametros.Estados.Abierto) ? Parametros.TiposImpresion.Original : Parametros.TiposImpresion.Modificado), true, Parametros.TiposArqueo.Turno, Parametros.Properties.Resources.TXTCIERREREGISTRO, this.printControlAreaReport);

                        Entidad.Turno T = db.Turnos.Single(t => t.ID.Equals(EtTurno.ID));
                        T.Cerrada = true;
                        T.Estado = 2;
                        T.FechaFinal = db.GetDateServer();
                        T.UsuarioCerradoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        if (EtResumenDia.SubEstacionID > 0)
                        {
                            if (EtTurno.Numero >= Convert.ToInt32(db.SubEstacions.Single(es => es.ID.Equals(EtResumenDia.SubEstacionID)).NumeroTurnos))
                            {
                                Entidad.ResumenDia RD = db.ResumenDias.Single(r => r.ID.Equals(EtResumenDia.ID));
                                RD.Cerrado = true;
                                db.SubmitChanges();
                            }
                        }
                        else
                        {
                            if (EtTurno.Numero >= Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(IDES)).NumeroTurnos))
                            {
                                Entidad.ResumenDia RD = db.ResumenDias.Single(r => r.ID.Equals(EtResumenDia.ID));
                                RD.Cerrado = true;
                                db.SubmitChanges();
                            }
                        }

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Finalizó el Turno: " + T.Numero.ToString() + " del " + EtResumenDia.FechaInicial.Date.ToString(), this.Name);

                        this.Close();
                    }
                    else
                    {
                        Parametros.General.DialogMsg("Debe de cerrar el arqueo de efectivo de este turno!", Parametros.MsgType.warning);
                        return;
                    }
                }
                else
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGARQUEOSISLASABIERTOS, Parametros.MsgType.warning);
                    return;
                } 
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
        
        #endregion
    }


}