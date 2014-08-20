namespace Flygaretorpet.se {
	partial class GUI {
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
			this.lblHouseName = new System.Windows.Forms.Label();
			this.lblTotalBalance = new System.Windows.Forms.Label();
			this.btnAddInvoice = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoSize = true;
			this.panel1.Location = new System.Drawing.Point(4, 97);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(371, 162);
			this.panel1.TabIndex = 0;
			// 
			// lblHouseName
			// 
			this.lblHouseName.AutoSize = true;
			this.lblHouseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHouseName.Location = new System.Drawing.Point(4, 15);
			this.lblHouseName.Name = "lblHouseName";
			this.lblHouseName.Size = new System.Drawing.Size(104, 20);
			this.lblHouseName.TabIndex = 1;
			this.lblHouseName.Text = "houseName";
			this.lblHouseName.Visible = false;
			// 
			// lblTotalBalance
			// 
			this.lblTotalBalance.AutoSize = true;
			this.lblTotalBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTotalBalance.Location = new System.Drawing.Point(4, 45);
			this.lblTotalBalance.Name = "lblTotalBalance";
			this.lblTotalBalance.Size = new System.Drawing.Size(110, 20);
			this.lblTotalBalance.TabIndex = 2;
			this.lblTotalBalance.Text = "totalBalance";
			this.lblTotalBalance.Visible = false;
			// 
			// btnAddInvoice
			// 
			this.btnAddInvoice.Location = new System.Drawing.Point(8, 68);
			this.btnAddInvoice.Name = "btnAddInvoice";
			this.btnAddInvoice.Size = new System.Drawing.Size(75, 23);
			this.btnAddInvoice.TabIndex = 3;
			this.btnAddInvoice.Text = "Add invoice";
			this.btnAddInvoice.UseVisualStyleBackColor = true;
			this.btnAddInvoice.Visible = false;
			this.btnAddInvoice.Click += new System.EventHandler(this.btnAddInvoice_Click);
			// 
			// GUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.btnAddInvoice);
			this.Controls.Add(this.lblTotalBalance);
			this.Controls.Add(this.lblHouseName);
			this.Controls.Add(this.panel1);
			this.Name = "GUI";
			this.Size = new System.Drawing.Size(378, 262);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblHouseName;
		private System.Windows.Forms.Label lblTotalBalance;
		private System.Windows.Forms.Button btnAddInvoice;
	}
}
