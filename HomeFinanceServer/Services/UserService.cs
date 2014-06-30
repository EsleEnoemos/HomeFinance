using System.Collections.Generic;
using System.Data;
using System.Web;
using HomeFinance.Shared;
using HomeFinanceServer.Data;

namespace HomeFinanceServer.Services {
	public class UserService {
		public List<User> Get( HttpContext context ) {
			List<User> list = new List<User>();
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "SELECT * FROM Users ORDER BY Username";
				while( cmd.Read() ) {
					list.Add( new User{Username = cmd.GetString( "Username" ), ID = cmd.GetInt( "User_ID" ) } );
				}
			}
			return list;
		}
	}
}