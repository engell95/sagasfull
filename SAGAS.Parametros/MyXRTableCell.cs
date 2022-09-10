using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SAGAS.Parametros
{
    public partial class MyXRTableCell :DevExpress.XtraReports.UI.XRTableCell
    {   

        /// <summary>
        /// Constructor Base
        /// </summary>
        public MyXRTableCell():base()
        {
            this.Visible = true;
        }

       

        /// <summary>
        /// Celdas Tablas Reportes
        /// </summary>
        /// <param name="Nombre">Nombre del Componente</param> 
        /// <param name="Campo">Campo o dato a mostrar</param>
        /// <param name="Ancho">Ancho de la celda</param>
        /// <param name="FondoColor">Color del fondo de la celda</param>
        /// <param name="FuenteColor">Color de las letras de la celda</param>
        public MyXRTableCell( string Campo, float Ancho, DevExpress.XtraPrinting.TextAlignment Alinear, Color FondoColor, Color FuenteColor)
            : base()
        {
            //**Celda
            this.WidthF = Ancho;
            this.Visible = true;
            this.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.StylePriority.UsePadding = false;
            this.TextAlignment = Alinear;
            this.Text = Campo;
            this.BackColor = FondoColor;
            this.ForeColor = FuenteColor;

        }

    }
}
