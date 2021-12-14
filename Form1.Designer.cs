namespace Lab6_7
{
    partial class Form1
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
            this.contourPictureBox = new System.Windows.Forms.PictureBox();
            this.sourcePictureBox = new System.Windows.Forms.PictureBox();
            this.contourLbl = new System.Windows.Forms.Label();
            this.shapeCountLbl = new System.Windows.Forms.Label();
            this.returnedPointLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.contourPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourcePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // contourPictureBox
            // 
            this.contourPictureBox.Location = new System.Drawing.Point(438, 46);
            this.contourPictureBox.Name = "contourPictureBox";
            this.contourPictureBox.Size = new System.Drawing.Size(320, 204);
            this.contourPictureBox.TabIndex = 0;
            this.contourPictureBox.TabStop = false;
            // 
            // sourcePictureBox
            // 
            this.sourcePictureBox.Location = new System.Drawing.Point(60, 46);
            this.sourcePictureBox.Name = "sourcePictureBox";
            this.sourcePictureBox.Size = new System.Drawing.Size(312, 204);
            this.sourcePictureBox.TabIndex = 1;
            this.sourcePictureBox.TabStop = false;
            // 
            // contourLbl
            // 
            this.contourLbl.AutoSize = true;
            this.contourLbl.Location = new System.Drawing.Point(377, 321);
            this.contourLbl.Name = "contourLbl";
            this.contourLbl.Size = new System.Drawing.Size(46, 17);
            this.contourLbl.TabIndex = 2;
            this.contourLbl.Text = "label1";
            // 
            // shapeCountLbl
            // 
            this.shapeCountLbl.AutoSize = true;
            this.shapeCountLbl.Location = new System.Drawing.Point(521, 321);
            this.shapeCountLbl.Name = "shapeCountLbl";
            this.shapeCountLbl.Size = new System.Drawing.Size(46, 17);
            this.shapeCountLbl.TabIndex = 3;
            this.shapeCountLbl.Text = "label1";
            // 
            // returnedPointLbl
            // 
            this.returnedPointLbl.AutoSize = true;
            this.returnedPointLbl.Location = new System.Drawing.Point(408, 389);
            this.returnedPointLbl.Name = "returnedPointLbl";
            this.returnedPointLbl.Size = new System.Drawing.Size(46, 17);
            this.returnedPointLbl.TabIndex = 4;
            this.returnedPointLbl.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.returnedPointLbl);
            this.Controls.Add(this.shapeCountLbl);
            this.Controls.Add(this.contourLbl);
            this.Controls.Add(this.sourcePictureBox);
            this.Controls.Add(this.contourPictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.contourPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourcePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox contourPictureBox;
        private System.Windows.Forms.PictureBox sourcePictureBox;
        private System.Windows.Forms.Label contourLbl;
        private System.Windows.Forms.Label shapeCountLbl;
        private System.Windows.Forms.Label returnedPointLbl;
    }
}

