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
    public partial class MyBandColumn :DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn
    {
       
        /// <summary>
        /// Constructor Base
        /// </summary>
        public MyBandColumn():base()
        {
            this.Visible = true;
        }

        /// <summary>
        /// Columnas Generales
        /// </summary>
        /// <param name="Nombre">ID del Objeto</param>
        /// <param name="Index">Posición de la columna</param>
        /// <param name="Ancho">Ancho de la columna</param>
        /// <param name="MostrarTitulo">Mostrar el titulo de la columna</param>
        public MyBandColumn(string Nombre, int Index, int Ancho, bool MostrarTitulo)
            : base()
        {
            //**Columna
            this.Width = Ancho;
            this.Caption = Nombre;
            this.FieldName = Nombre; //prod.ID.ToString() + lado.Key;
            this.Name = Nombre; //"col" + prod.ID.ToString() + lado.Key;
            this.Visible = true;
            this.OptionsColumn.ShowCaption = MostrarTitulo;
            this.OptionsColumn.FixedWidth = true;
            this.AppearanceHeader.Options.UseForeColor = true;
            this.AppearanceHeader.Options.UseBackColor = true;
            this.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.AppearanceHeader.ForeColor = Color.Black;
            this.DisplayFormat.FormatString = "N3";
            this.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

        }

        /// <summary>
        /// Columnas Alamacen
        /// </summary>
        /// <param name="Nombre">ID del Objeto</param>
        /// <param name="Index">Posición de la columna</param>
        /// <param name="Ancho">Ancho de la columna</param>
        /// <param name="MostrarTitulo">Mostrar el titulo de la columna</param>
        public MyBandColumn(string Nombre, string Campo, Entidad.Almacen alma)
            : base()
        {
            //**Columna
            this.Tag = alma;
            this.Caption = Nombre;
            this.FieldName = Campo; //prod.ID.ToString() + lado.Key;
            this.Name = Campo; //"col" + prod.ID.ToString() + lado.Key;
            this.Visible = true;
            this.OptionsColumn.ShowCaption = true;
            this.OptionsColumn.FixedWidth = true;
            this.AppearanceHeader.Options.UseForeColor = true;
            this.AppearanceHeader.Options.UseBackColor = true;
            this.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.AppearanceHeader.ForeColor = Color.Black;
            this.DisplayFormat.FormatString = "N3";
            this.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

        }

        /// <summary>
        /// Columnas Mangueras
        /// </summary>
        /// <param name="Nombre">ID del Objeto</param> 
        /// <param name="Campo">FieldName para la columna</param>
        /// <param name="Index">Posición de la columna</param>
        /// <param name="Ancho">Ancho de la columna</param>
        /// <param name="MostrarTitulo">Mostrar el titulo de la columna</param>
        /// <param name="col">Color del titulo de la columna</param>
        public MyBandColumn(string Nombre, string Campo, int Index, int Ancho, bool MostrarTitulo, Color col)
            : base()
        {
            //**Columna
            this.Width = Ancho;
            this.Caption = Nombre;
            this.FieldName = Campo;
            this.Name = "col" + Campo;
            this.Visible = true;
            this.OptionsColumn.ShowCaption = MostrarTitulo;
            this.OptionsColumn.FixedWidth = true;
            this.AppearanceHeader.Options.UseForeColor = true;
            this.AppearanceHeader.Options.UseBackColor = true;
            this.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.AppearanceHeader.ForeColor = Color.White;
            this.DisplayFormat.FormatString = "N3";
            this.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.AppearanceHeader.BackColor = col;

        }

    }
}
