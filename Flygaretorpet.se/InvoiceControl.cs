using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using Flygaretorpet.se.Classes;

namespace Flygaretorpet.se {
	public partial class InvoiceControl : UserControl {
		private Invoice invoice;
		private GUI gui;
		private House house;

		public InvoiceControl()
			: this( null, null, null ) {
		}
		public InvoiceControl( Invoice invoice, House house, GUI gui ) {
			this.invoice = invoice;
			this.gui = gui;
			this.house = house;
			InitializeComponent();
		}

		private void InvoiceControl_Load( object sender, EventArgs e ) {
			if( invoice == null ) {
				return;
			}
			lblName.Text = string.Format( "{0}, {1}, {2} kr", invoice.Name, invoice.Date.ToString( "yyyy-MM-dd" ), invoice.Amount );
			int y = 0;
			if( !string.IsNullOrEmpty( invoice.Comment ) ) {
				using( Font font = new Font( "Microsoft Sans Serif", 8, FontStyle.Italic ) ) {
					Label l = new Label { Text = invoice.Comment, Top = y, Left = 10, Font = font, AutoSize = true, Width = pnlPayments.Width - 5, Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right };
					using( Graphics gr = l.CreateGraphics() ) {
						SizeF measureString = gr.MeasureString( invoice.Comment, font );
						l.Height = (int)(measureString.Height + 5);
					}
					pnlPayments.Controls.Add( l );
					y += l.Height + 5;
				}
			}
			using( Font font = new Font( "Microsoft Sans Serif", 9 ) ) {
				Label l;
				foreach( InvoicePayment payment in invoice.Payments ) {
					l = new Label { Text = string.Format( "{0} kr", payment.Amount ), Top = y, Left = 10, Font = font, AutoSize = true};
					pnlPayments.Controls.Add( l );
					l = new Label { Text = string.Format( "{0}", payment.Date.ToString( "yyyy-MM-dd" ) ), Top = y, Left = 100, Font = font, AutoSize = true };
					pnlPayments.Controls.Add( l );
					y += l.Height + 5;
				}
				if( y == 0 ) {
					l = new Label { Text = "No payments found...", Top = y, Left = 10, Font = font, AutoSize = true };
					pnlPayments.Controls.Add( l );
					y += l.Height + 5;
				}
				l = new Label { Text = "Balance", Top = y, Left = 10, Font = font, AutoSize = true };
				l.ForeColor = invoice.Balance <= 0 ? Color.Green : Color.Red;
				pnlPayments.Controls.Add( l );
				l = new Label { Text = string.Format( "{0} kr", invoice.Balance ), Top = y, Left = 100, Font = font, AutoSize = true };
				l.ForeColor = invoice.Balance <= 0 ? Color.Green : Color.Red;
				pnlPayments.Controls.Add( l );
				y += l.Height + 5;

				Button b = new Button { Top = y, Left = 15, Font = font, AutoSize = true, Text = "Add payment" };
				b.Click += AddPaymentClick;
				pnlPayments.Controls.Add( b );
			}
		}
		private void AddPaymentClick( object sender, EventArgs eventArgs ) {
			PaymentDialog d = new PaymentDialog( invoice );
			if( d.ShowDialog( this ) != DialogResult.OK ) {
				return;
			}
			InvoicePayment ip = d.Payment;
			NameValueCollection data = new NameValueCollection();
			data.Add( "invoiceID", invoice.ID );
			data.Add( "amount", ip.Amount );
			data.Add( "date", ip.Date.ToString( "yyyy-MM-dd" ) );
			data.Add( "comment", ip.Comment );
			data.Add( "houseID", house.ID );
			Invoice i = Caller.Post<Invoice>( "AddInvoicePayment", data );
			if( i != null ) {
				gui.ShowCurrent();
			}
		}
	}
}
