using System.Windows.Forms;
using HomeFinance;
using HomeFinance.Shared;

namespace Accounts {
	public partial class AccountSummaryControl : UserControl {
		private HomeFinanceContext ctx;
		private Account account;

		public AccountSummaryControl() : this( null, null ) {
		}
		public AccountSummaryControl( Account account, HomeFinanceContext ctx ) {
			this.ctx = ctx;
			this.account = account;
			InitializeComponent();
		}

		private void AccountSummaryControl_Load( object sender, System.EventArgs e ) {
			if( account == null ) {
				return;
			}
			LoadInfo();
		}
		private void LoadInfo() {
			lblName.Text = account.Name;
			double balance = 0;
			foreach( AccountTransaction t in account.Transactions ) {
				balance += t.Amount;
			}
			lblBalance.Text = balance.Format();
		}

		private void button1_Click( object sender, System.EventArgs e ) {
			AccountPropertiesDialog ad = new AccountPropertiesDialog( account, ctx );
			if( ad.ShowDialog(this) == DialogResult.OK ) {
				account = ad.Account;
				LoadInfo();
				foreach( TreeNode node in ctx.TreeNode.Nodes ) {
					Account a = node.Tag as Account;
					if( a == null ) {
						continue;
					}
					if( a.ID == account.ID ) {
						node.Text = account.Name;
						break;
					}
				}
			}
		}
	}
}
