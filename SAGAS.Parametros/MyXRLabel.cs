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
    public partial class MyXRLabel :DevExpress.XtraReports.UI.XRLabel
    {   

        /// <summary>
        /// Constructor Base
        /// </summary>
        public MyXRLabel():base()
        {
            this.Visible = true;
        }

       

        /// <summary>
        /// Etiqueta Tablas Reportes
        /// </summary>
        /// <param name="Campo">Campo o dato a mostrar</param>
        /// <param name="Ancho">Ancho de la etiqueta</param>
        /// <param name="Alto">Alto de la etiqueta</param>
        /// <param name="Alinear">Posición del Texto</param>
        /// <param name="FondoColor">Color del fondo de la celda</param>
        /// <param name="FuenteColor">Color de las letras de la celda</param>
        public MyXRLabel(string Campo, float Ancho, float Alto, DevExpress.XtraPrinting.TextAlignment Alinear, Color FondoColor, Color FuenteColor)
            : base()
        {
            //**Celda
            this.WidthF = Ancho;
            this.HeightF = Alto;
            this.Visible = true;
            this.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.StylePriority.UsePadding = false;
            this.TextAlignment = Alinear;
            this.Text = Campo;
            this.BackColor = FondoColor;
            this.ForeColor = FuenteColor;
            this.Font = new System.Drawing.Font("Tahoma", 8F); 
            this.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
          
        }

    }
}
