using System;
using System.Windows.Forms;

namespace HomeFinance {
	public interface IFinanceControl : IDisposable {
		string DisplayName { get; }

		Control CreateUI();
		void UnloadControl( UnloadFinanceControlEventArgs unloadFinanceControlEventArgs );
		void Init( HomeFinanceContext context );
	}
}
