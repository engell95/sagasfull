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
    public partial class MyBand : DevExpress.XtraGrid.Views.BandedGrid.GridBand
    {   

        /// <summary>
        /// Constructor Base
        /// </summary>
        public MyBand():base()
        {
            this.Visible = true;
        }

        /// <summary>
        /// Creador de Bandas Generales
        /// </summary>
        /// <param name="Nombre">El nombre de la banda</param>
        /// <param name="Titulo">El titulo de la banda</param>
        /// <param name="Ancho">Ancho de la banda</param>
        /// <param name="Index">posición de la banda</param>
        public MyBand(string nombre,string Titulo, int Ancho, int Index)
            : base()
        {
            this.Name = nombre;
            this.Caption = Titulo;
            this.MinWidth = 20;
            this.OptionsBand.AllowMove = false;
            this.OptionsBand.AllowPress = false;
            this.OptionsBand.ShowCaption = true;
            this.AppearanceHeader.Options.UseTextOptions = true;
            this.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Width = Ancho;
        }

    }
}
