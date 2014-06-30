using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using HomeFinance;
using HomeFinance.Shared;

namespace Accounts {
	public partial class AccountControl : UserControl {
		private Account account;
		private HomeFinanceContext ctx;
		#region public int AccountID
		/// <summary>
		/// Gets the AccountID of the AccountControl
		/// </summary>
		/// <value></value>
		public int AccountID {
			get {
				return account == null ? 0 : account.ID;
			}
		}
		#endregion

		#region public AccountControl()
		/// <summary>
		/// Initializes a new instance of the <b>AccountControl</b> class.
		/// </summary>
		public AccountControl()
			: this( null, null ) {
		}
		#endregion
		#region public AccountControl( Account account, HomeFinanceContext ctx )
		/// <summary>
		/// Initializes a new instance of the <b>AccountControl</b> class.
		/// </summary>
		/// <param name="account"></param>
		/// <param name="ctx"></param>
		public AccountControl( Account account, HomeFinanceContext ctx ) {
			this.account = account;
			this.ctx = ctx;
			InitializeComponent();
		}
		#endregion

		#region private void AccountControl_Load( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the AccountControl's Load event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void AccountControl_Load( object sender, EventArgs e ) {
			if( account == null ) {
				return;
			}
			account.Transactions.Sort( delegate( AccountTransaction x, AccountTransaction y ) {
				return x.Date.CompareTo( y.Date );
			} );
			grid.SelectionChanged += GridOnSelectionChanged;
			NameValueList settings = ctx.Settings.Get( this );
			for( int i = 0; i < grid.Columns.Count; i++ ) {
				grid.Columns[ i ].Width = settings[ string.Format( "GridColumn{0}", i ) ].ToInt( 100 );
			}
			grid.Rows.Clear();
			foreach( AccountTransaction t in account.Transactions ) {
				int ind = grid.Rows.Add();
				DataGridViewRow row = grid.Rows[ ind ];
				row.Tag = t;
				row.Cells[ "Date" ].Value = t.Date.ToString( "yyyy-MM-dd" );
				row.Cells[ "Amount" ].Value = t.Amount.Format();
				row.Cells[ "Comment" ].Value = t.Comment;
				row.Cells[ "User" ].Value = Plug.Users.GetByID( t.UserID ).Username;
			}
			checkBox1.Checked = string.Equals( settings[ "ShowAll" ], "true" );
			dateTimePicker1.Value = new DateTime( settings[ "FilterDate" ].ToLong( DateTime.Now.Ticks ) );
		}
		#endregion
		#region private void GridOnSelectionChanged( object sender, EventArgs eventArgs )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void GridOnSelectionChanged( object sender, EventArgs eventArgs ) {
			DataGridViewSelectedRowCollection rows = grid.SelectedRows;
			if( rows.Count == 0 ) {
				grid.EditMode = DataGridViewEditMode.EditOnEnter;
			} else {
				grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
				grid.CancelEdit();
			}
		}
		#endregion
		#region public void UnloadControl( UnloadFinanceControlEventArgs e )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public void UnloadControl( UnloadFinanceControlEventArgs e ) {
			NameValueList settings = ctx.Settings.Get( this );
			for( int i = 0; i < grid.Columns.Count; i++ ) {
				settings[ string.Format( "GridColumn{0}", i ) ] = grid.Columns[ i ].Width.ToString();
			}
			settings[ "ShowAll" ] = checkBox1.Checked ? "true" : "false";
			settings[ "FilterDate" ] = dateTimePicker1.Value.Ticks.ToString();
		}
		#endregion
		#region private void grid_UserDeletedRow( object sender, DataGridViewRowEventArgs e )
		/// <summary>
		/// This method is called when the grid's UserDeletedRow event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="DataGridViewRowEventArgs"/> of the event.</param>
		private void grid_UserDeletedRow( object sender, DataGridViewRowEventArgs e ) {
			AccountTransaction t = e.Row.Tag as AccountTransaction;
			if( t == null ) {
				return;
			}
			BackgroundWorker bw = new BackgroundWorker();
			bw.DoWork += BWDeleteRow;
			bw.RunWorkerCompleted += BwOnRunWorkerCompleted;
			TE( false );
			bw.RunWorkerAsync( t );
		}
		#endregion
		#region private void BwOnRunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BwOnRunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
			TE( true );
		}
		#endregion
		#region private void BWDeleteRow( object sender, DoWorkEventArgs e )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BWDeleteRow( object sender, DoWorkEventArgs e ) {
			AccountTransaction t = (AccountTransaction)e.Argument;
			NameValueCollection values = new NameValueCollection();
			values.Add( "ID", t.ID );
			ctx.ServiceCaller.PostData<AccountTransaction>( string.Format( "{0}/AccountService/DeleteTransaction", ctx.ServiceBaseURL ), values );
			account.Transactions.Remove( t );
		}
		#endregion
		#region private void grid_RowValidating( object sender, DataGridViewCellCancelEventArgs e )
		/// <summary>
		/// This method is called when the grid's RowValidating event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="DataGridViewCellCancelEventArgs"/> of the event.</param>
		private void grid_RowValidating( object sender, DataGridViewCellCancelEventArgs e ) {
			DataGridViewRow row = grid.Rows[ e.RowIndex ];
			if( row.IsNewRow ) {
				return;
			}
			if( grid.IsCurrentRowDirty ) {
				BackgroundWorker bw = new BackgroundWorker();
				bw.RunWorkerCompleted += BwOnRunWorkerCompleted;
				bw.DoWork += BWPostRow;
				TE( false );
				bw.RunWorkerAsync( row );
				//grid.Sort( grid.Columns[ 0 ], ListSortDirection.Ascending );
			}
		}
		#endregion
		#region private void BWPostRow( object sender, DoWorkEventArgs e )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BWPostRow( object sender, DoWorkEventArgs e ) {
			DataGridViewRow row = (DataGridViewRow)e.Argument;
			AccountTransaction t = row.Tag as AccountTransaction ?? new AccountTransaction();
			DateTime dateTime;
			string ds = (row.Cells[ "Date" ].Value??"").ToString();
			if( string.IsNullOrEmpty( ds ) ) {
				ds = DateTime.Now.ToString( "yyyy-MM-dd" );
			}
			DateTime.TryParse( ds, out dateTime );
			t.Date = dateTime;
			t.Amount = row.Cells[ "Amount" ].Value.ToString().ToDouble();
			row.Cells[ "Amount" ].Value = t.Amount.Format();
			t.Comment = (row.Cells[ "Comment" ].Value??"").ToString();
			NameValueCollection values = new NameValueCollection();
			values.Add( "AccountID", account.ID );
			values.Add( "ID", t.ID );
			values.Add( "Amount", t.Amount );
			values.Add( "Comment", t.Comment );
			values.Add( "Date", t.Date.Ticks );
			AccountTransaction tt = ctx.ServiceCaller.PostData<AccountTransaction>( string.Format( "{0}/AccountService/SaveTransaction", ctx.ServiceBaseURL ), values );
			if( t.ID <= 0 ) {
				t.ID = tt.ID;
				t.UserID = Plug.Users.GetByName( ctx.CurrentUsername ).ID;
				account.Transactions.Add( t );
				row.Tag = tt;
			}
			row.Cells[ "Date" ].Value = tt.Date;
			row.Cells[ "User" ].Value = ctx.CurrentUsername;
		}
		#endregion
		#region private void checkBox1_CheckedChanged( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the checkBox1's CheckedChanged event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void checkBox1_CheckedChanged( object sender, EventArgs e ) {
			dateTimePicker1.Visible = !checkBox1.Checked;
			FilterItems();
		}
		#endregion
		#region private void FilterItems()
		/// <summary>
		/// 
		/// </summary>
		private void FilterItems() {
			foreach( DataGridViewRow row in grid.Rows ) {
				AccountTransaction trans = row.Tag as AccountTransaction;
				if( trans == null ) {
					continue;
				}
				row.Visible = checkBox1.Checked || (trans.Date.Year == dateTimePicker1.Value.Year && trans.Date.Month == dateTimePicker1.Value.Month);
			}
			NameValueList settings = ctx.Settings.Get( this );
			settings[ "ShowAll" ] = checkBox1.Checked ? "true" : "false";
			settings[ "FilterDate" ] = dateTimePicker1.Value.Ticks.ToString();
		}
		#endregion
		#region private void dateTimePicker1_ValueChanged( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the dateTimePicker1's ValueChanged event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void dateTimePicker1_ValueChanged( object sender, EventArgs e ) {
			FilterItems();
		}
		#endregion

		private void TE( bool enable ) {
			Cursor = enable ? Cursors.Default : Cursors.AppStarting;
			Enabled = enable;
		}

		private void grid_CellEndEdit( object sender, DataGridViewCellEventArgs e ) {
			if( e.ColumnIndex != 1 ) {
				return;
			}
			DataGridViewCell cell = grid.Rows[ e.RowIndex ].Cells[ e.ColumnIndex ];
			if( cell.Value == null ) {
				return;
			}
			string s = cell.Value.ToString();
			if( !string.IsNullOrEmpty( s ) ) {
				cell.Value = s.ToDouble().Format();
			}
		}
	}
}
