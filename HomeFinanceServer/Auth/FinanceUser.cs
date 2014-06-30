using System;
using BasicAuth;
using HomeFinanceServer.Data;

namespace HomeFinanceServer.Auth {
	public class FinanceUser : BasicUser {
		#region public int ID
		/// <summary>
		/// Gets the ID of the FinanceUser
		/// </summary>
		/// <value></value>
		public int ID {
			get {
				return _iD;
			}
			set {
				_iD = value;
			}
		}
		private int _iD;
		#endregion
		#region public DateTime CreatedDate
		/// <summary>
		/// Gets the CreatedDate of the FinanceUser
		/// </summary>
		/// <value></value>
		public DateTime CreatedDate {
			get {
				return _createdDate;
			}
		}
		private DateTime _createdDate;
		#endregion

		#region public override bool IsAuthenticated
		/// <summary>
		/// Gets the IsAuthenticated of the FinanceUser
		/// </summary>
		/// <value></value>
		public override bool IsAuthenticated {
			get {
				return !string.IsNullOrEmpty( UserName );
			}
		}
		#endregion

		#region internal static FinanceUser Load( string username )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		internal static FinanceUser Load( string username ) {
			if( string.IsNullOrEmpty( username ) ) {
				return null;
			}
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = "GetUser";
				cmd.AddWithValue( "@Username", username );
				if( cmd.Read() ) {
					return new FinanceUser {
						_iD = cmd.GetInt( "User_ID" ), _createdDate = cmd.GetDateTime( "CreatedDate" ), UserName = username
					};
				}
			}
			return null;
		}
		#endregion
	}
}