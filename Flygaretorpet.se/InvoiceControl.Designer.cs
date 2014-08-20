namespace Flygaretorpet.se {
	partial class InvoiceControl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if( disposing && (components != null) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblName = new System.Windows.Forms.Label();
			this.pnlPayments = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel1.Controls.Add(this.lblName);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(555, 33);
			this.panel1.TabIndex = 0;
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblName.Location = new System.Drawing.Point(3, 7);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(59, 17);
			this.lblName.TabIndex = 2;
			this.lblName.Text = "invoice";
			// 
			// pnlPayments
			// 
			this.pnlPayments.AutoSize = true;
			this.pnlPayments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPayments.Location = new System.Drawing.Point(0, 33);
			this.pnlPayments.Name = "pnlPayments";
			this.pnlPayments.Size = new System.Drawing.Size(555, 51);
			this.pnlPayments.TabIndex = 1;
			// 
			// InvoiceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlPayments);
			this.Controls.Add(this.panel1);
			this.Name = "InvoiceControl";
			this.Size = new System.Drawing.Size(555, 84);
			this.Load += new System.EventHandler(this.InvoiceControl_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Panel pnlPayments;
	}
}
