namespace ElectricBills {
	partial class ElectricBillsControl {
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
			this.dpGuestLastReading = new System.Windows.Forms.DateTimePicker();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbTotalSum = new ElectricBills.NumericTextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.tbPricePerkWh = new ElectricBills.NumericTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbTotalGroundFee = new ElectricBills.NumericTextBox();
			this.tbTotalPriceElectricity = new ElectricBills.NumericTextBox();
			this.tbTotalUsedkWh = new ElectricBills.NumericTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnSaveToExcel = new System.Windows.Forms.Button();
			this.tbGuestTotalPriceIncVAT = new ElectricBills.NumericTextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.tbGuestTotalPrice = new ElectricBills.NumericTextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.tbGuestPriceGroundFeeIncVAT = new ElectricBills.NumericTextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.tbGuestPriceGroundFee = new ElectricBills.NumericTextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.tbGuestPriceElectricityIncVAT = new ElectricBills.NumericTextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.tbGuestPriceElectricity = new ElectricBills.NumericTextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.tbGuestPeriodUsedkWh = new ElectricBills.NumericTextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.cbFeePart = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.tbGuestCurrentReadingkWh = new ElectricBills.NumericTextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbGuestLastReadingkWh = new ElectricBills.NumericTextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.dpGuestCurrentReading = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnNewBill = new System.Windows.Forms.Button();
			this.label19 = new System.Windows.Forms.Label();
			this.cbPreviousBills = new System.Windows.Forms.ComboBox();
			this.label20 = new System.Windows.Forms.Label();
			this.tbOCR = new System.Windows.Forms.TextBox();
			this.tbTotalSumIncVAT = new ElectricBills.NumericTextBox();
			this.cbAddVAT = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// dpGuestLastReading
			// 
			this.dpGuestLastReading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dpGuestLastReading.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dpGuestLastReading.Location = new System.Drawing.Point(221, 19);
			this.dpGuestLastReading.Name = "dpGuestLastReading";
			this.dpGuestLastReading.Size = new System.Drawing.Size(127, 20);
			this.dpGuestLastReading.TabIndex = 5;
			this.dpGuestLastReading.ValueChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.tbTotalSum);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.tbPricePerkWh);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.tbTotalGroundFee);
			this.groupBox1.Controls.Add(this.tbTotalPriceElectricity);
			this.groupBox1.Controls.Add(this.tbTotalUsedkWh);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(474, 149);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Totalpris (ex moms)";
			// 
			// tbTotalSum
			// 
			this.tbTotalSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTotalSum.Location = new System.Drawing.Point(174, 122);
			this.tbTotalSum.Name = "tbTotalSum";
			this.tbTotalSum.ReadOnly = true;
			this.tbTotalSum.Size = new System.Drawing.Size(174, 20);
			this.tbTotalSum.TabIndex = 22;
			this.tbTotalSum.TabStop = false;
			this.tbTotalSum.Text = "0";
			this.tbTotalSum.Value = 0D;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(16, 125);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(117, 13);
			this.label10.TabIndex = 21;
			this.label10.Text = "Totalsumma (ex. moms)";
			// 
			// tbPricePerkWh
			// 
			this.tbPricePerkWh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPricePerkWh.Location = new System.Drawing.Point(174, 96);
			this.tbPricePerkWh.Name = "tbPricePerkWh";
			this.tbPricePerkWh.ReadOnly = true;
			this.tbPricePerkWh.Size = new System.Drawing.Size(174, 20);
			this.tbPricePerkWh.TabIndex = 20;
			this.tbPricePerkWh.TabStop = false;
			this.tbPricePerkWh.Text = "0";
			this.tbPricePerkWh.Value = 0D;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 99);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(105, 13);
			this.label5.TabIndex = 19;
			this.label5.Text = "Pris/kWh (ex. moms)";
			// 
			// tbTotalGroundFee
			// 
			this.tbTotalGroundFee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTotalGroundFee.Location = new System.Drawing.Point(174, 69);
			this.tbTotalGroundFee.Name = "tbTotalGroundFee";
			this.tbTotalGroundFee.Size = new System.Drawing.Size(294, 20);
			this.tbTotalGroundFee.TabIndex = 3;
			this.tbTotalGroundFee.Text = "0";
			this.tbTotalGroundFee.Value = 0D;
			this.tbTotalGroundFee.TextChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// tbTotalPriceElectricity
			// 
			this.tbTotalPriceElectricity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTotalPriceElectricity.Location = new System.Drawing.Point(174, 43);
			this.tbTotalPriceElectricity.Name = "tbTotalPriceElectricity";
			this.tbTotalPriceElectricity.Size = new System.Drawing.Size(294, 20);
			this.tbTotalPriceElectricity.TabIndex = 2;
			this.tbTotalPriceElectricity.Text = "0";
			this.tbTotalPriceElectricity.Value = 0D;
			this.tbTotalPriceElectricity.TextChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// tbTotalUsedkWh
			// 
			this.tbTotalUsedkWh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTotalUsedkWh.Location = new System.Drawing.Point(174, 17);
			this.tbTotalUsedkWh.Name = "tbTotalUsedkWh";
			this.tbTotalUsedkWh.Size = new System.Drawing.Size(294, 20);
			this.tbTotalUsedkWh.TabIndex = 1;
			this.tbTotalUsedkWh.Text = "0";
			this.tbTotalUsedkWh.Value = 0D;
			this.tbTotalUsedkWh.TextChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(156, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "Totalpris grundavgift (ex. moms)";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(111, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Totalpris el (ex. moms)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(119, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Total förbrukning (kWh)";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnSaveToExcel);
			this.groupBox2.Controls.Add(this.tbGuestTotalPriceIncVAT);
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.tbGuestTotalPrice);
			this.groupBox2.Controls.Add(this.label17);
			this.groupBox2.Controls.Add(this.tbGuestPriceGroundFeeIncVAT);
			this.groupBox2.Controls.Add(this.label16);
			this.groupBox2.Controls.Add(this.tbGuestPriceGroundFee);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.tbGuestPriceElectricityIncVAT);
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.tbGuestPriceElectricity);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.tbGuestPeriodUsedkWh);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.cbFeePart);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.tbGuestCurrentReadingkWh);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.tbGuestLastReadingkWh);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.dpGuestCurrentReading);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.dpGuestLastReading);
			this.groupBox2.Location = new System.Drawing.Point(3, 207);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(474, 332);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Gäst";
			// 
			// btnSaveToExcel
			// 
			this.btnSaveToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveToExcel.Location = new System.Drawing.Point(354, 300);
			this.btnSaveToExcel.Name = "btnSaveToExcel";
			this.btnSaveToExcel.Size = new System.Drawing.Size(114, 23);
			this.btnSaveToExcel.TabIndex = 11;
			this.btnSaveToExcel.Text = "Export to Excel...";
			this.btnSaveToExcel.UseVisualStyleBackColor = true;
			this.btnSaveToExcel.Click += new System.EventHandler(this.btnSaveToExcel_Click);
			// 
			// tbGuestTotalPriceIncVAT
			// 
			this.tbGuestTotalPriceIncVAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestTotalPriceIncVAT.Location = new System.Drawing.Point(221, 303);
			this.tbGuestTotalPriceIncVAT.Name = "tbGuestTotalPriceIncVAT";
			this.tbGuestTotalPriceIncVAT.ReadOnly = true;
			this.tbGuestTotalPriceIncVAT.Size = new System.Drawing.Size(127, 20);
			this.tbGuestTotalPriceIncVAT.TabIndex = 42;
			this.tbGuestTotalPriceIncVAT.TabStop = false;
			this.tbGuestTotalPriceIncVAT.Text = "0";
			this.tbGuestTotalPriceIncVAT.Value = 0D;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(16, 305);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(117, 13);
			this.label18.TabIndex = 41;
			this.label18.Text = "Totalsumma (ink moms)";
			// 
			// tbGuestTotalPrice
			// 
			this.tbGuestTotalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestTotalPrice.Location = new System.Drawing.Point(221, 277);
			this.tbGuestTotalPrice.Name = "tbGuestTotalPrice";
			this.tbGuestTotalPrice.ReadOnly = true;
			this.tbGuestTotalPrice.Size = new System.Drawing.Size(127, 20);
			this.tbGuestTotalPrice.TabIndex = 40;
			this.tbGuestTotalPrice.TabStop = false;
			this.tbGuestTotalPrice.Text = "0";
			this.tbGuestTotalPrice.Value = 0D;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(16, 279);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(114, 13);
			this.label17.TabIndex = 39;
			this.label17.Text = "Totalsumma (ex moms)";
			// 
			// tbGuestPriceGroundFeeIncVAT
			// 
			this.tbGuestPriceGroundFeeIncVAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestPriceGroundFeeIncVAT.Location = new System.Drawing.Point(221, 253);
			this.tbGuestPriceGroundFeeIncVAT.Name = "tbGuestPriceGroundFeeIncVAT";
			this.tbGuestPriceGroundFeeIncVAT.ReadOnly = true;
			this.tbGuestPriceGroundFeeIncVAT.Size = new System.Drawing.Size(127, 20);
			this.tbGuestPriceGroundFeeIncVAT.TabIndex = 36;
			this.tbGuestPriceGroundFeeIncVAT.TabStop = false;
			this.tbGuestPriceGroundFeeIncVAT.Text = "0";
			this.tbGuestPriceGroundFeeIncVAT.Value = 0D;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(16, 255);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(133, 13);
			this.label16.TabIndex = 35;
			this.label16.Text = "Pris grundavgift (ink moms)";
			// 
			// tbGuestPriceGroundFee
			// 
			this.tbGuestPriceGroundFee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestPriceGroundFee.Location = new System.Drawing.Point(221, 227);
			this.tbGuestPriceGroundFee.Name = "tbGuestPriceGroundFee";
			this.tbGuestPriceGroundFee.ReadOnly = true;
			this.tbGuestPriceGroundFee.Size = new System.Drawing.Size(127, 20);
			this.tbGuestPriceGroundFee.TabIndex = 34;
			this.tbGuestPriceGroundFee.TabStop = false;
			this.tbGuestPriceGroundFee.Text = "0";
			this.tbGuestPriceGroundFee.Value = 0D;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(16, 229);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(130, 13);
			this.label15.TabIndex = 33;
			this.label15.Text = "Pris grundavgift (ex moms)";
			// 
			// tbGuestPriceElectricityIncVAT
			// 
			this.tbGuestPriceElectricityIncVAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestPriceElectricityIncVAT.Location = new System.Drawing.Point(221, 201);
			this.tbGuestPriceElectricityIncVAT.Name = "tbGuestPriceElectricityIncVAT";
			this.tbGuestPriceElectricityIncVAT.ReadOnly = true;
			this.tbGuestPriceElectricityIncVAT.Size = new System.Drawing.Size(127, 20);
			this.tbGuestPriceElectricityIncVAT.TabIndex = 32;
			this.tbGuestPriceElectricityIncVAT.TabStop = false;
			this.tbGuestPriceElectricityIncVAT.Text = "0";
			this.tbGuestPriceElectricityIncVAT.Value = 0D;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(16, 203);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(141, 13);
			this.label14.TabIndex = 31;
			this.label14.Text = "Pris elförbrukning (ink moms)";
			// 
			// tbGuestPriceElectricity
			// 
			this.tbGuestPriceElectricity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestPriceElectricity.Location = new System.Drawing.Point(221, 175);
			this.tbGuestPriceElectricity.Name = "tbGuestPriceElectricity";
			this.tbGuestPriceElectricity.ReadOnly = true;
			this.tbGuestPriceElectricity.Size = new System.Drawing.Size(127, 20);
			this.tbGuestPriceElectricity.TabIndex = 30;
			this.tbGuestPriceElectricity.TabStop = false;
			this.tbGuestPriceElectricity.Text = "0";
			this.tbGuestPriceElectricity.Value = 0D;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(16, 177);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(138, 13);
			this.label13.TabIndex = 29;
			this.label13.Text = "Pris elförbrukning (ex moms)";
			// 
			// tbGuestPeriodUsedkWh
			// 
			this.tbGuestPeriodUsedkWh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestPeriodUsedkWh.Location = new System.Drawing.Point(221, 122);
			this.tbGuestPeriodUsedkWh.Name = "tbGuestPeriodUsedkWh";
			this.tbGuestPeriodUsedkWh.ReadOnly = true;
			this.tbGuestPeriodUsedkWh.Size = new System.Drawing.Size(127, 20);
			this.tbGuestPeriodUsedkWh.TabIndex = 28;
			this.tbGuestPeriodUsedkWh.TabStop = false;
			this.tbGuestPeriodUsedkWh.Text = "0";
			this.tbGuestPeriodUsedkWh.Value = 0D;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(16, 124);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(142, 13);
			this.label12.TabIndex = 27;
			this.label12.Text = "Periodens förbrukning (kWh)";
			// 
			// cbFeePart
			// 
			this.cbFeePart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbFeePart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFeePart.FormattingEnabled = true;
			this.cbFeePart.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
			this.cbFeePart.Location = new System.Drawing.Point(221, 148);
			this.cbFeePart.Name = "cbFeePart";
			this.cbFeePart.Size = new System.Drawing.Size(127, 21);
			this.cbFeePart.TabIndex = 9;
			this.cbFeePart.SelectedIndexChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(16, 150);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(84, 13);
			this.label9.TabIndex = 25;
			this.label9.Text = "Del i grundavgift";
			// 
			// tbGuestCurrentReadingkWh
			// 
			this.tbGuestCurrentReadingkWh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestCurrentReadingkWh.Location = new System.Drawing.Point(221, 96);
			this.tbGuestCurrentReadingkWh.Name = "tbGuestCurrentReadingkWh";
			this.tbGuestCurrentReadingkWh.Size = new System.Drawing.Size(127, 20);
			this.tbGuestCurrentReadingkWh.TabIndex = 8;
			this.tbGuestCurrentReadingkWh.Text = "0";
			this.tbGuestCurrentReadingkWh.Value = 0D;
			this.tbGuestCurrentReadingkWh.TextChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(16, 99);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(197, 13);
			this.label8.TabIndex = 23;
			this.label8.Text = "Förbrukning nuvarande avläsning (kWh)";
			// 
			// tbGuestLastReadingkWh
			// 
			this.tbGuestLastReadingkWh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGuestLastReadingkWh.Location = new System.Drawing.Point(221, 44);
			this.tbGuestLastReadingkWh.Name = "tbGuestLastReadingkWh";
			this.tbGuestLastReadingkWh.Size = new System.Drawing.Size(127, 20);
			this.tbGuestLastReadingkWh.TabIndex = 6;
			this.tbGuestLastReadingkWh.Text = "0";
			this.tbGuestLastReadingkWh.Value = 0D;
			this.tbGuestLastReadingkWh.TextChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(16, 47);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(200, 13);
			this.label7.TabIndex = 21;
			this.label7.Text = "Förbrukning föregående avläsning (kWh)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 71);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(140, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "Datum nuvarande avläsning";
			// 
			// dpGuestCurrentReading
			// 
			this.dpGuestCurrentReading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dpGuestCurrentReading.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dpGuestCurrentReading.Location = new System.Drawing.Point(221, 71);
			this.dpGuestCurrentReading.Name = "dpGuestCurrentReading";
			this.dpGuestCurrentReading.Size = new System.Drawing.Size(127, 20);
			this.dpGuestCurrentReading.TabIndex = 7;
			this.dpGuestCurrentReading.ValueChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(143, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Datum föregående avläsning";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(19, 184);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(117, 13);
			this.label11.TabIndex = 25;
			this.label11.Text = "Totalsumma (ink moms)";
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(396, 569);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 12;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnNewBill
			// 
			this.btnNewBill.Location = new System.Drawing.Point(22, 569);
			this.btnNewBill.Name = "btnNewBill";
			this.btnNewBill.Size = new System.Drawing.Size(75, 23);
			this.btnNewBill.TabIndex = 11;
			this.btnNewBill.Text = "Ny räkning";
			this.btnNewBill.UseVisualStyleBackColor = true;
			this.btnNewBill.Click += new System.EventHandler(this.btnNewBill_Click);
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(22, 599);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(92, 13);
			this.label19.TabIndex = 29;
			this.label19.Text = "Tidigare räkningar";
			// 
			// cbPreviousBills
			// 
			this.cbPreviousBills.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPreviousBills.FormattingEnabled = true;
			this.cbPreviousBills.Location = new System.Drawing.Point(120, 596);
			this.cbPreviousBills.Name = "cbPreviousBills";
			this.cbPreviousBills.Size = new System.Drawing.Size(253, 21);
			this.cbPreviousBills.TabIndex = 13;
			this.cbPreviousBills.SelectedIndexChanged += new System.EventHandler(this.dbPreviousBills_SelectedIndexChanged);
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(19, 158);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(128, 13);
			this.label20.TabIndex = 31;
			this.label20.Text = "OCR-nummer/anteckning";
			// 
			// tbOCR
			// 
			this.tbOCR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOCR.Location = new System.Drawing.Point(177, 155);
			this.tbOCR.Name = "tbOCR";
			this.tbOCR.Size = new System.Drawing.Size(294, 20);
			this.tbOCR.TabIndex = 4;
			this.tbOCR.TextChanged += new System.EventHandler(this.TextBoxChanged);
			// 
			// tbTotalSumIncVAT
			// 
			this.tbTotalSumIncVAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTotalSumIncVAT.Location = new System.Drawing.Point(177, 181);
			this.tbTotalSumIncVAT.Name = "tbTotalSumIncVAT";
			this.tbTotalSumIncVAT.ReadOnly = true;
			this.tbTotalSumIncVAT.Size = new System.Drawing.Size(174, 20);
			this.tbTotalSumIncVAT.TabIndex = 26;
			this.tbTotalSumIncVAT.TabStop = false;
			this.tbTotalSumIncVAT.Text = "0";
			this.tbTotalSumIncVAT.Value = 0D;
			// 
			// cbAddVAT
			// 
			this.cbAddVAT.AutoSize = true;
			this.cbAddVAT.Location = new System.Drawing.Point(22, 546);
			this.cbAddVAT.Name = "cbAddVAT";
			this.cbAddVAT.Size = new System.Drawing.Size(92, 17);
			this.cbAddVAT.TabIndex = 10;
			this.cbAddVAT.Text = "Lägg till moms";
			this.cbAddVAT.UseVisualStyleBackColor = true;
			this.cbAddVAT.CheckedChanged += new System.EventHandler(this.cbAddVAT_CheckedChanged);
			// 
			// ElectricBillsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cbAddVAT);
			this.Controls.Add(this.tbOCR);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.cbPreviousBills);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.btnNewBill);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.tbTotalSumIncVAT);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.MinimumSize = new System.Drawing.Size(480, 580);
			this.Name = "ElectricBillsControl";
			this.Size = new System.Drawing.Size(480, 631);
			this.Load += new System.EventHandler(this.ElectricBillsControl_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dpGuestLastReading;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private NumericTextBox tbTotalGroundFee;
		private NumericTextBox tbTotalPriceElectricity;
		private NumericTextBox tbTotalUsedkWh;
		private System.Windows.Forms.Label label5;
		private NumericTextBox tbPricePerkWh;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.DateTimePicker dpGuestCurrentReading;
		private System.Windows.Forms.Label label1;
		private NumericTextBox tbGuestCurrentReadingkWh;
		private System.Windows.Forms.Label label8;
		private NumericTextBox tbGuestLastReadingkWh;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbFeePart;
		private System.Windows.Forms.Label label9;
		private NumericTextBox tbTotalSum;
		private System.Windows.Forms.Label label10;
		private NumericTextBox tbTotalSumIncVAT;
		private System.Windows.Forms.Label label11;
		private NumericTextBox tbGuestPeriodUsedkWh;
		private System.Windows.Forms.Label label12;
		private NumericTextBox tbGuestTotalPriceIncVAT;
		private System.Windows.Forms.Label label18;
		private NumericTextBox tbGuestTotalPrice;
		private System.Windows.Forms.Label label17;
		private NumericTextBox tbGuestPriceGroundFeeIncVAT;
		private System.Windows.Forms.Label label16;
		private NumericTextBox tbGuestPriceGroundFee;
		private System.Windows.Forms.Label label15;
		private NumericTextBox tbGuestPriceElectricityIncVAT;
		private System.Windows.Forms.Label label14;
		private NumericTextBox tbGuestPriceElectricity;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnNewBill;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.ComboBox cbPreviousBills;
		private System.Windows.Forms.Button btnSaveToExcel;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox tbOCR;
		private System.Windows.Forms.CheckBox cbAddVAT;
	}
}
