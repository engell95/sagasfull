using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;

namespace SAGAS.Parametros.Forms.Dialogs
{
    public partial class WaitSplashForm : WaitForm
    {
        public WaitSplashForm()
        {
            InitializeComponent();
            this.progressPanelCenter.AutoHeight = true;
        }

        #region Overrides
      
        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanelCenter.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanelCenter.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }
    }
}