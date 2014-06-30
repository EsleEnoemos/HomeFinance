using System;
using System.Configuration;
using System.Windows.Forms;

namespace HomeFinance {
	public class HomeFinanceContext {
		public readonly string ServiceBaseURL; // = "http://homefinance.local";
		public readonly string CurrentUsername;
		public JSONWebServiceCaller ServiceCaller = new JSONWebServiceCaller();
		private Form1 mainForm;
		public TreeNode TreeNode;
		public readonly PluginSettingsList Settings;
		public readonly MenuStrip MainMenu;
		
		internal HomeFinanceContext( Form1 form, TreeNode treeNode, MenuStrip mainMenu ) {
			mainForm = form;
			TreeNode = treeNode;
			CurrentUsername = Environment.UserName;
			Settings = ContentPersistentForm.Settings.PluginSettings;
			MainMenu = mainMenu;
			ServiceBaseURL = ConfigurationManager.AppSettings[ "ServerURL" ];
		}
	}
}