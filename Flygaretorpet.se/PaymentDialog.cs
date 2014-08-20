using System.Windows.Forms;
using Flygaretorpet.se.Classes;

namespace Flygaretorpet.se {
	public partial class PaymentDialog : Form {
		private Invoice invoice;
		public InvoicePayment Payment {
			get {
				return new InvoicePayment {
					Amount = tbAmount.Text.ToDouble(),
					Comment = tbComment.Text.Trim(),
					Date = dateTimePicker1.Value
				};
			}
		}

		public PaymentDialog() : this( null ) {
		}
		public PaymentDialog( Invoice invoice ) {
			this.invoice = invoice;
			InitializeComponent();
		}

		private void PaymentDialog_Load( object sender, System.EventArgs e ) {
			if( invoice == null ) {
				return;
			}
			label2.Text = string.Format( "{0}, {1}", invoice.Comment, invoice.Date.ToString( "yyyy-MM-dd" ) );
		}

		private void PaymentDialog_FormClosing( object sender, FormClosingEventArgs e ) {
			if( DialogResult != DialogResult.OK ) {
				return;
			}
			double d = tbAmount.Text.ToDouble();
			if( d <= 0 ) {
				MessageBox.Show( "Please enter an amount, larger than 0" );
				e.Cancel = true;
				return;
			}
		}
	}
}
