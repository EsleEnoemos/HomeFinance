namespace ElectricBills {
	partial class ElectricBillExcelProperties {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbSender = new System.Windows.Forms.TextBox();
			this.tbReceiver = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.numBillNumber = new System.Windows.Forms.NumericUpDown();
			this.btnSetReceiver = new System.Windows.Forms.Button();
			this.btnSetSender = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numBillNumber)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 75);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Fakturanummer";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Avsändare";
			// 
			// tbSender
			// 
			this.tbSender.AcceptsReturn = true;
			this.tbSender.Location = new System.Drawing.Point(105, 7);
			this.tbSender.Multiline = true;
			this.tbSender.Name = "tbSender";
			this.tbSender.ReadOnly = true;
			this.tbSender.Size = new System.Drawing.Size(402, 59);
			this.tbSender.TabIndex = 4;
			// 
			// tbReceiver
			// 
			this.tbReceiver.AcceptsReturn = true;
			this.tbReceiver.Location = new System.Drawing.Point(105, 98);
			this.tbReceiver.Multiline = true;
			this.tbReceiver.Name = "tbReceiver";
			this.tbReceiver.ReadOnly = true;
			this.tbReceiver.Size = new System.Drawing.Size(402, 59);
			this.tbReceiver.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Mottagare";
			// 
			// numBillNumber
			// 
			this.numBillNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.numBillNumber.Location = new System.Drawing.Point(105, 73);
			this.numBillNumber.Name = "numBillNumber";
			this.numBillNumber.Size = new System.Drawing.Size(431, 20);
			this.numBillNumber.TabIndex = 7;
			// 
			// btnSetReceiver
			// 
			this.btnSetReceiver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSetReceiver.Location = new System.Drawing.Point(513, 101);
			this.btnSetReceiver.Name = "btnSetReceiver";
			this.btnSetReceiver.Size = new System.Drawing.Size(30, 23);
			this.btnSetReceiver.TabIndex = 8;
			this.btnSetReceiver.Text = "...";
			this.btnSetReceiver.UseVisualStyleBackColor = true;
			this.btnSetReceiver.Click += new System.EventHandler(this.btnSetReceiver_Click);
			// 
			// btnSetSender
			// 
			this.btnSetSender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSetSender.Location = new System.Drawing.Point(513, 10);
			this.btnSetSender.Name = "btnSetSender";
			this.btnSetSender.Size = new System.Drawing.Size(30, 23);
			this.btnSetSender.TabIndex = 9;
			this.btnSetSender.Text = "...";
			this.btnSetSender.UseVisualStyleBackColor = true;
			this.btnSetSender.Click += new System.EventHandler(this.btnSetSender_Click);
			// 
			// ElectricBillExcelProperties
			// 
			this.Controls.Add(this.btnSetSender);
			this.Controls.Add(this.btnSetReceiver);
			this.Controls.Add(this.numBillNumber);
			this.Controls.Add(this.tbReceiver);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbSender);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FileDlgOkCaption = "&Save";
			this.FileDlgStartLocation = FileDialogExtenders.AddonWindowLocation.Bottom;
			this.Name = "ElectricBillExcelProperties";
			this.Size = new System.Drawing.Size(555, 185);
			this.Load += new System.EventHandler(this.ElectricBillExcelProperties_Load);
			((System.ComponentModel.ISupportInitialize)(this.numBillNumber)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbSender;
		private System.Windows.Forms.TextBox tbReceiver;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numBillNumber;
		private System.Windows.Forms.Button btnSetReceiver;
		private System.Windows.Forms.Button btnSetSender;
	}
}
