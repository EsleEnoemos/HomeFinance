using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using HomeFinance.Shared;
using HomeFinanceServer.Auth;
using HomeFinanceServer.Data;

namespace HomeFinanceServer.Services {
	public class AccountService {
		#region public List<Account> Get( HttpContext context )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public List<Account> Get( HttpContext context ) {
			FinanceUser user = FinanceUser.Load( context.User.Identity.Name );
			List<Account> list = new List<Account>();
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = "GetAccounts";
				cmd.AddWithValue( "@User_ID", user.ID );
				while( cmd.Read() ) {
					Account a = new Account {
						ID = cmd.GetInt( "Account_ID" ),
						Name = cmd.GetString( "Name" ),
						CreatedDate = cmd.GetDateTime( "CreatedDate" ),
						UserID = cmd.GetInt( "CreatedByUser_ID" )
					};
					a.Transactions = GetTransactions( a.ID );
					a.PermittedUsers = GetUserPermissions( a.ID );
					list.Add( a );
				}
			}

			return list;
		}
		#endregion
		#region public Account GetAccount( HttpContext context )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public Account GetAccount( HttpContext context ) {
			Account account = null;
			FinanceUser user = FinanceUser.Load( context.User.Identity.Name );
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = @"SELECT Accounts.* FROM Accounts
	INNER JOIN AccountPermissions ON AccountPermissions.Account_ID = Accounts.Account_ID
	WHERE AccountPermissions.User_ID = @User_ID AND Accounts.Account_ID = @Account_ID";
				cmd.AddWithValue( "@User_ID", user.ID );
				cmd.AddWithValue( "@Account_ID", context.Request.GetInt( "ID" ) );
				if( cmd.Read() ) {
					account = new Account {
						ID = cmd.GetInt( "Account_ID" ), CreatedDate = cmd.GetDateTime( "CreatedDate" ), Name = cmd.GetString( "Name" ), UserID = cmd.GetInt( "CreatedByUser_ID" )
					};
					account.Transactions = GetTransactions( account.ID );
					account.PermittedUsers = GetUserPermissions( account.ID );
				}
			}
			return account;
		}
		#endregion
		#region private List<AccountTransaction> GetTransactions( int accountID )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="accountID"></param>
		/// <returns></returns>
		private List<AccountTransaction> GetTransactions( int accountID ) {
			List<AccountTransaction> list = new List<AccountTransaction>();
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = "GetAccountTransactions";
				cmd.AddWithValue( "@Account_ID", accountID );
				while( cmd.Read() ) {
					list.Add( new AccountTransaction {
						ID = cmd.GetInt( "AccountTransaction_ID" ),
						Amount = cmd.GetDouble( "Amount" ),
						Comment = cmd.GetString( "Comment" ),
						UserID = cmd.GetInt( "User_ID" ),
						Date = cmd.GetDateTime( "Date" )
					} );
				}
			}
			return list;
		}
		#endregion
		#region private List<int> GetUserPermissions( int accountID )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="accountID"></param>
		/// <returns></returns>
		private List<int> GetUserPermissions( int accountID ) {
			List<int> list = new List<int>();
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = string.Format( "SELECT AccountPermissions.User_ID FROM AccountPermissions WHERE AccountPermissions.Account_ID = {0}", accountID );
				cmd.CommandType = CommandType.Text;
				while( cmd.Read() ) {
					list.Add( cmd.GetInt( "User_ID" ) );
				}
			}
			return list;
		}
		#endregion
		#region public Account Save( HttpContext context )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public Account Save( HttpContext context ) {
			HttpRequest req = context.Request;
			Account a = new Account {
				Name = req.GetString( "Name" ),
				ID = req.GetInt( "ID" )
			};
			string tmp = req.Form[ "userid" ];
			string[] userids = tmp.Contains( "," ) ? tmp.Split( ',' ) : new[] { tmp };
			foreach( string s in userids ) {
				a.PermittedUsers.Add( s.ToInt() );
			}
			using( DBCommand cmd = DBCommand.New ) {
				FinanceUser user = FinanceUser.Load( context.User.Identity.Name );
				cmd.CommandText = "UpdateAccount";
				SqlParameter id = cmd.Add( "@Account_ID", SqlDbType.Int, ParameterDirection.InputOutput, a.ID );
				cmd.AddWithValue( "@Name", a.Name );
				cmd.AddWithValue( "@User_ID", user.ID );
				cmd.ExecuteNonQuery();
				if( a.ID <= 0 ) {
					a.ID = (int)id.Value;
				}
				cmd.ClearParameters();
				cmd.CommandText = "ClearAccountPermissions";
				cmd.AddWithValue( "@Account_ID", a.ID );
				cmd.ExecuteNonQuery();
				cmd.CommandText = "AddAccountPermission";
				SqlParameter uid = cmd.Add( "@User_ID", SqlDbType.Int );
				foreach( int userid in a.PermittedUsers ) {
					uid.Value = userid;
					cmd.ExecuteNonQuery();
				}
			}
			a.Transactions = GetTransactions( a.ID );
			return a;
		}
		#endregion
		#region public AccountTransaction SaveTransaction( HttpContext context )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public AccountTransaction SaveTransaction( HttpContext context ) {
			FinanceUser user = FinanceUser.Load( context.User.Identity.Name );
			HttpRequest req = context.Request;
			int accountID = req.GetInt( "AccountID" );
			AccountTransaction t = new AccountTransaction {
				ID = req.GetInt( "ID" ),
				UserID = user.ID,
				Amount = req.GetDouble( "Amount" ),
				Comment = req.GetString( "Comment" ),
				Date = new DateTime( req.GetLong( "Date" ) )
			};

			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = "UpdateAccountTransaction";
				SqlParameter id = cmd.Add( "@AccountTransaction_ID", SqlDbType.Int, ParameterDirection.InputOutput, t.ID );
				cmd.AddWithValue( "@Account_ID", accountID );
				cmd.AddWithValue( "@Amount", t.Amount );
				cmd.AddWithValue( "@User_ID", user.ID );
				cmd.AddWithValue( "@Date", t.Date );
				cmd.AddWithValue( "@Comment", DBCommand.NullZero( t.Comment ) );
				cmd.ExecuteNonQuery();
				if( t.ID <= 0 ) {
					t.ID = (int)id.Value;
				}
			}
			return t;
		}
		#endregion
		#region public AccountTransaction DeleteTransaction( HttpContext context )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public AccountTransaction DeleteTransaction( HttpContext context ) {
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = "DeleteAccountTransaction";
				cmd.AddWithValue( "@AccountTransaction_ID", context.Request.GetInt( "ID" ) );
				cmd.ExecuteNonQuery();
			}
			return null;
		}
		#endregion
	}
}