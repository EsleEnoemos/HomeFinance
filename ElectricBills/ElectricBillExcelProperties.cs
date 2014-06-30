using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ElectricBills {
	public partial class ElectricBillExcelProperties : FileDialogExtenders.FileDialogControlBase {
		public int BillNumber {
			get {
				return (int)numBillNumber.Value;
			}
			set {
				numBillNumber.Value = value;
			}
		}
		public string Sender {
			get {
				return tbSender.Text;
			}
			set {
				tbSender.Text = value;
			}
		}
		public string Receiver {
			get {
				return tbReceiver.Text;
			}
			set {
				tbReceiver.Text = value;
			}
		}

		private readonly FileDialog ParentDialog;
		public ElectricBillExcelProperties( FileDialog pd ) {
			InitializeComponent();
			ParentDialog = pd;
		}

		private void ElectricBillExcelProperties_Load( object sender, EventArgs e ) {
			if( ParentDialog != null ) {
				ParentDialog.FileOk += ParentDialogOnFileOk;
			}
			if( !string.IsNullOrEmpty( Sender ) ) {
				if( Sender.Contains( "\n" ) && !Sender.Contains( "\r\n" ) ) {
					Sender = Sender.Replace( "\n", "\r\n" );
				}
			}
			if( !string.IsNullOrEmpty( Receiver ) ) {
				if( Receiver.Contains( "\n" ) && !Receiver.Contains( "\r\n" ) ) {
					Receiver = Receiver.Replace( "\n", "\r\n" );
				}
			}
		}

		private void ParentDialogOnFileOk( object sender, CancelEventArgs cancelEventArgs ) {
			List<string> errors = new List<string>();
			if( BillNumber == 0 ) {
				errors.Add( "Du måste ange fakturanummer" );
			}
			if( string.IsNullOrEmpty( tbSender.Text ) ) {
				errors.Add( "Du måste ange avsändare" );
			}
			if( string.IsNullOrEmpty( tbReceiver.Text ) ) {
				errors.Add( "Du måste ange mottagare" );
			}

			if( errors.Count > 0 ) {
				MessageBox.Show( errors.ToString( Environment.NewLine ) );
				cancelEventArgs.Cancel = true;
			}
		}

		private void btnSetSender_Click( object sender, EventArgs e ) {
			SetText( tbSender, "Ange avsändare" );
		}

		private void SetText( TextBox tb, string title ) {
			MultilineInputForm f = new MultilineInputForm {
				Text = title,
				Value = tb.Text
			};
			if( f.ShowDialog(this) == DialogResult.OK ) {
				tb.Text = f.Value;
			}
		}

		private void btnSetReceiver_Click( object sender, EventArgs e ) {
			SetText( tbReceiver, "Ange mottagare" );
		}
	}
}
