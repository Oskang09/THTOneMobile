namespace THTOneMobile
{
    partial class SalesEndTotal
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
            this.endBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.total = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // endBtn
            // 
            this.endBtn.Location = new System.Drawing.Point(384, 8);
            this.endBtn.Name = "endBtn";
            this.endBtn.Size = new System.Drawing.Size(88, 39);
            this.endBtn.TabIndex = 5;
            this.endBtn.Text = "退出";
            this.endBtn.UseVisualStyleBackColor = true;
            this.endBtn.Click += new System.EventHandler(this.endBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("KaiTi", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(44, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "总额";
            // 
            // total
            // 
            this.total.Cursor = System.Windows.Forms.Cursors.Default;
            this.total.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.total.Location = new System.Drawing.Point(12, 52);
            this.total.Name = "total";
            this.total.ReadOnly = true;
            this.total.Size = new System.Drawing.Size(460, 80);
            this.total.TabIndex = 3;
            this.total.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SalesEndTotal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 140);
            this.Controls.Add(this.endBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.total);
            this.Name = "SalesEndTotal";
            this.Text = "Total";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button endBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox total;
    }
}