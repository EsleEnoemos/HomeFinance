using System;

namespace HomeFinance {
	public class UnloadFinanceControlEventArgs : EventArgs {
		#region public bool Cancel
		/// <summary>
		/// Get/Sets the Cancel of the CloseEventArgs
		/// </summary>
		/// <value></value>
		public bool Cancel {
			get {
				return _cancel;
			}
			set {
				_cancel = value;
			}
		}
		private bool _cancel;
		#endregion
	}
}