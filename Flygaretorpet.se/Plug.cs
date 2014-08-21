using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using Flygaretorpet.se.Classes;
using HomeFinance;

namespace Flygaretorpet.se {
	public class Plug : IFinanceControl {
		private static HomeFinanceContext ctx;
		private GUI gui;

		#region internal static string URL
		/// <summary>
		/// Gets the URL of the Plug
		/// </summary>
		/// <value></value>
		internal static string URL {
			get {
				if( _uRL == null ) {
					_uRL = ConfigurationManager.AppSettings[ "Flygaretorpet.URL" ];
					if( !string.IsNullOrEmpty( _uRL ) ) {
						_uRL = string.Format( "http://{0}/", _uRL );
					}
				}
				return _uRL;
			}
		}
		private static string _uRL;
		#endregion
		#region internal static string EMail
		/// <summary>
		/// Gets the EMail of the Plug
		/// </summary>
		/// <value></value>
		internal static string EMail {
			get { return _eMail ?? (_eMail = ConfigurationManager.AppSettings[ "Flygaretorpet.EMail" ]); }
		}
		private static string _eMail;
		#endregion
		#region internal static string Password
		/// <summary>
		/// Gets the Password of the Plug
		/// </summary>
		/// <value></value>
		internal static string Password {
			get { return _password ?? (_password = ConfigurationManager.AppSettings[ "Flygaretorpet.Password" ]); }
		}
		private static string _password;
		#endregion


		public void Dispose() {
		}
		public string DisplayName {
			get {
				return "Flygaretorpet.se";
			}
		}
		public Control CreateUI() {
			if( gui != null ) {
				gui.Dispose();
			}
			if( string.IsNullOrEmpty( URL ) ) {
				LoginDialog d = new LoginDialog();
				while( d.ShowDialog( ctx.MainMenu ) == DialogResult.OK ) {
					_uRL = string.Format( "http://{0}/", d.Server );
					_eMail = d.EMail;
					_password = d.Password;
					if( !string.IsNullOrEmpty( Caller.Login() ) ) {
						break;
					}
					d.Failed = true;
				}
			}
			if( string.IsNullOrEmpty( URL ) ) {
				ctx.TreeNode.Nodes.Add( "Invalid/missing connection details!" );
			} else {
				ctx.TreeNode.TreeView.AfterSelect += TreeViewOnAfterSelect;
				List<House> houseList = Caller.Get<List<House>>( "GetHouses" );
				foreach( House h in houseList ) {
					TreeNode node = ctx.TreeNode.Nodes.Add( h.Name );
					node.Tag = h;
				}
			}
			ctx.TreeNode.Expand();
			gui = new GUI();
			return gui;
		}
		private void TreeViewOnAfterSelect( object sender, TreeViewEventArgs e ) {
			if( gui == null ) {
				return;
			}
			TreeNode node = e.Node;
			if( node == null ) {
				return;
			}
			House house = node.Tag as House;
			if( house == null ) {
				return;
			}
			gui.Show( house );
		}
		public void UnloadControl( UnloadFinanceControlEventArgs unloadFinanceControlEventArgs ) {
			ctx.TreeNode.TreeView.AfterSelect += TreeViewOnAfterSelect;
			ctx.TreeNode.Nodes.Clear();
			gui.Dispose();
			gui = null;
		}
		public void Init( HomeFinanceContext context ) {
			ctx = context;
		}
	}
}
