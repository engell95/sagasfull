using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;

namespace SAGAS.Parametros.Forms
{
    public partial class FormLicencia : DevExpress.XtraEditors.XtraForm
    {
        public FormLicencia()
        {
            InitializeComponent();
        }

        private void FormLicencia_Load(object sender, EventArgs e)
        {
            Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            //**EstacionServicio
            lkeEstacionServicio.DataSource = from es in db.EstacionServicios
                                                        where es.Activo
                                                        select new { es.ID, es.Nombre };
            lkeEstacionServicio.DisplayMember = "Nombre";
            lkeEstacionServicio.ValueMember = "ID";

            var obj = from l in db.Licencias
                      select new
                      {
                          l.ID,
                          NamePC = Security.Decrypt(l.NamePC),
                          BaseBoard = Security.Decrypt(l.BaseBoard),
                          SerialDisk = Security.Decrypt(l.SerialDisk),
                          CPUId = Security.Decrypt(l.CPUId),
                          LogicalDiskSerial = Security.Decrypt(l.LogicalDiskSerial),
                          EstacionServicio = Security.Decrypt(l.EstacionServicio),
                          TipoConexionStr = Security.Decrypt(l.TipoConexion),
                          SerialRegistro = Security.Decrypt(l.SerialRegistro),
                          SerialBD = Security.Decrypt(l.SerialBD)

                      }
                      ;
                       
            this.gridLicencia.DataSource = obj;
        }
    }
}