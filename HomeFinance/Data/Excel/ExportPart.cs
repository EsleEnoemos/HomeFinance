using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace HomeFinance.Data.Excel {
	
	/// <summary>
	/// Contains all information about a sheet, or other part of data, for export
	/// This class is typically used together with the ExportList-method in <see cref="ExcelFactory"/>
	/// </summary>
	/// <remarks></remarks>
	/// <example></example>
	public class ExportPart {
		#region public const int MaxNumberOfExcelSheetRows
		/// <summary>
		/// Gets the MaxNumberOfExcelSheetRows of the <see cref="ExportPart"/>
		/// This is the maximum number of rows allowed in an Excel sheet.
		/// If more items than this is added to <see cref="Data"/>, the content will be splitted to additional sheets.
		/// This number includes the rows containing headers and additional headers.
		/// All headers will be repeated for each sheet
		/// </summary>
		public const int MaxNumberOfExcelSheetRows = 65536;
		#endregion
		#region private static readonly char[] InvalidSheetCharacters
		/// <summary>
		/// Gets the InvalidSheetCharacters of the <see cref="ExportPart"/>
		/// </summary>
		private static readonly char[] InvalidSheetCharacters = new[] { ':', '\\', '/', '?', '*', '[', ']' };
		#endregion
		#region public string SheetName
		/// <summary>
		/// Name of the sheet when exporting to Excel
		/// </summary>
		/// <value></value>
		public string SheetName {
			get { return _sheetName; }
			set {
				_sheetName = value;
				//if( !string.IsNullOrEmpty( _sheetName ) ) {
				//    if( _sheetName.IndexOfAny( InvalidSheetCharacters ) > -1 ) {
				//        foreach( char character in InvalidSheetCharacters ) {
				//            _sheetName = _sheetName.Replace( character, '_' );
				//        }
				//    }
				//    if( _sheetName.Length > 31 ) {
				//        _sheetName = _sheetName.Substring( 0, 28 ) + "...";
				//    }
				//}
			}
		}
		private string _sheetName;
		#endregion
		#region public bool AutoFitColumnWidths
		/// <summary>
		/// Get/Sets the AutoFitColumnWidths of the ExportPart
		/// </summary>
		/// <value></value>
		public bool AutoFitColumnWidths {
			get { return _autoFitColumnWidths; }
			set { _autoFitColumnWidths = value; }
		}
		private bool _autoFitColumnWidths;
		#endregion
		#region public CellDataList Headers
		/// <summary>
		/// The column headers
		/// These will by default use a bold font when exporing to Excel
		/// </summary>
		public CellDataList Headers = new CellDataList();
		#endregion
		#region public List<CellDataList> AdditionalHeaders
		/// <summary>
		/// Get/Sets the AdditionalHeaders of the <see cref="ExportPart"/>
		/// These will by default use a bold font when exporting to Excel
		/// </summary>
		public List<CellDataList> AdditionalHeaders = new List<CellDataList>();
		#endregion
		#region public List<CellDataList> Data
		/// <summary>
		/// The cell data
		/// </summary>
		public List<CellDataList> Data = new List<CellDataList>();
		#endregion
		#region public List<DataValidationList> DataValidationLists
		/// <summary>
		/// Get/Sets the DataValidationLists of the ExportPart
		/// </summary>
		/// <value></value>
		public List<DataValidationList> DataValidationLists {
			get {
				return _dataValidationLists ?? (_dataValidationLists = new List<DataValidationList>());
			}
			set {
				_dataValidationLists = value;
			}
		}
		private List<DataValidationList> _dataValidationLists;
		#endregion

		#region public int CountColorsUsed()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int CountColorsUsed() {
			int count = 0;
			Hashtable colorHash = new Hashtable();
			CountColors( Headers, ref count, colorHash );
			foreach( CellDataList list in AdditionalHeaders ) {
				CountColors( list, ref count, colorHash );
			}
			foreach( CellDataList list in Data ) {
				CountColors( list, ref count, colorHash );
			}
			return count;
		}
		#endregion
		#region private void CountColors( CellDataList list, ref int count, Hashtable colorHash )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <param name="count"></param>
		/// <param name="colorHash"></param>
		private void CountColors( CellDataList list, ref int count, Hashtable colorHash ) {
			foreach( CellData value in list ) {
				if( value.BackgroundColor != Color.Transparent ) {
					if( !colorHash.ContainsKey( value.BackgroundColor.ToArgb() ) ) {
						colorHash[ value.BackgroundColor.ToArgb() ] = null;
						count++;
					}
				}
				if( value.FontColor != SystemColors.WindowText ) {
					if( !colorHash.ContainsKey( value.FontColor.ToArgb() ) ) {
						colorHash[ value.FontColor.ToArgb() ] = null;
						count++;
					}
				}
				if( value.PatternColor != Color.Empty ) {
					if( !colorHash.ContainsKey( value.PatternColor.ToArgb() ) ) {
						colorHash[ value.PatternColor.ToArgb() ] = null;
						count++;
					}
				}
				if( value.LeftBorderColor != Color.Transparent ) {
					if( !colorHash.ContainsKey( value.LeftBorderColor.ToArgb() ) ) {
						colorHash[ value.LeftBorderColor.ToArgb() ] = null;
						count++;
					}
				}
				if( value.RightBorderColor != Color.Transparent ) {
					if( !colorHash.ContainsKey( value.RightBorderColor.ToArgb() ) ) {
						colorHash[ value.RightBorderColor.ToArgb() ] = null;
						count++;
					}
				}
				if( value.TopBorderColor != Color.Transparent ) {
					if( !colorHash.ContainsKey( value.TopBorderColor.ToArgb() ) ) {
						colorHash[ value.TopBorderColor.ToArgb() ] = null;
						count++;
					}
				}
				if( value.BottomBorderColor != Color.Transparent ) {
					if( !colorHash.ContainsKey( value.BottomBorderColor.ToArgb() ) ) {
						colorHash[ value.BottomBorderColor.ToArgb() ] = null;
						count++;
					}
				}
			}
		}
		#endregion
		public static string FixSheetName( string sheetName ) {
			if( !string.IsNullOrEmpty( sheetName ) ) {
				if( sheetName.IndexOfAny( InvalidSheetCharacters ) > -1 ) {
					foreach( char character in InvalidSheetCharacters ) {
						sheetName = sheetName.Replace( character, '_' );
					}
				}
				if( sheetName.Length > 31 ) {
					sheetName = sheetName.Substring( 0, 28 ) + "...";
				}
			}
			return sheetName;
		}

		public class DataValidationList {
			#region public List<int> Columns
			/// <summary>
			/// Get/Sets the Columns of the DataValidationList
			/// </summary>
			/// <value></value>
			public List<int> Columns {
				get {
					return _columns ?? (_columns = new List<int>());
				}
				set {
					_columns = value;
				}
			}
			private List<int> _columns;
			#endregion
			#region public string Name
			/// <summary>
			/// Get/Sets the Name of the DataValidationList
			/// </summary>
			/// <value></value>
			public string Name {
				get {
					return _name;
				}
				set {
					string tmp = value;
					if( !string.IsNullOrEmpty( tmp ) ) {
						if( tmp.StartsWithNumber() ) {
							tmp = "_" + tmp;
						}
						if( tmp.Contains( " " ) ) {
							tmp = tmp.Replace( " ", "_" );
						}
						if( tmp.Contains( "," ) ) {
							tmp = tmp.Replace( ",", "." );
						}
					}
					_name = tmp;
				}
			}
			private string _name;
			#endregion
			#region public DataValidationValueList Values
			/// <summary>
			/// Get/Sets the Values of the DataValidationList
			/// </summary>
			/// <value></value>
			public DataValidationValueList Values {
				get {
					return _values ?? (_values = new DataValidationValueList());
				}
				set {
					_values = value;
				}
			}
			private DataValidationValueList _values;
			#endregion

			public class DataValidationValue {
				#region public string Value
				/// <summary>
				/// Get/Sets the Value of the DataValidationValue
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
				#region public string DisplayValue
				/// <summary>
				/// Get/Sets the DisplayValue of the DataValidationValue
				/// </summary>
				/// <value></value>
				public string DisplayValue {
					get {
						return _displayValue;
					}
					set {
						_displayValue = value;
					}
				}
				private string _displayValue;
				#endregion

				#region public DataValidationValue()
				/// <summary>
				/// Initializes a new instance of the <b>DataValidationValue</b> class.
				/// </summary>
				public DataValidationValue() {
				}
				#endregion
				#region public DataValidationValue( string value, string displayValue )
				/// <summary>
				/// Initializes a new instance of the <b>DataValidationValue</b> class.
				/// </summary>
				/// <param name="value"></param>
				/// <param name="displayValue"></param>
				public DataValidationValue( string value, string displayValue ) {
					_value = value;
					_displayValue = displayValue;
				}
				#endregion
			}
			public class DataValidationValueList : List<DataValidationValue> {
				public DataValidationValueList() {
				}
				public DataValidationValueList(IEnumerable<DataValidationValue> items) {
					if( items != null ) {
						foreach( DataValidationValue item in items ) {
							Add( item );
						}
					}
				}
				#region public void Add( string value, string displayValue )
				/// <summary>
				/// 
				/// </summary>
				/// <param name="value"></param>
				/// <param name="displayValue"></param>
				public void Add( string value, string displayValue ) {
					Add( new DataValidationValue( value, displayValue ) );
				}
				#endregion
			}
		}
	}
}