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
    public partial class MyBarButtonItem :DevExpress.XtraBars.BarButtonItem
    {   

        /// <summary>
        /// Constructor Base
        /// </summary>
        public MyBarButtonItem():base()
        {
            this.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }

       

        /// <summary>
        /// BarButtonItem para reportes
        /// </summary>
        /// <param name="texto">Texto del Componente</param> 
        public MyBarButtonItem(string texto)
            : base()
        {
            //**Boton
            this.Caption = texto;
            this.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reportes));
            this.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
        }

    }
}
