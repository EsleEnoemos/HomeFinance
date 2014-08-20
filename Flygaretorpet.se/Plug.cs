using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Flygaretorpet.se.Classes;
using HomeFinance;

namespace Flygaretorpet.se {
	public class Plug : IFinanceControl {
		private static HomeFinanceContext ctx;
		private GUI gui;

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
			if( string.IsNullOrEmpty( Caller.URL ) ) {
				ctx.TreeNode.Nodes.Add( "Please set configuration for Flygaretorpet.se in app-config!" );
			} else {
				ctx.TreeNode.TreeView.AfterSelect += TreeViewOnAfterSelect;
				List<House> houseList = Caller.Get<List<House>>( "GetHouses" );
				foreach( House h in houseList ) {
					TreeNode node = ctx.TreeNode.Nodes.Add( h.Name );
					node.Tag = h;
				}
				ctx.TreeNode.Expand();
			}
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
