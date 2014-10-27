using System.Windows.Forms;
using HomeFinance;

namespace Flygaretorpet.se {
	public partial class LoginDialog : Form {
		private HomeFinanceContext ctx;
		#region public string Server
		/// <summary>
		/// Gets the Server of the LoginDialog
		/// </summary>
		/// <value></value>
		public string Server {
			get {
				return tbServer.Text.Trim();
			}
		}
		#endregion
		#region public string EMail
		/// <summary>
		/// Gets the EMail of the LoginDialog
		/// </summary>
		/// <value></value>
		public string EMail {
			get {
				return tbEmail.Text.Trim();
			}
		}
		#endregion
		#region public string Password
		/// <summary>
		/// Gets the Password of the LoginDialog
		/// </summary>
		/// <value></value>
		public string Password {
			get {
				return tbPassword.Text.Trim();
			}
		}
		#endregion

		#region public bool Failed
		/// <summary>
		/// Sets the Failed of the LoginDialog
		/// </summary>
		/// <value></value>
		public bool Failed {
			set {
				lblLoginFailed.Visible = value;
			}
		}
		#endregion

		public LoginDialog()
			: this(null) {
		}
		public LoginDialog( HomeFinanceContext ctx ) {
			this.ctx = ctx;
			InitializeComponent();
		}

		private void LoginDialog_FormClosing( object sender, FormClosingEventArgs e ) {
			if( DialogResult != DialogResult.OK ) {
				return;
			}
			if( string.IsNullOrEmpty( tbServer.Text.Trim() ) ) {
				MessageBox.Show( "Please enter a server" );
				e.Cancel = true;
				return;
			}
			if( !tbServer.Text.Trim().Contains( "." ) ) {
				MessageBox.Show( "Please enter a valid server..." );
				e.Cancel = true;
				return;
			}
			if( tbServer.Text.Trim().Contains( ":" ) || tbServer.Text.Trim().Contains( "/" ) || tbServer.Text.Trim().Contains( " " ) ) {
				MessageBox.Show( "Server must be specified as \"server.domain\", without ports, protocols etc." );
				e.Cancel = true;
				return;
			}
			if( string.IsNullOrEmpty( tbEmail.Text.Trim() ) ) {
				MessageBox.Show( "Please enter an email" );
				e.Cancel = true;
				return;
			}
			if( string.IsNullOrEmpty( tbPassword.Text.Trim() ) ) {
				MessageBox.Show( "Please enter a password" );
				e.Cancel = true;
				return;
			}
			if( ctx != null ) {
				if( checkBox1.Checked ) {
					NameValueList settings = new NameValueList();
					settings[ "Flygaretorpet.se_Server" ] = tbServer.Text;
					settings[ "Flygaretorpet.se_Email" ] = tbEmail.Text;
					settings[ "Flygaretorpet.se_Password" ] = tbPassword.Text;
					ctx.Settings.Set( this, settings );
					return;
				}
				ctx.Settings.Set( this, new NameValueList());
			}
		}

		private void LoginDialog_Load( object sender, System.EventArgs e ) {
			if( ctx == null ) {
				return;
			}
			NameValueList settings = ctx.Settings.Get( this );
			tbServer.Text = settings[ "Flygaretorpet.se_Server" ];
			tbEmail.Text = settings[ "Flygaretorpet.se_Email" ];
			tbPassword.Text = settings[ "Flygaretorpet.se_Password" ];
			checkBox1.Checked = !string.IsNullOrEmpty( tbServer.Text );
		}
	}
}
