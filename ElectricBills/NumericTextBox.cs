using System.Windows.Forms;

namespace ElectricBills {
	public class NumericTextBox : TextBox {
		private bool nonNumberEntered;
		public double Value {
			get {
				return Text.ToDouble();
			}
			set {
				Text = value.ToString();
			}
		}
		public bool HasValue {
			get {
				return !string.IsNullOrEmpty( Text );
			}
		}
		protected override void OnKeyDown( KeyEventArgs e ) {
			// Initialize the flag to false.
			nonNumberEntered = false;

			// Determine whether the keystroke is a number from the top of the keyboard.
			if( e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9 ) {
				// Determine whether the keystroke is a number from the keypad.
				if( e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9 ) {
					// Determine whether the keystroke is a backspace.
					if( e.KeyCode != Keys.Back ) {
						// A non-numerical keystroke was pressed.
						// Set the flag to true and evaluate in KeyPress event.
						nonNumberEntered = true;
					}
				}
			}
			//If shift key was pressed, it's not a number.
			if( ModifierKeys == Keys.Shift ) {
				nonNumberEntered = true;
			}
			if( e.KeyValue == 188 || e.KeyValue == 190 || e.KeyValue == 110 ) {
				nonNumberEntered = false;
			}
		}
		protected override void OnKeyPress( KeyPressEventArgs e ) {
			if( nonNumberEntered ) {
				e.Handled = true;
			}
		}
	}
}
