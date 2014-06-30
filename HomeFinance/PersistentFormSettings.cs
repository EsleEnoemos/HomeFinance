using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace HomeFinance {
	public class PersistentFormSettings {
		#region private static DirectoryInfo AppDir
		/// <summary>
		/// Gets the AppDir of the Settings
		/// </summary>
		/// <value></value>
		private static DirectoryInfo AppDir {
			get {
				if( _appDir == null ) {
					_appDir = new DirectoryInfo( string.Format( "{0}\\HomeFinance", Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) ) );
					if( !Directory.Exists( _appDir.FullName ) ) {
						Directory.CreateDirectory( _appDir.FullName );
						_appDir.Refresh();
					}
				}
				return _appDir;
			}
		}
		private static DirectoryInfo _appDir;
		#endregion
		#region private static string Filename
		/// <summary>
		/// Gets the Filename of the Settings
		/// </summary>
		/// <value></value>
		private static string Filename {
			get {
				return _filename ?? (_filename = string.Format( "{0}\\PersistentFormSettings.config", AppDir.FullName ));
			}
		}
		private static string _filename;
		#endregion
		#region private static XmlSerializer Serializer
		/// <summary>
		/// Gets the Serializer of the Settings
		/// </summary>
		/// <value></value>
		private static XmlSerializer Serializer {
			get {
				return _serializer ?? (_serializer = new XmlSerializer( typeof(PersistentFormSettings) ));
			}
		}
		private static XmlSerializer _serializer;
		#endregion

		#region public string this[ string name ]
		/// <summary>
		/// Get/Sets the <see cref="System.String"/> item identified by the given arguments of the Settings
		/// </summary>
		/// <value></value>
		public string this[ string name ] {
			get {
				foreach( NameValue nv in Values ) {
					if( string.Equals( nv.Name, name ) ) {
						return nv.Value;
					}
				}
				return null;
			}
			set {
				foreach( NameValue nv in Values ) {
					if( string.Equals( nv.Name, name ) ) {
						nv.Value = value;
						return;
					}
				}
				Values.Add( new NameValue {
					Name = name, Value = value
				} );
			}
		}
		#endregion
		#region public List<NameValue> Values
		/// <summary>
		/// Get/Sets the Values of the Settings
		/// </summary>
		/// <value></value>
		public List<NameValue> Values {
			get {
				return _values ?? (_values = new List<NameValue>());
			}
			set {
				_values = value;
			}
		}
		private List<NameValue> _values;
		#endregion
		#region public int? WindowState
		/// <summary>
		/// Get/Sets the WindowState of the Settings
		/// </summary>
		/// <value></value>
		public int? WindowState {
			get {
				return _windowState;
			}
			set {
				_windowState = value;
			}
		}
		private int? _windowState;
		#endregion
		#region public int Width
		/// <summary>
		/// Get/Sets the Width of the Settings
		/// </summary>
		/// <value></value>
		public int Width {
			get {
				return _width;
			}
			set {
				_width = value;
			}
		}
		private int _width;
		#endregion
		#region public int Height
		/// <summary>
		/// Get/Sets the Height of the Settings
		/// </summary>
		/// <value></value>
		public int Height {
			get {
				return _height;
			}
			set {
				_height = value;
			}
		}
		private int _height;
		#endregion
		#region public int Top
		/// <summary>
		/// Get/Sets the Top of the Settings
		/// </summary>
		/// <value></value>
		public int Top {
			get {
				return _top;
			}
			set {
				_top = value;
			}
		}
		private int _top;
		#endregion
		#region public int Left
		/// <summary>
		/// Get/Sets the Left of the Settings
		/// </summary>
		/// <value></value>
		public int Left {
			get {
				return _left;
			}
			set {
				_left = value;
			}
		}
		private int _left;
		#endregion
		#region public int? SplitterDistance
		/// <summary>
		/// Get/Sets the SplitterDistance of the Settings
		/// </summary>
		/// <value></value>
		public int? SplitterDistance {
			get {
				return _splitterDistance;
			}
			set {
				_splitterDistance = value;
			}
		}
		private int? _splitterDistance;
		#endregion
		#region public bool SplitterCollapsed
		/// <summary>
		/// Get/Sets the SplitterCollapsed of the Settings
		/// </summary>
		/// <value></value>
		public bool SplitterCollapsed {
			get {
				return _splitterCollapsed;
			}
			set {
				_splitterCollapsed = value;
			}
		}
		private bool _splitterCollapsed;
		#endregion

		#region public string BillNumber
		/// <summary>
		/// Get/Sets the BillNumber of the PersistentFormSettings
		/// </summary>
		/// <value></value>
		public string BillNumber {
			get {
				return _billNumber;
			}
			set {
				_billNumber = value;
			}
		}
		private string _billNumber;
		#endregion
		#region public string BillSender
		/// <summary>
		/// Get/Sets the BillSender of the PersistentFormSettings
		/// </summary>
		/// <value></value>
		public string BillSender {
			get {
				return _billSender;
			}
			set {
				_billSender = value;
			}
		}
		private string _billSender;
		#endregion
		#region public string BillReceiver
		/// <summary>
		/// Get/Sets the BillReceiver of the PersistentFormSettings
		/// </summary>
		/// <value></value>
		public string BillReceiver {
			get {
				return _billReceiver;
			}
			set {
				_billReceiver = value;
			}
		}
		private string _billReceiver;
		#endregion
		#region public int ActiveBillIndex
		/// <summary>
		/// Get/Sets the ActiveBillIndex of the PersistentFormSettings
		/// </summary>
		/// <value></value>
		public int ActiveBillIndex {
			get {
				return _activeBillIndex;
			}
			set {
				_activeBillIndex = value;
			}
		}
		private int _activeBillIndex;
		#endregion
		#region public string SelectedControlName
		/// <summary>
		/// Get/Sets the SelectedControlName of the PersistentFormSettings
		/// </summary>
		/// <value></value>
		public string SelectedControlName {
			get {
				return _selectedControlName;
			}
			set {
				_selectedControlName = value;
			}
		}
		private string _selectedControlName;
		#endregion

		#region public PluginSettingsList PluginSettings
		/// <summary>
		/// Get/Sets the PluginSettings of the PersistentFormSettings
		/// </summary>
		/// <value></value>
		public PluginSettingsList PluginSettings {
			get {
				return _pluginSettings ?? (_pluginSettings = new PluginSettingsList());
			}
			set {
				_pluginSettings = value;
			}
		}
		private PluginSettingsList _pluginSettings;
		#endregion

		#region public void Save()
		/// <summary>
		/// 
		/// </summary>
		public void Save() {
			Stream s = null;
			try {
				if( File.Exists( Filename ) ) {
					File.Delete( Filename );
				}
				s = File.OpenWrite( Filename );
				Serializer.Serialize( s, this );
			} catch {
			} finally {
				if( s != null ) {
					s.Flush();
					s.Close();
					s.Dispose();
				}
			}
		}
		#endregion
		#region public static PersistentFormSettings Load()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static PersistentFormSettings Load() {
			if( !File.Exists( Filename ) ) {
				return new PersistentFormSettings();
			}
			Stream s = null;
			try {
				s = File.OpenRead( Filename );
				return (PersistentFormSettings)Serializer.Deserialize( s );
			} catch {
			} finally {
				if( s != null ) {
					s.Close();
					s.Dispose();
				}
			}
			return new PersistentFormSettings();
		}
		#endregion
	}
	public class NameValue {
		#region public string Name
		/// <summary>
		/// Get/Sets the Name of the NameValue
		/// </summary>
		/// <value></value>
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}
		private string _name;
		#endregion
		#region public string Value
		/// <summary>
		/// Get/Sets the Value of the NameValue
		/// </summary>
		/// <value></value>
		public string Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}
		private string _value;
		#endregion

		public NameValue() {
			
		}
		public NameValue( string name, string value ) {
			_name = name;
			_value = value;
		}
	}
	public class PluginSettingsItem {
		#region public string TypeName
		/// <summary>
		/// Get/Sets the TypeName of the PluginSettings
		/// </summary>
		/// <value></value>
		public string TypeName {
			get {
				return _typeName;
			}
			set {
				_typeName = value;
			}
		}
		private string _typeName;
		#endregion
		#region public NameValueList NameValues
		/// <summary>
		/// Get/Sets the NameValues of the PluginSettings
		/// </summary>
		/// <value></value>
		public NameValueList NameValues {
			get {
				return _nameValues ?? (_nameValues = new NameValueList());
			}
			set {
				_nameValues = value;
			}
		}
		private NameValueList _nameValues;
		#endregion
	}
	public class NameValueList : List<NameValue> {
		#region public string this[ string name ]
		/// <summary>
		/// Get/Sets the <see cref="String"/> item identified by the given arguments of the NameValueList
		/// </summary>
		/// <value></value>
		public string this[ string name ] {
			get {
				foreach( NameValue nv in this ) {
					if( string.Equals( nv.Name, name ) ) {
						return nv.Value;
					}
				}
				return null;
			}
			set {
				foreach( NameValue nv in this ) {
					if( string.Equals( nv.Name, name ) ) {
						nv.Value = value;
						return;
					}
				}
				NameValue n = new NameValue( name, value );
				Add( n );
			}
		}
		#endregion
		#region public void Set( string name, string value )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Set( string name, string value ) {
			foreach( NameValue nv in this ) {
				if( string.Equals( name, nv.Name ) ) {
					nv.Value = value;
					return;
				}
			}
			Add( new NameValue( name, value ) );
		}
		#endregion
	}
	public class PluginSettingsList : List<PluginSettingsItem> {
		//[XmlIgnore]
		//public NameValueList this[ object caller ] {
		//    get {
		//        return Get( caller );
		//    }
		//}
		public NameValueList Get( object caller ) {
			string tn = caller.GetType().FullName;
			foreach( PluginSettingsItem ps in this ) {
				if( string.Equals( ps.TypeName, tn ) ) {
					return ps.NameValues;
				}
			}
			PluginSettingsItem p = new PluginSettingsItem {
				TypeName = tn
			};
			Add( p );
			return p.NameValues;
		}
		public void Set( object caller, NameValueList values ) {
			string tn = caller.GetType().FullName;
			foreach( PluginSettingsItem ps in this ) {
				if( string.Equals( ps.TypeName, tn ) ) {
					ps.NameValues = values;
					return;
				}
			}
			Add( new PluginSettingsItem {
				TypeName = tn,
				NameValues = values
			} );
		}
	}

}
