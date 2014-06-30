using BasicAuth;

namespace HomeFinanceServer.Auth {
	public class MyAuthProvider : BasicAuthProvider {
		public override bool IsValidUser( string userName, string password, out IBasicUser user ) {
			if( !string.IsNullOrEmpty( userName ) ) {
				user = FinanceUser.Load( userName );
				return user != null;
			}
			user = null;
			return false;
		}
		public override bool IsRequestAllowed( System.Web.HttpRequest request, IBasicUser user ) {
			return user != null && !string.IsNullOrEmpty( user.UserName );
		}
	}
}