using System.Windows.Forms;

namespace ElectricBills {
	public partial class MultilineInputForm : Form {
		public string Value {
			get {
				return textBox1.Text;
			}
			set {
				textBox1.Text = value;
			}
		}
		public MultilineInputForm() {
			InitializeComponent();
		}
	}
}
