using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using HomeFinance.Controls;

namespace HomeFinance {
	public partial class Form1 : ContentPersistentForm {
		#region public List<IFinanceControl> FinanceControls
		/// <summary>
		/// Gets the FinanceControls of the Form1
		/// </summary>
		/// <value></value>
		public List<IFinanceControl> FinanceControls {
			get {
				if( _financeControls == null ) {
					lock( financeControlsLock ) {
						if( _financeControls == null ) {
							List<IFinanceControl> tmp = new List<IFinanceControl>();
							DirectoryInfo baseDir = new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData ) );
							baseDir = new DirectoryInfo( string.Format( "{0}\\HomeFinancePlugins", baseDir.FullName ) );
							if( !Directory.Exists( baseDir.FullName ) ) {
								Directory.CreateDirectory( baseDir.FullName );
							}
							foreach( DirectoryInfo pluginDir in baseDir.GetDirectories() ) {
								List<FileInfo> files = new List<FileInfo>( pluginDir.GetFiles( "*.dll", SearchOption.AllDirectories ) );
								files.AddRange( pluginDir.GetFiles( "*.exe", SearchOption.AllDirectories ) );
								foreach( FileInfo dll in files ) {
									try {
										Assembly ass = Assembly.LoadFrom( dll.FullName );
										Type[] types = ass.GetTypes();
										foreach( Type type in types ) {
											try {
												if( !type.IsAbstract ) {
													if( typeof( IFinanceControl ).IsAssignableFrom( type ) ) {
														IFinanceControl mt = ass.CreateInstance( type.FullName ) as IFinanceControl;
														if( mt != null ) {
															if( mt is WelcomeControlLoader ) {
																continue;
															}
															//mt.Init( new HomeFinanceContext( this ) );
															tmp.Add( mt );
														}
													}
												}
											} catch( Exception ) {
											}
										}
									} catch( ReflectionTypeLoadException rex ) {
										foreach( Type type in rex.Types ) {
											try {
												if( !type.IsAbstract ) {
													if( typeof( IFinanceControl ).IsAssignableFrom( type ) ) {
														IFinanceControl mt = type.Assembly.CreateInstance( type.FullName ) as IFinanceControl;
														if( mt != null ) {
															if( mt is WelcomeControlLoader ) {
																continue;
															}
															//mt.Init( new HomeFinanceContext( this ) );
															tmp.Add( mt );
														}
													}
												}
											} catch( Exception ) {
											}
										}
									} catch {
									}
								}
							}
							tmp.Sort( delegate( IFinanceControl x, IFinanceControl y ) {
								return string.Compare( x.DisplayName, y.DisplayName, false, CultureInfo.CurrentCulture );
							} );
							_financeControls = tmp;
						}
					}
				}
				return _financeControls;
			}
		}
		private volatile List<IFinanceControl> _financeControls;
		private object financeControlsLock = new object();
		#endregion
		#region private static WelcomeControlLoader WelcomeControl
		/// <summary>
		/// Gets the WelcomeControl of the Form1
		/// </summary>
		/// <value></value>
		private static WelcomeControlLoader WelcomeControl {
			get {
				return _welcomeControl ?? (_welcomeControl = new WelcomeControlLoader());
			}
		}
		private static WelcomeControlLoader _welcomeControl;
		#endregion

		private IFinanceControl activeControl;
		private List<ToolStripItem> pluginMenuItems;

		public Form1() {
			InitializeComponent();
		}

		#region private void Form1_Load( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the Form1's Load event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void Form1_Load( object sender, EventArgs e ) {
			if( Settings.WindowState.HasValue ) {
				FormWindowState ws = (FormWindowState)Settings.WindowState;
				if( ws == FormWindowState.Minimized ) {
					ws = FormWindowState.Normal;
				}
				Width = Settings.Width;
				Height = Settings.Height;
				Top = Settings.Top;
				Left = Settings.Left;
				WindowState = ws;
			}
			if( Settings.SplitterDistance.HasValue ) {
				splitContainer1.SplitterDistance = Settings.SplitterDistance.Value;
			}
			if( Settings.SplitterCollapsed ) {
				splitContainer1.Panel1Collapsed = true;
			}

			tvControls.Nodes.Clear();
			TreeNode sn = null;
			TreeNode root = tvControls.Nodes.Add( WelcomeControl.DisplayName );
			WelcomeControl.Init( new HomeFinanceContext( this, root, menuStrip1 ) );
			root.Tag = WelcomeControl;
			foreach( IFinanceControl fc in FinanceControls ) {
				TreeNode node = root.Nodes.Add( fc.DisplayName );
				node.Tag = fc; //.GetType();
				fc.Init( new HomeFinanceContext( this, node, menuStrip1 ) );
				if( string.Equals( fc.GetType().FullName, Settings.SelectedControlName ) ) {
					sn = node;
				}
			}

			root.Expand();
			if( sn == null ) {
				sn = root;
			}
			menuStrip1.ItemAdded += PluginMenuItemAdded;
			tvControls.SelectedNode = sn;
		}
		#endregion
		#region private void tvControls_AfterSelect( object sender, TreeViewEventArgs e )
		/// <summary>
		/// This method is called when the tvControls's AfterSelect event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="TreeViewEventArgs"/> of the event.</param>
		private void tvControls_AfterSelect( object sender, TreeViewEventArgs e ) {
			IFinanceControl fc = e.Node.Tag as IFinanceControl;
			if( fc == null || fc == activeControl ) {
				return;
			}
			if( splitContainer1.Panel2.Controls.Count > 0 ) {
				splitContainer1.Panel2.Controls[ 0 ].Dispose();
				splitContainer1.Panel2.Controls.Clear();
			}
			if( e.Node == null ) {
				return;
			}
			activeControl = fc;
			Control c = fc.CreateUI();
			if( c == null ) {
				return;
			}
			//Panel pnl = new Panel();
			//pnl.AutoScroll = true;
			//pnl.AutoSize = true;
			//pnl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			//pnl.Dock = DockStyle.Fill;
			//pnl.BackColor = System.Drawing.Color.Red;
			//pnl.Width = c.Width;
			//pnl.Height = c.Height;
			//c.Dock = DockStyle.Fill;
			//pnl.Controls.Add( c );
			c.Width = splitContainer1.Panel2.Width;
			c.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			ClearPluginMenuItems();
			splitContainer1.Panel2.Controls.Add( c );
		}
		#endregion
		#region private void PluginMenuItemAdded( object sender, ToolStripItemEventArgs e )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PluginMenuItemAdded( object sender, ToolStripItemEventArgs e ) {
			(pluginMenuItems ?? (pluginMenuItems = new List<ToolStripItem>())).Add( e.Item );
		}
		#endregion
		#region private void ClearPluginMenuItems()
		/// <summary>
		/// 
		/// </summary>
		private void ClearPluginMenuItems() {
			if( pluginMenuItems != null ) {
				for( int i = pluginMenuItems.Count - 1; i >= 0; i-- ) {
					pluginMenuItems[ i ].Dispose();
					try {
						menuStrip1.Items.Remove( pluginMenuItems[ i ] );
					} catch {
					}
				}
			}
			pluginMenuItems = new List<ToolStripItem>();
		}
		#endregion
		#region private void Form1_FormClosing( object sender, FormClosingEventArgs e )
		/// <summary>
		/// This method is called when the Form1's FormClosing event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="FormClosingEventArgs"/> of the event.</param>
		private void Form1_FormClosing( object sender, FormClosingEventArgs e ) {
			Settings.WindowState = (int)WindowState;
			if( WindowState != FormWindowState.Minimized ) {
				Settings.Width = Width;
				if( Settings.Width < MinimumSize.Width ) {
					Settings.Width = MinimumSize.Width;
				}
				Settings.Height = Height;
				if( Settings.Height < MinimumSize.Height ) {
					Settings.Height = MinimumSize.Height;
				}
				Settings.Top = Top;
				if( Settings.Top < 0 ) {
					Settings.Top = 0;
				}
				Settings.Left = Left;
				if( Settings.Left < 0 ) {
					Settings.Left = 0;
				}
			}
			Settings.SplitterDistance = splitContainer1.SplitterDistance;
			Settings.SplitterCollapsed = splitContainer1.Panel1Collapsed;
			if( splitContainer1.Panel2.Controls.Count == 0 ) {
				Settings.SelectedControlName = null;
				return;
			}
			if( activeControl == null ) {
				Settings.SelectedControlName = null;
				return;
			}
			UnloadFinanceControlEventArgs uea = new UnloadFinanceControlEventArgs();
			activeControl.UnloadControl( uea );
			e.Cancel = uea.Cancel;
			Settings.SelectedControlName = activeControl.GetType().FullName;
			Settings.Save();
		}
		#endregion
		#region private void tvControls_BeforeSelect( object sender, TreeViewCancelEventArgs e )
		/// <summary>
		/// This method is called when the tvControls's BeforeSelect event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="TreeViewCancelEventArgs"/> of the event.</param>
		private void tvControls_BeforeSelect( object sender, TreeViewCancelEventArgs e ) {
			if( splitContainer1.Panel2.Controls.Count == 0 ) {
				return;
			}
			if( activeControl == null ) {
				return;
			}
			IFinanceControl fc = e.Node.Tag as IFinanceControl;
			if( fc == null ) {
				return;
			}
			if( fc == activeControl ) {
				return;
			}
			UnloadFinanceControlEventArgs uea = new UnloadFinanceControlEventArgs();
			activeControl.UnloadControl( uea );
			e.Cancel = uea.Cancel;
		}
		#endregion
		#region private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		/// <summary>
		/// This method is called when the exitToolStripMenuItem's Click event has been fired.
		/// </summary>
		/// <param name="sender">The <see cref="object"/> that fired the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> of the event.</param>
		private void exitToolStripMenuItem_Click( object sender, EventArgs e ) {
			Close();
		}
		#endregion

	}
}
