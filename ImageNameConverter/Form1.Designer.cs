﻿namespace ImageNameConverter
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.lstOld = new System.Windows.Forms.ListBox();
            this.lstNew = new System.Windows.Forms.ListBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.picSelected = new System.Windows.Forms.PictureBox();
            this.PrbUmbennen = new System.Windows.Forms.ProgressBar();
            this.LblProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picSelected)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(12, 12);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(91, 23);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Dateien öffnen";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.BtnOpenFile_Click);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRename.Location = new System.Drawing.Point(641, 298);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(75, 23);
            this.btnRename.TabIndex = 2;
            this.btnRename.Text = "Umbennen";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.BtnRename_Click);
            // 
            // lstOld
            // 
            this.lstOld.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstOld.FormattingEnabled = true;
            this.lstOld.Location = new System.Drawing.Point(12, 41);
            this.lstOld.Name = "lstOld";
            this.lstOld.Size = new System.Drawing.Size(490, 147);
            this.lstOld.TabIndex = 3;
            this.lstOld.SelectedIndexChanged += new System.EventHandler(this.LstOld_SelectedIndexChanged);
            // 
            // lstNew
            // 
            this.lstNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstNew.FormattingEnabled = true;
            this.lstNew.Location = new System.Drawing.Point(12, 203);
            this.lstNew.Name = "lstNew";
            this.lstNew.Size = new System.Drawing.Size(490, 147);
            this.lstNew.TabIndex = 4;
            this.lstNew.SelectedIndexChanged += new System.EventHandler(this.LstNew_SelectedIndexChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // picSelected
            // 
            this.picSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSelected.Location = new System.Drawing.Point(508, 12);
            this.picSelected.Name = "picSelected";
            this.picSelected.Size = new System.Drawing.Size(208, 156);
            this.picSelected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSelected.TabIndex = 5;
            this.picSelected.TabStop = false;
            // 
            // PrbUmbennen
            // 
            this.PrbUmbennen.Location = new System.Drawing.Point(508, 327);
            this.PrbUmbennen.Name = "PrbUmbennen";
            this.PrbUmbennen.Size = new System.Drawing.Size(208, 23);
            this.PrbUmbennen.TabIndex = 6;
            // 
            // LblProgress
            // 
            this.LblProgress.AutoSize = true;
            this.LblProgress.Location = new System.Drawing.Point(508, 303);
            this.LblProgress.Name = "LblProgress";
            this.LblProgress.Size = new System.Drawing.Size(53, 13);
            this.LblProgress.TabIndex = 7;
            this.LblProgress.Text = "Fortschritt";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 362);
            this.Controls.Add(this.LblProgress);
            this.Controls.Add(this.PrbUmbennen);
            this.Controls.Add(this.picSelected);
            this.Controls.Add(this.lstNew);
            this.Controls.Add(this.lstOld);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnOpenFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Bilddateienname Umwandler by MrCode v1.0.3";
            ((System.ComponentModel.ISupportInitialize)(this.picSelected)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.ListBox lstOld;
        private System.Windows.Forms.ListBox lstNew;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.PictureBox picSelected;
        private System.Windows.Forms.ProgressBar PrbUmbennen;
        private System.Windows.Forms.Label LblProgress;
    }
}

