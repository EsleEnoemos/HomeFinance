using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using Flygaretorpet.se.Classes;

namespace Flygaretorpet.se {
	public partial class GUI : UserControl {
		private House currentHouse;
		public GUI() {
			InitializeComponent();
		}

		public void Show( House house ) {
			btnAddInvoice.Visible = true;
			currentHouse = house;
			panel1.Controls.Clear();
			if( house == null ) {
				btnAddInvoice.Visible = false;
				lblHouseName.Visible = false;
				lblTotalBalance.Visible = false;
				return;
			}
			lblHouseName.Text = house.Name;
			lblHouseName.Visible = true;
			NameValueCollection qs = new NameValueCollection();
			qs.Add( "houseID", house.ID );
			List<House> houses = Caller.Get<List<House>>( "GetHouses", qs );
			house = houses[ 0 ];
			double totalBalance = 0;
			house.Invoices.Sort(delegate( Invoice x, Invoice y ) { return x.Date.CompareTo( y.Date )*-1; });
			foreach( Invoice invoice in house.Invoices ) {
				totalBalance += invoice.Balance;
				InvoiceControl c = new InvoiceControl( invoice, house, this );
				c.Dock = DockStyle.Top;
				panel1.Controls.Add( c );
			}
			lblTotalBalance.Text = string.Format( "Total balance: {0} kr", totalBalance );
			if( totalBalance > 0 ) {
				lblTotalBalance.ForeColor = Color.Red;
			}
			lblTotalBalance.Visible = true;
		}
		public void ShowCurrent() {
			Show( currentHouse );
		}

		private void btnAddInvoice_Click( object sender, System.EventArgs e ) {
			InvoiceDialog d = new InvoiceDialog( currentHouse );
			if( d.ShowDialog( this ) != DialogResult.OK ) {
				return;
			}
			NameValueCollection data = new NameValueCollection();
			Invoice i = d.Invoice;
			data.Add( "amount", i.Amount );
			data.Add( "date", i.Date.ToString( "yyyy-MM-dd" ) );
			data.Add( "comment", i.Comment );
			data.Add( "houseID", currentHouse.ID );
			data.Add( "name", i.Name );
			Invoice ni = Caller.Post<Invoice>( "AddInvoice", data );
			if( ni != null ) {
				ShowCurrent();
			}
		}
	}
}
