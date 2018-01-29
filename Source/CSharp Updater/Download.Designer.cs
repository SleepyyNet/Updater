namespace CSharp_Updater
{
    partial class Download
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Download));
            this.label_Title = new System.Windows.Forms.Label();
            this.pictureBox_Logo = new System.Windows.Forms.PictureBox();
            this.progressBar_DownloadProgess = new System.Windows.Forms.ProgressBar();
            this.label_Status = new System.Windows.Forms.Label();
            this.label_Content = new System.Windows.Forms.Label();
            this.label_Version = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.label_Speed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.Location = new System.Drawing.Point(51, 13);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(115, 13);
            this.label_Title.TabIndex = 1;
            this.label_Title.Text = "Iron Dices Downloader";
            // 
            // pictureBox_Logo
            // 
            this.pictureBox_Logo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Logo.Image")));
            this.pictureBox_Logo.Location = new System.Drawing.Point(13, 13);
            this.pictureBox_Logo.Name = "pictureBox_Logo";
            this.pictureBox_Logo.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Logo.TabIndex = 2;
            this.pictureBox_Logo.TabStop = false;
            // 
            // progressBar_DownloadProgess
            // 
            this.progressBar_DownloadProgess.Location = new System.Drawing.Point(12, 64);
            this.progressBar_DownloadProgess.Name = "progressBar_DownloadProgess";
            this.progressBar_DownloadProgess.Size = new System.Drawing.Size(465, 23);
            this.progressBar_DownloadProgess.TabIndex = 3;
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(12, 48);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(65, 13);
            this.label_Status.TabIndex = 5;
            this.label_Status.Text = "label_Status";
            // 
            // label_Content
            // 
            this.label_Content.AutoSize = true;
            this.label_Content.Location = new System.Drawing.Point(51, 32);
            this.label_Content.Name = "label_Content";
            this.label_Content.Size = new System.Drawing.Size(72, 13);
            this.label_Content.TabIndex = 6;
            this.label_Content.Text = "label_Content";
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.Location = new System.Drawing.Point(164, 13);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(70, 13);
            this.label_Version.TabIndex = 7;
            this.label_Version.Text = "label_Version";
            // 
            // label_Speed
            // 
            this.label_Speed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Speed.Location = new System.Drawing.Point(381, 45);
            this.label_Speed.Name = "label_Speed";
            this.label_Speed.Size = new System.Drawing.Size(100, 16);
            this.label_Speed.TabIndex = 8;
            this.label_Speed.Text = "label_Speed";
            this.label_Speed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Download
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 98);
            this.Controls.Add(this.label_Speed);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.label_Content);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.progressBar_DownloadProgess);
            this.Controls.Add(this.pictureBox_Logo);
            this.Controls.Add(this.label_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Download";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Downloader";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.PictureBox pictureBox_Logo;
        private System.Windows.Forms.ProgressBar progressBar_DownloadProgess;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Label label_Content;
        private System.Windows.Forms.Label label_Version;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label label_Speed;
    }
}

