using System.Windows.Forms;
using Flygaretorpet.se.Classes;

namespace Flygaretorpet.se {
	public partial class InvoiceDialog : Form {
		private House house;
		public Invoice Invoice {
			get {
				return new Invoice {
					Amount = tbAmount.Text.ToDouble(),
					Date = dateTimePicker1.Value,
					Comment = tbComment.Text.Trim()
				};
			}
		}

		public InvoiceDialog()
			: this( null ) {
		}
		public InvoiceDialog( House house ) {
			this.house = house;
			InitializeComponent();
		}

		private void InvoiceDialog_Load( object sender, System.EventArgs e ) {
			if( house == null ) {
				return;
			}
			label2.Text = house.Name;
		}

		private void InvoiceDialog_FormClosing( object sender, FormClosingEventArgs e ) {
			if( DialogResult != DialogResult.OK ) {
				return;
			}
			double d = tbAmount.Text.ToDouble();
			if( d <= 0 ) {
				MessageBox.Show( "Please enter an amount, larger than 0" );
				e.Cancel = true;
				return;
			}
			if( string.IsNullOrEmpty( tbComment.Text.Trim() ) ) {
				MessageBox.Show( "Please enter a comment" );
				e.Cancel = true;
				return;
			}
		}
	}
}
