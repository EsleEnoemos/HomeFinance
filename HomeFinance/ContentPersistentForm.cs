using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HomeFinance {
	public class ContentPersistentForm : Form {
		#region public static PersistentFormSettings Settings
		/// <summary>
		/// Gets the Settings of the Form1
		/// </summary>
		/// <value></value>
		public static PersistentFormSettings Settings {
			get {
				return _settings ?? (_settings = PersistentFormSettings.Load());
			}
		}
		private static PersistentFormSettings _settings;
		#endregion
		#region public ContentPersistentForm()
		/// <summary>
		/// Initializes a new instance of the <b>ContentPersistentForm</b> class.
		/// </summary>
		public ContentPersistentForm() {
			FormClosed += ContentPersistentForm_FormClosed;
			Load += FormLoad;
		}
		#endregion

		#region private void LoadSettings()
		/// <summary>
		/// 
		/// </summary>
		private void LoadSettings() {
			List<Control> list = new List<Control>();
			foreach( Control c in Controls ) {
				if( c is Panel ) {
					foreach( Control child in c.Controls ) {
						list.Add( child );
					}
				} else {
					list.Add( c );
				}
			}
			for( int i = 0; i < list.Count; i++ ) {
				try {
					Control c = list[ i ];
					if( Settings[ c.Name ] == null ) {
						continue;
					}
					if( c is TextBox ) {
						c.Text = Settings[ c.Name ];
					} else if( c is CheckBox ) {
						((CheckBox)c).Checked = string.Equals( "true", Settings[ c.Name ] );
					} else if( c is RadioButton ) {
						((RadioButton)c).Checked = string.Equals( "true", Settings[ c.Name ] );
					} else if( c is GroupBox ) {
						foreach( Control cc in c.Controls ) {
							list.Add( cc );
						}
					}
				} catch {
				}
			}
			for( int i = 0; i < list.Count; i++ ) {
				try {
					Control c = list[ i ];
					if( Settings[ c.Name ] == null ) {
						continue;
					}
					if( c is ComboBox && Settings[ c.Name ].ToInt() > -1 ) {
						ComboBox cb = (ComboBox)c;
						if( cb.Items.Count > 0 ) {
							((ComboBox)c).SelectedIndex = Settings[ c.Name ].ToInt();
						}
					}
				} catch {
				}
			}
		}
		#endregion
		#region protected virtual void FormLoad( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the ContentPersistentForm's Load event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		protected virtual void FormLoad( object sender, EventArgs e ) {
			LoadSettings();
		}
		#endregion
		#region void ContentPersistentForm_FormClosed( object sender, FormClosedEventArgs e )
		/// <summary>
		/// This method is called when the ContentPersistentForm's FormClosed event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> of the event.</param>
		void ContentPersistentForm_FormClosed( object sender, FormClosedEventArgs e ) {
			Settings.Values = new List<NameValue>();
			List<Control> list = new List<Control>();
			foreach( Control c in Controls ) {
				if( c is Panel || c is GroupBox ) {
					foreach( Control child in c.Controls ) {
						list.Add( child );
					}
				} else {
					list.Add( c );
				}
			}
			for( int i = 0; i < list.Count; i++ ) {
				Control c = list[ i ];
				if( c is Label || c is Button ) {
					continue;
				}
				if( c is TextBox ) {
					Settings[ c.Name ] = c.Text;
				} else if( c is CheckBox ) {
					Settings[ c.Name ] = ((CheckBox)c).Checked ? "true" : "false";
				} else if( c is RadioButton ) {
					Settings[ c.Name ] = ((RadioButton)c).Checked ? "true" : "false";
				} else if( c is GroupBox || c is Panel ) {
					foreach( Control cc in c.Controls ) {
						list.Add( cc );
					}
				} else if( c is ComboBox ) {
					Settings[ c.Name ] = ((ComboBox)c).SelectedIndex.ToString();
				}
			}
			Settings.Save();
		}
		#endregion

		protected void Alert( string message ) {
			MessageBox.Show( message );
		}
	}
}
