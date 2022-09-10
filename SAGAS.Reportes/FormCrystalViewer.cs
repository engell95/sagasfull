using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SAGAS.Reportes
{
    public partial class FormCrystalViewer : Form
    {
        public FormCrystalViewer()
        {
            InitializeComponent();
        }

        private void FormCrystalViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(this.Tag.ToString());
        }

    }
}
