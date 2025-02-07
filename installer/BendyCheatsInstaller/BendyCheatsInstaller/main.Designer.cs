namespace BendyCheatsInstaller
{
    partial class main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            btn_install = new Button();
            pgb_installProgress = new ProgressBar();
            rtb_output = new RichTextBox();
            SuspendLayout();
            // 
            // btn_install
            // 
            btn_install.Enabled = false;
            btn_install.Location = new Point(12, 12);
            btn_install.Name = "btn_install";
            btn_install.Size = new Size(75, 23);
            btn_install.TabIndex = 0;
            btn_install.Text = "Install";
            btn_install.UseVisualStyleBackColor = true;
            btn_install.Click += btn_install_Click;
            // 
            // pgb_installProgress
            // 
            pgb_installProgress.Location = new Point(12, 176);
            pgb_installProgress.Name = "pgb_installProgress";
            pgb_installProgress.Size = new Size(410, 23);
            pgb_installProgress.TabIndex = 1;
            // 
            // rtb_output
            // 
            rtb_output.Location = new Point(12, 41);
            rtb_output.Name = "rtb_output";
            rtb_output.ReadOnly = true;
            rtb_output.Size = new Size(410, 129);
            rtb_output.TabIndex = 2;
            rtb_output.Text = "";
            rtb_output.WordWrap = false;
            // 
            // main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 211);
            Controls.Add(rtb_output);
            Controls.Add(pgb_installProgress);
            Controls.Add(btn_install);
            Cursor = Cursors.Cross;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "main";
            Text = "Bendy Cheats Installer";
            ResumeLayout(false);
        }

        #endregion

        private Button btn_install;
        private ProgressBar pgb_installProgress;
        private RichTextBox rtb_output;
    }
}