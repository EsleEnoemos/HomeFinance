using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using HomeFinance;
using HomeFinance.Data.Excel;
using HomeFinance.Shared;
using SpreadsheetGear;

namespace Accounts {
	public partial class GUI : UserControl {
		private HomeFinanceContext ctx;
		private AccountControl ac;
		private ToolStripMenuItem menu;
		private Account selectedAccount;

		public GUI()
			: this( null ) {
		}
		public GUI( HomeFinanceContext ctx ) {
			this.ctx = ctx;
			InitializeComponent();
		}
		public void UnloadControl( UnloadFinanceControlEventArgs e ) {
			if( ctx == null || ctx.TreeNode == null ) {
				return;
			}
			NameValueList settings = ctx.Settings.Get( this );
			settings[ "SelectedAccount" ] = "";
			if( ac != null ) {
				ac.UnloadControl( e );
				settings[ "SelectedAccount" ] = ac.AccountID.ToString();
			}
			ctx.TreeNode.TreeView.AfterSelect -= TreeViewOnAfterSelect;
			ctx.TreeNode.Nodes.Clear();
		}

		private void GUI_Load( object sender, EventArgs e ) {
			Dock = DockStyle.Fill;
			if( ctx == null || ctx.TreeNode == null ) {
				return;
			}
			menu = (ToolStripMenuItem)ctx.MainMenu.Items.Add( "Konton" );
			ToolStripItem excelMenu = menu.DropDownItems.Add( "Exportera transaktioner till Excel..." );
			excelMenu.Click += ExcelMenuOnClick;
			excelMenu = menu.DropDownItems.Add( "Importera transaktioner från Excel..." );
			excelMenu.Click += ImportFromExcelClick;

			ctx.TreeNode.TreeView.AfterSelect += TreeViewOnAfterSelect;
			if( ctx.TreeNode.ContextMenu == null ) {
				MenuItem mi = new MenuItem( "Lägg till konto..." );
				mi.Click += NewAccountClick;
				ctx.TreeNode.ContextMenu = new ContextMenu();
				ctx.TreeNode.ContextMenu.MenuItems.Add( mi );
			}
			LoadAccounts();
			NameValueList settings = ctx.Settings.Get( this );
			int selectedAccountID = settings[ "SelectedAccount" ].ToInt();
			if( selectedAccountID > 0 ) {
				foreach( TreeNode node in ctx.TreeNode.Nodes ) {
					Account a = node.Tag as Account;
					if( a != null && a.ID == selectedAccountID ) {
						node.TreeView.SelectedNode = node;
						break;
					}
				}
			}
		}

		private void ImportFromExcelClick( object sender, EventArgs eventArgs ) {
			if( selectedAccount == null ) {
				return;
			}
			OpenFileDialog of = new OpenFileDialog {
				CheckFileExists = true, Filter = "Excel 2007-2010|*.xlsx|Excel 97-2003|*.xls"
			};
			if( of.ShowDialog( this ) != DialogResult.OK ) {
				return;
			}
			ExcelParsedFile sheet = ExcelParsedFile.Load( of.FileName, 0 );
			while( sheet.MoveNext() ) {
				DateTime date;
				DateTime.TryParse( sheet.GetString( 0 ), out date );
				AccountTransaction t = new AccountTransaction { Date = date, Amount = sheet.GetDouble( 1 ), Comment = sheet.GetString( 2 ) };
				NameValueCollection values = new NameValueCollection();
				values.Add( "AccountID", selectedAccount.ID );
				values.Add( "ID", 0 );
				values.Add( "Amount", t.Amount );
				values.Add( "Comment", t.Comment );
				values.Add( "Date", t.Date.Ticks );
				AccountTransaction tt = ctx.ServiceCaller.PostData<AccountTransaction>( string.Format( "{0}/AccountService/SaveTransaction", ctx.ServiceBaseURL ), values );
				selectedAccount.Transactions.Add( tt );
			}
			LoadAccount( selectedAccount );
		}

		private void ExcelMenuOnClick( object sender, EventArgs eventArgs ) {
			if( selectedAccount == null ) {
				return;
			}
			SaveFileDialog sf = new SaveFileDialog{OverwritePrompt = true, Filter = "Excel 2007-2010|*.xlsx|Excel 97-2003|*.xls"};
			if( sf.ShowDialog(this) != DialogResult.OK) {
				return;
			}
			ExportPart ep = new ExportPart{SheetName = selectedAccount.Name,AutoFitColumnWidths = true};
			ep.Headers.Add( "Datum" );
			ep.Headers.Add( "Summa" );
			ep.Headers.Add( "Kommentar" );
			foreach( AccountTransaction t in selectedAccount.Transactions ) {
				CellDataList cd = new CellDataList();
				ep.Data.Add( cd );
				cd.Add( t.Date );
				string a = t.Amount.ToString();
				if( a.Contains( "," ) ) {
					a = a.Replace( ",", "." );
				}
				cd.Add( a );
				cd.Add( t.Comment );
			}
			FileFormat format = sf.FilterIndex == 1 ? FileFormat.OpenXMLWorkbook : FileFormat.Excel8;
			if( File.Exists( sf.FileName ) ) {
				File.Delete( sf.FileName );
			}
			using( Stream stream = sf.OpenFile() ) {
				ExcelFactory.ExportList( stream, new List<ExportPart>( new[] { ep } ), format );
				stream.Flush();
			}
		}

		private void NewAccountClick( object sender, EventArgs eventArgs ) {
			if( new AccountPropertiesDialog(null,ctx).ShowDialog(this) == DialogResult.OK ) {
				LoadAccounts();
			}
		}

		private void LoadAccounts() {
			selectedAccount = null;
			menu.Enabled = false;
			Controls.Clear();
			ac = null;
			BackgroundWorker bw = new BackgroundWorker();
			bw.DoWork += BWGetAccounts;
			bw.RunWorkerCompleted += BWComplete;
			TE( false );
			bw.RunWorkerAsync();
		}

		private void BWComplete( object sender, RunWorkerCompletedEventArgs e ) {
			TE( true );
			List<Account> accounts = (List<Account>)e.Result;
			int y = 0;
			ctx.TreeNode.Nodes.Clear();
			foreach( Account account in accounts ) {
				TreeNode node = ctx.TreeNode.Nodes.Add( account.Name );
				node.Tag = account;
				AccountSummaryControl asc = new AccountSummaryControl( account, ctx );
				asc.Top = y;
				asc.Width = Width - 5;
				asc.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
				Controls.Add( asc );
				y += 41;
			}
			ctx.TreeNode.Expand();
		}

		private void BWGetAccounts( object sender, DoWorkEventArgs e ) {
			List<Account> accounts = ctx.ServiceCaller.GetData<List<Account>>( string.Format( "{0}/AccountService/Get", ctx.ServiceBaseURL ) );
			e.Result = accounts;
		}

		private void LoadAccount( Account a ) {
			selectedAccount = a;
			menu.Enabled = true;
			Controls.Clear();
			ac = new AccountControl( a, ctx ){Dock = DockStyle.Fill};
			Controls.Add( ac );
		}
		private void TreeViewOnAfterSelect( object sender, TreeViewEventArgs e ) {
			if( ctx == null || e.Node == null ) {
				return;
			}
			Account a = e.Node.Tag as Account;
			if( a == null ) {
				LoadAccounts();
				return;
			}
			LoadAccount( a );
		}
		private void TE( bool enable ) {
			Cursor = enable ? Cursors.Default : Cursors.AppStarting;
			Enabled = enable;
			if( !enable ) {
				int h = Height;
				if( Application.OpenForms.Count > 0 ) {
					h = Application.OpenForms[ 0 ].Height;
				}
				Controls.Add( new Label {
					Text = "Laddar...", Left = (Width / 2) - 15, Top = h / 2
				} );
			} else {
				Controls.Clear();
			}
		}
	}
}
