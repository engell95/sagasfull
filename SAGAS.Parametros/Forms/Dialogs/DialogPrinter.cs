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
using System.Drawing.Printing;

namespace SAGAS.Parametros.Forms.Dialogs
{
    public partial class DialogPrinter : Form
    {
        #region >>> INICIO <<<
        public string LocalPrinter;                                        

        public DialogPrinter()
        {
            InitializeComponent();  
        }

        #endregion     
        
        #region >>> METODOS <<<

        #endregion

        #region >>> EVENTOS <<<

        private void DialogConexion_Load(object sender, EventArgs e)
        {
            
            try
            {
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    LocalPrinter = PrinterSettings.InstalledPrinters[i];
                    cboPrinter.Properties.Items.Add(LocalPrinter);
                }

                cboPrinter.Text = Parametros.Config.SelectedPrinterLocal;  
                 
            }
            catch (Exception ex)
            { MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR",  MessageBoxButtons.OK,  MessageBoxIcon.Error); }
        
        }     
 
        private void btnTest_Click(object sender, EventArgs e)
        {
          try
            {
              Parametros.Config.SetValueByKeyAppSettings(Parametros.Config.strPrinterLocal, cboPrinter.Text.ToString());
                
              PrintDocument doc = new PrintDocument();
              doc.PrinterSettings.PrinterName = Parametros.Config.SelectedPrinterLocal;
              doc.PrintPage += new PrintPageEventHandler(this.doc_PrintPage);
              doc.Print();
             
            }
            catch (Exception ex)
          {
              MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("SAGAS Prueba Correcta de Impresión.....", this.Font, Brushes.Black, new PointF(100, 100));
            e.HasMorePages = false;
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e) 
        {
            Parametros.Config.SetValueByKeyAppSettings(Parametros.Config.strPrinterLocal, cboPrinter.Text.ToString());
            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;            
            this.Close();
        }

        #endregion
  
    }
}