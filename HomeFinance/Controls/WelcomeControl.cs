using System.Windows.Forms;

namespace HomeFinance.Controls {
	public partial class WelcomeControl : UserControl {
		public WelcomeControl() {
			InitializeComponent();
		}

		public void UnloadControl( UnloadFinanceControlEventArgs e ) {
		}
	}
	public class WelcomeControlLoader : IFinanceControl {
		private WelcomeControl ui;

		public string DisplayName {
			get {
				return "Home Finance";
			}
		}
		public void Dispose() {
			if( ui != null ) {
				ui.Dispose();
			}
		}


		public Control CreateUI() {
			ui = new WelcomeControl();
			return ui;
		}

		public void UnloadControl( UnloadFinanceControlEventArgs unloadFinanceControlEventArgs ) {
		}

		public void Init( HomeFinanceContext context ) {
			
		}
	}
}
