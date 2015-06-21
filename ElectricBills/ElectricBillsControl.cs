using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using HomeFinance;
using HomeFinance.Data.Excel;
using FileDialogExtenders;
using HomeFinance.Shared;
using SpreadsheetGear;

namespace ElectricBills {
	public partial class ElectricBillsControl : UserControl {
		private bool changed;
		private HomeFinanceContext context;
		#region private BillList Bills
		/// <summary>
		/// Gets the Bills of the ElectricBillsControl
		/// </summary>
		/// <value></value>
		private BillList Bills {
			get {
				return _bills ?? (_bills = context.ServiceCaller.GetData<BillList>( string.Format( "{0}/BillService/Get", context.ServiceBaseURL ) ));
			}
		}
		private BillList _bills;
		#endregion

		private Bill bill;
		#region private double VAT
		/// <summary>
		/// Gets the VAT of the ElectricBillsControl
		/// </summary>
		/// <value></value>
		private double VAT {
			get {
				return cbAddVAT.Checked ? 1.15 : 1.0;
			}
		}
		#endregion

		#region public ElectricBillsControl()
		/// <summary>
		/// Initializes a new instance of the <b>ElectricBillsControl</b> class.
		/// </summary>
		public ElectricBillsControl()
			: this( null ) {
		}
		#endregion
		#region public ElectricBillsControl( HomeFinanceContext context )
		/// <summary>
		/// Initializes a new instance of the <b>ElectricBillsControl</b> class.
		/// </summary>
		/// <param name="context"></param>
		public ElectricBillsControl( HomeFinanceContext context ) {
			this.context = context;
			InitializeComponent();
			cbFeePart.SelectedIndex = 0;
		}
		#endregion

		#region public void UnloadControl( UnloadFinanceControlEventArgs e )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public void UnloadControl( UnloadFinanceControlEventArgs e ) {
			if( changed ) {
				DialogResult dr = MessageBox.Show( this, "Vill du spara dina ändringar?", "Spara ändringar?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3 );
				if( dr == DialogResult.Cancel ) {
					e.Cancel = true;
					return;
				}
				if( dr == DialogResult.Yes ) {
					Save();
				}
			}
			if( context.TreeNode.ContextMenu != null ) {
				context.TreeNode.ContextMenu.Dispose();
				context.TreeNode.ContextMenu = null;
			}
			ContentPersistentForm.Settings.ActiveBillIndex = cbPreviousBills.SelectedIndex;
			ContentPersistentForm.Settings[ "AddVAT" ] = cbAddVAT.Checked ? "true" : "false";
			ContentPersistentForm.Settings.Save();
		}
		#endregion

		#region private void TextBoxChanged( object sender, EventArgs e )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextBoxChanged( object sender, EventArgs e ) {
			changed = true;
			tbPricePerkWh.Value = tbTotalUsedkWh.Value > 0 ? tbTotalPriceElectricity.Value / tbTotalUsedkWh.Value : 0;
			tbTotalSum.Value = tbTotalUsedkWh.Value * tbPricePerkWh.Value + tbTotalGroundFee.Value;
			tbTotalSumIncVAT.Value = tbTotalSum.Value * VAT;

			tbGuestPeriodUsedkWh.Value = tbGuestCurrentReadingkWh.Value - tbGuestLastReadingkWh.Value;
			tbGuestPriceElectricity.Value = tbGuestPeriodUsedkWh.Value * tbPricePerkWh.Value;
			tbGuestPriceElectricityIncVAT.Value = tbGuestPriceElectricity.Value * VAT;
			tbGuestPriceGroundFee.Value = cbFeePart.SelectedIndex == 0 ? 0 : tbTotalGroundFee.Value * (1.0 / (cbFeePart.SelectedIndex * 1.0));
			tbGuestPriceGroundFeeIncVAT.Value = tbGuestPriceGroundFee.Value * VAT;
			tbGuestTotalPrice.Value = tbGuestPriceGroundFee.Value + tbGuestPriceElectricity.Value;
			tbGuestTotalPriceIncVAT.Value = tbGuestTotalPrice.Value * VAT;
		}
		#endregion
		#region private void ElectricBillsControl_Load( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the ElectricBillsControl's Load event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void ElectricBillsControl_Load( object sender, EventArgs e ) {
			context.TreeNode.ContextMenu = new ContextMenu();
			MenuItem menuItem = context.TreeNode.ContextMenu.MenuItems.Add( "Exportera räkningar till Excel..." );
			menuItem.Click += ExportAllToExcel;
			menuItem = context.TreeNode.ContextMenu.MenuItems.Add( "Importera räkningar från Excel..." );
			menuItem.Click += ImportFromExcel;
			cbAddVAT.Checked = string.Equals( ContentPersistentForm.Settings[ "AddVAT" ], "true" );
			LoadBills();
		}
		#endregion
		private void LoadBills() {
			int si = 0;
			foreach( Bill bb in Bills ) {
				int ind = cbPreviousBills.Items.Add( bb );
				if( ind == ContentPersistentForm.Settings.ActiveBillIndex ) {
					si = ind;
				}
			}
			if( si < cbPreviousBills.Items.Count ) {
				cbPreviousBills.SelectedIndex = si;
			}
			if( bill == null ) {
				return;
			}
			LoadBill( bill );
			changed = false;
		}
		private void ImportFromExcel( object sender, EventArgs eventArgs ) {
			if( changed ) {
				DialogResult dr = MessageBox.Show( this, string.Format( "Vill du spara dina ändringar?{0}Det kommer att gå förlorade annars!", Environment.NewLine ), "Spara ändringar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
				if( dr == DialogResult.Cancel ) {
					return;
				}
				if( dr == DialogResult.Yes ) {
					Save();
				}
			}
			changed = false;
			OpenFileDialog of = new OpenFileDialog {
				AutoUpgradeEnabled = true, Filter = "Excel 2007-2010|*.xlsx|Excel 97-2003|*.xls", CheckFileExists = true, Multiselect = false
			};
			if( of.ShowDialog(this) != DialogResult.OK ) {
				return;
			}
			ExcelParsedFile file = ExcelParsedFile.Load( of.FileName, 0 );
			while( file.MoveNext() ) {
				Bill b = new Bill {
					TotalUsedKWh = file.GetDouble( 0 ),
					TotalPriceElectricity = file.GetDouble( 1 ),
					TotalPriceGroundFee = file.GetDouble( 2 ),
					OCR = file.GetString( 3 ),
					GuestLastReadingTicks = file.GetDateTime( 4 ).Ticks,
					GuestLastReadingKWh = file.GetDouble( 5 ),
					GuestCurrentReadingTicks = file.GetDateTime( 6 ).Ticks,
					GuestCurrentReadingKWh = file.GetDouble( 7 ),
					GuestPartInGroundFee = file.GetInt( 8 )
				};
				SaveBill( b );
				Bills.Add( b );
			}
			LoadBills();
		}
		#region private void ExportAllToExcel( object sender, EventArgs eventArgs )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ExportAllToExcel( object sender, EventArgs eventArgs ) {
			SaveFileDialog sf = new SaveFileDialog {
				OverwritePrompt = true, Filter = "Excel 2007-2010|*.xlsx|Excel 97-2003|*.xls", AutoUpgradeEnabled = true
			};
			if( sf.ShowDialog( this ) != DialogResult.OK ) {
				return;
			}
			ExportPart ep = new ExportPart {
				SheetName = "Elräkningar",
				AutoFitColumnWidths = true
			};
			ep.Headers.Add( "Total förbrukning (kWh)" );
			ep.Headers.Add( "Totalpris el" );
			ep.Headers.Add( "Totalpris grundavgift" );
			ep.Headers.Add( "OCR" );
			ep.Headers.Add( "Datum föregående avläsning" );
			ep.Headers.Add( "Förbrukning föregående avläsning" );
			ep.Headers.Add( "Datum nuvarande avläsning" );
			ep.Headers.Add( "Förbrukning nuvarande avläsning" );
			ep.Headers.Add( "Del i grundavgift" );
			foreach( Bill b in Bills ) {
				CellDataList cd = new CellDataList();
				ep.Data.Add( cd );
				cd.Add( EFD( b.TotalUsedKWh ) );
				cd.Add( EFD( b.TotalPriceElectricity ) );
				cd.Add( EFD( b.TotalPriceGroundFee ) );
				cd.Add( b.OCR, false );
				cd.Add( new DateTime( b.GuestLastReadingTicks ).ToString( "yyyy-MM-dd" ) );
				cd.Add( EFD( b.GuestLastReadingKWh ) );
				cd.Add( new DateTime( b.GuestCurrentReadingTicks ).ToString( "yyyy-MM-dd" ) );
				cd.Add( EFD( b.GuestCurrentReadingKWh ) );
				cd.Add( b.GuestPartInGroundFee );
			}
			if( File.Exists( sf.FileName ) ) {
				try {
					File.Delete( sf.FileName );
				} catch( Exception ex ) {
					MessageBox.Show( "Kan inte skriva över existerande fil: {0}".FillBlanks( ex.Message ) );
					return;
				}
			}
			using( Stream s = File.OpenWrite( sf.FileName ) ) {
				ExcelFactory.ExportList( s, new List<ExportPart>( new[] { ep } ), sf.FilterIndex == 1 ? FileFormat.OpenXMLWorkbook : FileFormat.Excel8 );
				s.Flush();
				s.Close();
			}
		}
		#endregion
		private static string EFD( double d ) {
			string str = d.ToString();
			if( str.Contains( "," ) ) {
				str = str.Replace( ",", "." );
			}
			return str;
		}
		#region private void LoadBill( Bill b )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="b"></param>
		private void LoadBill( Bill b ) {
			tbTotalUsedkWh.Value = b.TotalUsedKWh;
			tbTotalPriceElectricity.Value = b.TotalPriceElectricity;
			tbTotalGroundFee.Value = b.TotalPriceGroundFee;
			tbGuestLastReadingkWh.Value = b.GuestLastReadingKWh;
			tbGuestCurrentReadingkWh.Value = b.GuestCurrentReadingKWh;
			dpGuestLastReading.Value = new DateTime( b.GuestLastReadingTicks );
			dpGuestCurrentReading.Value = new DateTime( b.GuestCurrentReadingTicks );
			cbFeePart.SelectedIndex = b.GuestPartInGroundFee;
			tbOCR.Text = b.OCR;
		}
		#endregion

		#region private void btnSave_Click( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the btnSave's Click event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void btnSave_Click( object sender, EventArgs e ) {
			Save();
		}
		#endregion
		#region private void Save()
		/// <summary>
		/// 
		/// </summary>
		private void Save() {
			bool wasNew = bill == null;
			if( bill == null ) {
				bill = new Bill();
				Bills.Add( bill );
				
			}
			bill.TotalUsedKWh = tbTotalUsedkWh.Value;
			bill.TotalPriceElectricity = tbTotalPriceElectricity.Value;
			bill.TotalPriceGroundFee = tbTotalGroundFee.Value;
			bill.GuestLastReadingKWh = tbGuestLastReadingkWh.Value;
			bill.GuestCurrentReadingKWh = tbGuestCurrentReadingkWh.Value;
			bill.GuestLastReadingTicks = dpGuestLastReading.Value.Ticks;
			bill.GuestCurrentReadingTicks = dpGuestCurrentReading.Value.Ticks;
			bill.GuestPartInGroundFee = cbFeePart.SelectedIndex;
			bill.OCR = tbOCR.Text;
			try {
				TE( false );
				SaveBill( bill );
			} catch( Exception ex ) {
				MessageBox.Show( string.Format( "Error saving bill: {0}", ex.Message ) );
			} finally {
				TE( true );
			}
			//Bills.Save();
			if( wasNew ) {
				int ind = cbPreviousBills.Items.Add( bill );
				cbPreviousBills.SelectedIndex = ind;
			}
			changed = false;
		}
		#endregion
		#region private void TE( bool enable )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="enable"></param>
		private void TE( bool enable ) {
			Cursor = enable ? Cursors.Default : Cursors.AppStarting;
			for( int i = 0; i < Controls.Count; i++ ) {
				TextBox tb = Controls[ i ] as TextBox;
				if( tb != null && !tb.ReadOnly ) {
					tb.Enabled = enable;
				} else {
					Controls[ i ].Enabled = enable;
				}
			}
		}
		#endregion
		#region private void SaveBill( Bill b )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="b"></param>
		private void SaveBill( Bill b ) {
			NameValueCollection values = new NameValueCollection();
			values.Add( "ElectricBill_ID", b.ID );
			values.Add( "TotalUsedKWh", b.TotalUsedKWh );
			values.Add( "TotalPriceElectricity", b.TotalPriceElectricity );
			values.Add( "TotalPriceGroundFee", b.TotalPriceGroundFee );
			values.Add( "GuestLastReadingTicks", b.GuestLastReadingTicks );
			values.Add( "GuestLastReadingKWh", b.GuestLastReadingKWh );
			values.Add( "GuestCurrentReadingTicks", b.GuestCurrentReadingTicks );
			values.Add( "GuestCurrentReadingKWh", b.GuestCurrentReadingKWh );
			values.Add( "GuestPartInGroundFee", b.GuestPartInGroundFee );
			values.Add( "OCR", b.OCR );
			values.Add( "CreatedDate", b.CreatedDate.Ticks );
			values.Add( "CreatedByUser_ID", b.CreatedByUserID );
			Bill bb = context.ServiceCaller.PostData<Bill>( string.Format( "{0}/BillService/Save", context.ServiceBaseURL ), values );
			if( b.ID <= 0 ) {
				b.ID = bb.ID;
			}
			b.CreatedByUserID = bb.CreatedByUserID;
		}
		#endregion

		#region private void btnNewBill_Click( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the btnNewBill's Click event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void btnNewBill_Click( object sender, EventArgs e ) {
			if( changed ) {
				UnloadFinanceControlEventArgs ce = new UnloadFinanceControlEventArgs();
				UnloadControl( ce );
				if( ce.Cancel ) {
					return;
				}
			}
			bill = null;
			Bill b = new Bill();
			b.GuestLastReadingTicks = dpGuestCurrentReading.Value.Ticks;
			b.GuestCurrentReadingTicks = DateTime.Now.Ticks;
			b.GuestPartInGroundFee = cbFeePart.SelectedIndex;
			b.GuestLastReadingKWh = tbGuestCurrentReadingkWh.Value;
			//Bills.Add( bill );
			LoadBill( b );
			changed = false;
		}
		#endregion

		#region private void dbPreviousBills_SelectedIndexChanged( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the dbPreviousBills's SelectedIndexChanged event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void dbPreviousBills_SelectedIndexChanged( object sender, EventArgs e ) {
			Bill bb = cbPreviousBills.SelectedItem as Bill;
			if( bb != null ) {
				bill = bb;
				LoadBill( bill );
				changed = false;
			}
		}
		#endregion

		#region private void btnSaveToExcel_Click( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the btnSaveToExcel's Click event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void btnSaveToExcel_Click( object sender, EventArgs e ) {
			SaveFileDialog sf = new SaveFileDialog {
				AutoUpgradeEnabled = true, OverwritePrompt = true, Filter = "Excel 2007-2010|*.xlsx|Excel 97-2003|*.xls"
			};

			//sf.ShowDialog()
			ElectricBillExcelProperties exp = new ElectricBillExcelProperties( sf ) {
				Sender = ContentPersistentForm.Settings.BillSender, Receiver = ContentPersistentForm.Settings.BillReceiver, BillNumber = ContentPersistentForm.Settings.BillNumber.ToInt()
			};

			if( sf.ShowDialog( exp, this ) != DialogResult.OK ) {
				return;
			}
			if( File.Exists( sf.FileName ) ) {
				try {
					File.Delete( sf.FileName );
				} catch {
					MessageBox.Show( "Kan inte skriva över filen... kontrollera att den inte är öppen i något annat program." );
					return;
				}
			}
			ContentPersistentForm.Settings.BillSender = exp.Sender;
			ContentPersistentForm.Settings.BillReceiver = exp.Receiver;
			ContentPersistentForm.Settings.BillNumber = exp.BillNumber.ToString();
			ContentPersistentForm.Settings.Save();

			ExportPart ep = new ExportPart {
				SheetName = "Elräkning", AutoFitColumnWidths = true
			};
			ep.Headers.Add( exp.Sender );
			ep.Headers.Add( "Faktura El" );
			ep.Data.Add( new CellDataList( new[] { "", "Fakturanummer {0}".FillBlanks( exp.BillNumber ) } ) );
			ep.Data.Add( new CellDataList( new[] { "", "Fakturadag {0}".FillBlanks( DateTime.Now.ToString() ) } ) );
			ep.Data.Add( new CellDataList( new[] { "", exp.Receiver } ) );
			ep.Data.Add( new CellDataList( new[] { "Betalningsvillkor", "Betalas samtidigt som månadshyran" } ) );
			ep.Data.Add( new CellDataList( new[] { "Avläst mätarställning {0}".FillBlanks( dpGuestLastReading.Value.ToString() ), tbGuestLastReadingkWh.Text } ) );
			ep.Data.Add( new CellDataList( new[] { "Avläst mätarställning {0}".FillBlanks( dpGuestCurrentReading.Value.ToString() ), tbGuestCurrentReadingkWh.Text } ) );
			ep.Data.Add( new CellDataList( new[] { "Periodens förbrukning (kWh)", FromDBL( tbGuestPeriodUsedkWh.Text ) } ) );
			ep.Data.Add( new CellDataList( new[] { "Pris/kWh", FromDBL( tbPricePerkWh.Text ) } ) );
			ep.Data.Add( new CellDataList( new[] { new CellData( "Nätavgift" ), new CellData( FromDBL( tbGuestPriceGroundFee.Text ) ) } ) );
			ep.Data.Add( new CellDataList( new[] { "Summa", FromDBL( tbGuestTotalPrice.Text ) } ) );
			ep.Data.Add( new CellDataList( new[] { "Moms", FromDBL( (tbGuestTotalPriceIncVAT.Value - tbGuestTotalPrice.Value).ToString() ) } ) );
			ep.Data.Add( new CellDataList( new[] { new CellData( "Belopp att betala" ) { Bold = true }, new CellData( FromDBL( tbGuestTotalPriceIncVAT.Text ) ) { Bold = true } } ) );
			FileFormat ff = sf.FilterIndex == 2 ? FileFormat.Excel8 : FileFormat.OpenXMLWorkbook;
			using( Stream stream = File.OpenWrite( sf.FileName ) ) {
				ExcelFactory.ExportList( stream, new List<ExportPart>( new[] { ep } ), ff );
				stream.Flush();
				stream.Close();
			}
		}
		#endregion
		#region private string FromDBL( string str )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private string FromDBL( string str ) {
			if( !string.IsNullOrEmpty( str ) ) {
				if( str.Contains( "," ) ) {
					str = str.Replace( ",", "." );
				}
			}
			return str;
		}
		#endregion

		private void cbAddVAT_CheckedChanged( object sender, EventArgs e ) {
			VATRenameLabel( label3 );
			VATRenameLabel( label4 );
			VATRenameLabel( label5 );
			VATRenameLabel( label10 );
			VATRenameLabel( label11 );
			VATRenameLabel( label13 );
			VATRenameLabel( label15 );
			VATRenameLabel( label17 );
			TextBoxChanged( null, null );
		}
		private void VATRenameLabel( Label l ) {
			string t = l.Text.Substring( 0, l.Text.IndexOf( "(" ) );
			l.Text = t + (cbAddVAT.Checked ? "(ex. " : "(ink. ") + "moms)";
		}
	}
	public class ElectricBillLoader : IFinanceControl {
		private ElectricBillsControl ui;
		private HomeFinanceContext ctx;

		public string DisplayName {
			get {
				return "Elräkningar";
			}
		}
		public void Init( HomeFinanceContext context ) {
			ctx = context;
		}

		public Control CreateUI() {
			ui = new ElectricBillsControl( ctx );
			return ui;
		}

		public void UnloadControl( UnloadFinanceControlEventArgs unloadFinanceControlEventArgs ) {
			if( ui != null ) {
				ui.UnloadControl( unloadFinanceControlEventArgs );
			}
		}


		public void Dispose() {
			if( ui != null ) {
				ui.Dispose();
			}
		}
	}
}
