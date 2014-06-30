using System.Collections.Generic;
using System.Windows.Forms;
using HomeFinance;
using HomeFinance.Shared;

namespace Accounts {
	public class Plug : IFinanceControl {
		private static HomeFinanceContext ctx;
		private GUI gui;

		internal static List<User> Users {
			get {
				return _users ?? (_users = ctx.ServiceCaller.GetData<List<User>>( string.Format( "{0}/UserService/Get", ctx.ServiceBaseURL ) ));
			}
		}
		private static List<User> _users;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() {
			if( gui != null ) {
				gui.Dispose();
			}
		}

		public string DisplayName {
			get {
				return "Konton";
			}
		}

		public Control CreateUI() {
			return (gui = new GUI( ctx ));
		}

		public void UnloadControl( UnloadFinanceControlEventArgs e ) {
			if( gui != null ) {
				gui.UnloadControl( e );
			}
		}

		public void Init( HomeFinanceContext context ) {
			ctx = context;
		}
	}
}
