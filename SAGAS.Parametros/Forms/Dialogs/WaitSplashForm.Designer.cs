namespace SAGAS.Parametros.Forms.Dialogs
{
    partial class WaitSplashForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressPanelCenter = new DevExpress.XtraWaitForm.ProgressPanel();
            this.tableLayoutPanelCenter = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressPanelCenter
            // 
            this.progressPanelCenter.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanelCenter.Appearance.Options.UseBackColor = true;
            this.progressPanelCenter.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.progressPanelCenter.AppearanceCaption.Options.UseFont = true;
            this.progressPanelCenter.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.progressPanelCenter.AppearanceDescription.Options.UseFont = true;
            this.progressPanelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressPanelCenter.ImageHorzOffset = 20;
            this.progressPanelCenter.Location = new System.Drawing.Point(0, 17);
            this.progressPanelCenter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.progressPanelCenter.Name = "progressPanelCenter";
            this.progressPanelCenter.Size = new System.Drawing.Size(246, 39);
            this.progressPanelCenter.TabIndex = 0;
            this.progressPanelCenter.Text = "progressPanelCenter";
            this.progressPanelCenter.UseWaitCursor = true;
            // 
            // tableLayoutPanelCenter
            // 
            this.tableLayoutPanelCenter.AutoSize = true;
            this.tableLayoutPanelCenter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelCenter.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelCenter.ColumnCount = 1;
            this.tableLayoutPanelCenter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanelCenter.Controls.Add(this.progressPanelCenter, 0, 0);
            this.tableLayoutPanelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCenter.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelCenter.Name = "tableLayoutPanelCenter";
            this.tableLayoutPanelCenter.Padding = new System.Windows.Forms.Padding(0, 14, 0, 14);
            this.tableLayoutPanelCenter.RowCount = 1;
            this.tableLayoutPanelCenter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCenter.Size = new System.Drawing.Size(246, 73);
            this.tableLayoutPanelCenter.TabIndex = 1;
            this.tableLayoutPanelCenter.UseWaitCursor = true;
            // 
            // WaitSplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(246, 73);
            this.Controls.Add(this.tableLayoutPanelCenter);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(246, 0);
            this.Name = "WaitSplashForm";
            this.Text = "<>";
            this.TopMost = true;
            this.UseWaitCursor = true;
            this.tableLayoutPanelCenter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraWaitForm.ProgressPanel progressPanelCenter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCenter;
    }
}
