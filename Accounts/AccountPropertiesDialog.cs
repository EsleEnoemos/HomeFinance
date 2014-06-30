using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using HomeFinance;
using HomeFinance.Shared;

namespace Accounts {
	public partial class AccountPropertiesDialog : Form {
		private Account account;
		private HomeFinanceContext ctx;
		#region public Account Account
		/// <summary>
		/// Gets the Account of the AccountPropertiesDialog
		/// </summary>
		/// <value></value>
		public Account Account {
			get {
				return _account;
			}
		}
		private Account _account;
		#endregion

		public AccountPropertiesDialog()
			: this( null, null ) {
		}
		public AccountPropertiesDialog( Account account, HomeFinanceContext ctx ) {
			this.account = account;
			this.ctx = ctx;
			InitializeComponent();
		}

		private void AccountPropertiesDialog_Load( object sender, System.EventArgs e ) {
			// checkbox height 17
			List<int> permittedUsers = new List<int>();
			if( account != null ) {
				textBox1.Text = account.Name;
				permittedUsers = account.PermittedUsers;
			}
			if( ctx == null ) {
				return;
			}
			BackgroundWorker bw = new BackgroundWorker();
			bw.DoWork += BwOnDoWork;
			bw.RunWorkerCompleted += BwOnRunWorkerCompleted;
			Enabled = false;
			Cursor = Cursors.AppStarting;
			bw.RunWorkerAsync( permittedUsers );
		}

		private void BwOnRunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
			try {
				int y = 0;
				List<int> permittedUsers = new List<int>();
				if( account != null ) {
					permittedUsers = account.PermittedUsers;
				}
				List<User> users = (List<User>)e.Result;
				foreach( User user in users ) {
					panel1.Controls.Add( new CheckBox {
						Text = user.Username, Left = 5, Top = y, Tag = user, Checked = permittedUsers.Contains( user.ID ) || (account == null && string.Equals( user.Username, ctx.CurrentUsername )), Enabled = !string.Equals( user.Username, ctx.CurrentUsername )
					} );
					y += 19;
				}
			} finally {
				Cursor = Cursors.Default;
				Enabled = true;
			}
		}

		private void BwOnDoWork( object sender, DoWorkEventArgs e ) {
			List<User> users = Plug.Users; // ctx.ServiceCaller.GetData<List<User>>( string.Format( "{0}/UserService/Get", ctx.ServiceBaseURL ) );
			e.Result = users;
		}

		private void AccountPropertiesDialog_FormClosing( object sender, FormClosingEventArgs e ) {
			if( ctx == null || DialogResult != DialogResult.OK ) {
				return;
			}
			if( string.IsNullOrEmpty( textBox1.Text ) ) {
				MessageBox.Show( "Du måste ange ett namn" );
			}
			_account = new Account {
				Name = textBox1.Text
			};
			if( account != null ) {
				_account.ID = account.ID;
			}
			foreach( Control c in panel1.Controls ) {
				CheckBox cb = c as CheckBox;
				if( cb == null || !cb.Checked ) {
					continue;
				}
				User u = (User)cb.Tag;
				_account.PermittedUsers.Add( u.ID );
			}
			NameValueCollection values = new NameValueCollection();
			values.Add( "Name", _account.Name );
			values.Add( "ID", _account.ID );
			foreach( int uid in _account.PermittedUsers ) {
				values.Add( "userid", uid );
			}
			_account = ctx.ServiceCaller.PostData<Account>( string.Format( "{0}/AccountService/Save", ctx.ServiceBaseURL ), values );
		}
	}
}
