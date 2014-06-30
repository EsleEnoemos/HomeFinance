using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using SpreadsheetGear;

namespace HomeFinance.Data.Excel {
	/// <summary>
	/// This class is used to parse Excel-files.
	/// An instance of this object can be used in the same way as a parsed tab-separated text-file, but there are also methods available to get typed data from columns, as well as iterating through it 
	/// in the same way as a <see cref="SqlDataReader"/>
	/// </summary>
	/// <remarks></remarks>
	/// <example></example>
	public class ExcelParsedFile {
		private Hashtable ordinalHash = new Hashtable();
		private int currentIndex = -1;
		#region public string SheetName
		/// <summary>
		/// Gets the SheetName of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public string SheetName {
			get { return _sheetName; }
		}
		private string _sheetName;
		#endregion
		#region public string this[ int index ]
		/// <summary>
		/// Returns the string on the current row, indexed with the current index
		/// </summary>
		/// <value></value>
		public string this[ int index ] {
			get {
				return GetString( index );
			}
		}
		#endregion
		#region public string this[ string columnName ]
		/// <summary>
		/// Returns the string on the current row, from the column with the specified column name
		/// </summary>
		/// <value></value>
		public string this[ string columnName ] {
			get {
				return GetString( columnName );
			}
		}
		#endregion
		#region public int CurrentIndex
		/// <summary>
		/// Gets the CurrentIndex of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public int CurrentIndex {
			get {
				return currentIndex;
			}
		}
		#endregion
		#region public List<string> ColumnNames
		/// <summary>
		/// Gets the ColumnNames of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public List<string> ColumnNames {
			get { return _columnNames; }
		}
		private List<string> _columnNames;
		#endregion
		#region public List<FileRow> Content
		/// <summary>
		/// Gets the Content of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public List<FileRow> Content {
			get { return _content; }
		}
		private List<FileRow> _content;
		#endregion

		#region public int ColumnCount
		/// <summary>
		/// Gets the ColumnCount of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public readonly int ColumnCount;
		#endregion
		#region public int RowCount
		/// <summary>
		/// Gets the RowCount of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public readonly int RowCount;
		#endregion
		#region public FileRow CurrentRow
		/// <summary>
		/// Gets the CurrentRow of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public FileRow CurrentRow {
			get {
				if( currentIndex < 0 ) {
					throw new IndexOutOfRangeException( "Can not access current row on an unitilialized object" );
				}
				if( currentIndex >= RowCount ) {
					throw new IndexOutOfRangeException( "End of file has been reached" );
				}
				return Content[ currentIndex ];
			}
		}
		#endregion
		#region public int CurrentExcelRowIndex
		/// <summary>
		/// Gets the CurrentExcelRowIndex of the ExcelParsedFile
		/// </summary>
		/// <value></value>
		public int CurrentExcelRowIndex {
			get {
				return CurrentRow.ExcelRowIndex;
			}
		}
		#endregion

		#region public void Reset()
		/// <summary>
		/// 
		/// </summary>
		public void Reset() {
			currentIndex = -1;
		}
		#endregion
		#region public bool MoveNext()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool MoveNext() {
			return (++currentIndex) < Content.Count;
		}
		#endregion
		#region public string GetString( int index )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetString( int index ) {
			return Content[ currentIndex ][ index ];
		}
		#endregion
		#region public string GetString( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public string GetString( string columnName ) {
			return GetString( GetOrdinal( columnName ) );
		}
		#endregion
		#region public string GetString( string columnName, string otherColumnName )
		/// <summary>
		/// Use this method if a column can have two different names
		/// </summary>
		/// <param name="columnName">Name of the column</param>
		/// <param name="otherColumnName">If no column is found with the first name, this name is used instead</param>
		/// <returns></returns>
		public string GetString( string columnName, string otherColumnName ) {
			return ContainsColumn( columnName ) ? GetString( GetOrdinal( columnName ) ) : GetString( GetOrdinal( otherColumnName ) );
		}
		#endregion

		#region public int GetOrdinal( string fieldName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public int GetOrdinal( string fieldName ) {
			if( string.IsNullOrEmpty( fieldName ) ) {
				throw new InvalidColumnNameException( "Column names can not be null or empty" );
			}
			try {
				fieldName = fieldName.ToLower();
				if( !ordinalHash.ContainsKey( fieldName ) ) {
					throw new InvalidColumnNameException( "No column with name " + fieldName + " was found" );
				}
				return (int)ordinalHash[ fieldName ];
			} catch( InvalidColumnNameException ex ) {
				throw new InvalidColumnNameException( "No column with name " + fieldName + " was found", ex );
			}
		}
		#endregion
		#region public int GetInt( int index )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int GetInt( int index ) {
			return GetInt( index, 0 );
		}
		#endregion
		#region public int GetInt( int index, int defaultValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public int GetInt( int index, int defaultValue ) {
			return GetString( index ).ToInt( defaultValue );
		}
		#endregion
		#region public int GetInt( string fieldName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public int GetInt( string fieldName ) {
			return GetInt( fieldName, 0 );
		}
		#endregion
		#region public int GetInt( string fieldName, int defaultValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public int GetInt( string fieldName, int defaultValue ) {
			return GetInt( GetOrdinal( fieldName ), defaultValue );
		}
		#endregion
		#region public double GetDouble( int index )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public double GetDouble( int index ) {
			return GetDouble( index, 0 );
		}
		#endregion
		#region public double GetDouble( int index, double defaultValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public double GetDouble( int index, double defaultValue ) {
			return GetString( index ).ToDouble( defaultValue );
		}
		#endregion
		#region public double GetDouble( string fieldName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public double GetDouble( string fieldName ) {
			return GetDouble( fieldName, 0 );
		}
		#endregion
		#region public double GetDouble( string fieldName, double defaultValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public double GetDouble( string fieldName, double defaultValue ) {
			return GetDouble( GetOrdinal( fieldName ), defaultValue );
		}
		#endregion
		#region public DateTime? GetNullableDateTime( string fieldName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		public DateTime? GetNullableDateTime( string fieldName ) {
			return GetNullableDateTime( GetOrdinal( fieldName ) );
		}
		#endregion
		#region public DateTime? GetNullableDateTime( int index )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public DateTime? GetNullableDateTime( int index ) {
			string str = GetString( index );
			if( string.IsNullOrEmpty( str ) ) {
				return null;
			}
			DateTime d;
			if( DateTime.TryParse( str, out d ) ) {
				return d;
			}
			return null;
		}
		#endregion
		#region public DateTime GetDateTime( int index )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public DateTime GetDateTime( int index ) {
			string str = GetString( index );
			DateTime d;
			DateTime.TryParse( str, out d );
			return d;
		}
		#endregion
		#region public bool IsEmpty( string fieldName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns>True if it is empty, otherwise false.</returns>
		public bool IsEmpty( string fieldName ) {
			return IsEmpty( GetOrdinal( fieldName ) );
		}
		#endregion
		#region public bool IsEmpty( string fieldName, string otherColumnName )
		/// <summary>
		/// Use this method if a column can have two different names
		/// </summary>
		/// <param name="fieldName">Name of the column</param>
		/// <param name="otherColumnName">If no column is found with the first name, this name is used instead</param>
		/// <returns>True if it is empty, otherwise false.</returns>
		public bool IsEmpty( string fieldName, string otherColumnName ) {
			return ContainsColumn( fieldName ) ? IsEmpty( GetOrdinal( fieldName ) ) : IsEmpty( GetOrdinal( otherColumnName ) );
		}
		#endregion

		#region public bool IsEmpty( int index )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns>True if it is empty, otherwise false.</returns>
		public bool IsEmpty( int index ) {
			string s = GetString( index );
			if( s != null ) {
				s = s.Trim();
			}
			return string.IsNullOrEmpty( s );
		}
		#endregion
		#region public bool CurrentLineLineIsEmpty()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool CurrentLineLineIsEmpty() {
			return CurrentLineLineIsEmpty( 0 );
		}
		#endregion
		#region public bool CurrentLineLineIsEmpty( int fromColumnIndex )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fromColumnIndex"></param>
		/// <returns></returns>
		public bool CurrentLineLineIsEmpty( int fromColumnIndex ) {
			for( int i = fromColumnIndex; i < ColumnCount; i++ ) {
				if( !string.IsNullOrEmpty( this[ i ] ) ) {
					return false;
				}
			}
			return true;
		}
		#endregion
		#region public bool CurrentLineLineIsEmpty( string startColumn )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="startColumn"></param>
		/// <returns></returns>
		public bool CurrentLineLineIsEmpty( string startColumn ) {
			return CurrentLineLineIsEmpty( GetOrdinal( startColumn ) );
		}
		#endregion

		#region public bool ContainsColumn( string columnName )
		/// <summary>
		/// Returns a value indicating whether the specified <see cref="String"/>
		///  is contained in the <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile"/>.
		/// </summary>
		/// <param name="columnName">The <see cref="String"/> to locate in the 
		/// <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile"/>.</param>
		/// <returns><b>true</b> if the <i>String</i> parameter is a member 
		/// of the <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile"/>; otherwise, <b>false</b>.</returns>
		public bool ContainsColumn( string columnName ) {
			if( string.IsNullOrEmpty( columnName ) ) {
				return false;
			}
			columnName = columnName.ToLower();
			return ordinalHash.ContainsKey( columnName );
		}
		#endregion


		#region private ExcelParsedFile( string filename, int sheetNumber, bool allowEmpty )
		/// <summary>
		/// Initializes a new instance of the <b>ExcelParsedFile</b> class.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="sheetNumber"></param>
		/// <param name="allowEmpty"></param>
		/// <exception cref="DuplicateNameException">If multiple columns found with name \.</exception>
		/// <exception cref="InvalidDataException">If no columns was found in the specified file. The first row of the file must contain column names!
		///  or if no content was found in the specified file.</exception>
		private ExcelParsedFile( string filename, int sheetNumber, bool allowEmpty ) {
			IWorkbook b = Factory.GetWorkbook( filename );
			IWorksheet sheet = b.Worksheets[ sheetNumber ];
			_sheetName = sheet.Name;
			int r = 0;
			int c = 0;
			_columnNames = new List<string>();
			while( !string.IsNullOrEmpty( sheet.Cells[ r, c ].Text ) ) {
				string key = sheet.Cells[ r, c ].Text.ToLower();
				if( ordinalHash.ContainsKey( key ) ) {
					throw new DuplicateNameException( "Multiple columns found with name \"" + sheet.Cells[ r, c ].Text + "\"" );
				}
				ordinalHash.Add( key, c );
				_columnNames.Add( sheet.Cells[ r, c ].Text );
				c++;
			}
			ColumnCount = _columnNames.Count;
			if( ColumnCount == 0 && !allowEmpty ) {
				throw new InvalidDataException( "No columns was found in the specified file. The first row of the file must contain column names!" );
			}
			_content = new List<FileRow>();
			if( ColumnCount > 0 ) {
				r++;
				while(!IsRowEmpty( sheet, r, ColumnCount )) {
					FileRow data = new FileRow( this, r + 1 ); // r + 1, because this property is used to display the number of row in Excel, not the index
					_content.Add( data );
					for(int i = 0; i < ColumnCount; i++) {
						/// 2009-03-18, Jakob Adolfsson
						/// Changed from sheet.Cells[ r, i ].Text to sheet.Cells[ r, i ].Entry, since the Text-property only returns the value displayed in Excel, not the actual value entered.
						/// The downside of this is that a formula will not be evaluated, but will be returned as the entered string value.
						/// 
						/// 2009-06-08, Jakob Adolfsson
						/// Added a check if the entry starts with an equals sign.
						/// If so, the Text-property is read to get the evaluated forumula
						string entry = sheet.Cells[ r, i ].Entry;
						if(!string.IsNullOrEmpty( entry ) && entry.StartsWith( "=" )) {
							entry = sheet.Cells[ r, i ].Text;
						}
						data.Add( entry );
					}
					r++;
				}
			}
			RowCount = _content.Count;
			if( RowCount == 0 && !allowEmpty ) {
				throw new InvalidDataException( "No content was found in the specified file." );
			}
		}
		#endregion

		#region public static ExcelParsedFile Load( string filename, int sheetNumber )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="sheetNumber"></param>
		/// <returns></returns>
		public static ExcelParsedFile Load( string filename, int sheetNumber ) {
			return new ExcelParsedFile( filename, sheetNumber, false );
		}
		#endregion
		#region public static List<ExcelParsedFile> Load( string filename, params int[] sheetNumbers )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="sheetNumbers"></param>
		/// <returns></returns>
		public static List<ExcelParsedFile> Load( string filename, params int[] sheetNumbers ) {
			List<ExcelParsedFile> list = new List<ExcelParsedFile>( sheetNumbers.Length );
			foreach( int sheetNumber in sheetNumbers ) {
				list.Add( new ExcelParsedFile( filename, sheetNumber, true ) );
			}
			return list;
		}
		#endregion
		#region public static List<ExcelParsedFile> LoadAllSheets( string filename )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static List<ExcelParsedFile> LoadAllSheets( string filename ) {
			return LoadAllSheets( filename, 0 );
		}
		#endregion
		#region public static List<ExcelParsedFile> LoadAllSheets( string filename, int startSheet )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="startSheet"></param>
		/// <returns></returns>
		public static List<ExcelParsedFile> LoadAllSheets( string filename, int startSheet ) {
			IWorkbook b = Factory.GetWorkbook( filename );
			int sheetCount = b.Worksheets.Count;
			b.Close();
			List<ExcelParsedFile> list = new List<ExcelParsedFile>( sheetCount );
			for( int i = startSheet; i < sheetCount; i++ ) {
				list.Add( new ExcelParsedFile( filename, i, true ) );
			}
			return list;
		}
		#endregion
		#region private bool IsRowEmpty( IWorksheet sheet, int row, int colCount )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="row"></param>
		/// <param name="colCount"></param>
		/// <returns>True if row is empty, otherwise false.</returns>
		private bool IsRowEmpty( IWorksheet sheet, int row, int colCount ) {
			for( int i = 0; i < colCount; i++ ) {
				string t = sheet.Cells[ row, i ].Text;
				if( t != null ) {
					t = t.Trim();
				}
				if( !string.IsNullOrEmpty( t ) ) {
					return false;
				}
			}
			return true;
		}
		#endregion

		#region public string CurrentRowToString( string preSign, string separator, string postSign )
		/// <summary>
		/// Returns a <see cref="string"/> that represents the current row is data
		/// </summary>
		/// <param name="preSign"></param>
		/// <param name="separator"></param>
		/// <param name="postSign"></param>
		/// <returns></returns>
		public string CurrentRowToString( string preSign, string separator, string postSign ) {
			StringBuilder sb = new StringBuilder();
			if( preSign != null ) {
				sb.Append( preSign );
			}
			int colCount = ColumnCount;
			for( int i = 0; i < colCount; i++ ) {
				string s = Content[ currentIndex ][ i ];
				sb.Append( s );
				if( (i + 1) < colCount ) {
					sb.Append( separator );
				}
			}
			if( postSign != null ) {
				sb.Append( postSign );
			}
			return sb.ToString();
		}
		#endregion
		#region public string CurrentRowToString( string separator )
		/// <summary>
		/// Returns a <see cref="string"/> that represents the current row of data
		/// </summary>
		/// <param name="separator"></param>
		/// <returns></returns>
		public string CurrentRowToString( string separator ) {
			return CurrentRowToString( null, separator, null );
		}
		#endregion

		/// <summary>
		/// Represents one row of data in this Excel-file
		/// </summary>
		/// <remarks></remarks>
		/// <example></example>
		public class FileRow : List<string> {
			#region public readonly ExcelParsedFile File
			/// <summary>
			/// Gets the File of the <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile.FileRow"/>
			/// </summary>
			public readonly ExcelParsedFile File;
			#endregion
			#region public readonly int ExcelRowIndex
			/// <summary>
			/// Gets the RowIndex of the <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile.FileRow"/>
			/// </summary>
			public readonly int ExcelRowIndex;
			#endregion

			#region public FileRow( ExcelParsedFile file, int rowIndex )
			/// <summary>
			/// Initializes a new instance of the <b>FileRow</b> class.
			/// </summary>
			/// <param name="file"></param>
			/// <param name="rowIndex"></param>
			public FileRow( ExcelParsedFile file, int rowIndex ) {
				File = file;
				ExcelRowIndex = rowIndex;
			}
			#endregion

			#region public string GetString( int index )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public string GetString( int index ) {
				return this[ index ];
			}
			#endregion	
			#region public string GetString( string columnName )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="columnName"></param>
			/// <returns></returns>
			public string GetString( string columnName ) {
				return GetString( File.GetOrdinal( columnName ) );
			}
			#endregion
			#region public int GetInt( int index )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public int GetInt( int index ) {
				return GetInt( index, 0 );
			}
			#endregion
			#region public int GetInt( int index, int defaultValue )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <param name="defaultValue"></param>
			/// <returns></returns>
			public int GetInt( int index, int defaultValue ) {
				return GetString( index ).ToInt( defaultValue );
			}
			#endregion
			#region public int GetInt( string fieldName )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fieldName"></param>
			/// <returns></returns>
			public int GetInt( string fieldName ) {
				return GetInt( fieldName, 0 );
			}
			#endregion
			#region public int GetInt( string fieldName, int defaultValue )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fieldName"></param>
			/// <param name="defaultValue"></param>
			/// <returns></returns>
			public int GetInt( string fieldName, int defaultValue ) {
				return GetString( fieldName ).ToInt( defaultValue );
			}
			#endregion
			#region public double GetDouble( int index )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public double GetDouble( int index ) {
				return GetDouble( index, 0 );
			}
			#endregion
			#region public double GetDouble( int index, double defaultValue )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <param name="defaultValue"></param>
			/// <returns></returns>
			public double GetDouble( int index, double defaultValue ) {
				return GetString( index ).ToDouble( defaultValue );
			}
			#endregion
			#region public double GetDouble( string fieldName )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fieldName"></param>
			/// <returns></returns>
			public double GetDouble( string fieldName ) {
				return GetDouble( fieldName, 0 );
			}
			#endregion
			#region public double GetDouble( string fieldName, double defaultValue )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fieldName"></param>
			/// <param name="defaultValue"></param>
			/// <returns></returns>
			public double GetDouble( string fieldName, double defaultValue ) {
				return GetString( fieldName ).ToDouble( defaultValue );
			}
			#endregion
			public DateTime GetDateTime( int index ) {
				DateTime d;
				DateTime.TryParse( GetString( index ), out d );
				return d;
			}
			#region public bool IsEmpty( string fieldName )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fieldName"></param>
			/// <returns>True if it is empty, otherwise false.</returns>
			public bool IsEmpty( string fieldName ) {
				return IsEmpty( File.GetOrdinal( fieldName ) );
			}
			#endregion
			#region public bool IsEmpty( int index )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="index"></param>
			/// <returns>True if it is empty, otherwise false.</returns>
			public bool IsEmpty( int index ) {
				string s = GetString( index );
				if( s != null ) {
					s = s.Trim();
				}
				return string.IsNullOrEmpty( s );
			}
			#endregion
			#region public bool LineIsEmpty()
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public bool LineIsEmpty() {
				return LineIsEmpty( 0 );
			}
			#endregion
			#region public bool LineIsEmpty( int fromColumnIndex )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fromColumnIndex"></param>
			/// <returns></returns>
			public bool LineIsEmpty( int fromColumnIndex ) {
				for( int i = fromColumnIndex; i < File.ColumnCount; i++ ) {
					if( !string.IsNullOrEmpty( this[ i ] ) ) {
						return false;
					}
				}
				return true;
			}
			#endregion
			#region public bool LineIsEmpty( string startColumn )
			/// <summary>
			/// 
			/// </summary>
			/// <param name="startColumn"></param>
			/// <returns></returns>
			public bool LineIsEmpty( string startColumn ) {
				return LineIsEmpty( File.GetOrdinal( startColumn ) );
			}
			#endregion

			#region public string ToString( string separator )
			/// <summary>
			/// Returns a <see cref="string"/> that represents the current <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile.FileRow"/>.
			/// </summary>
			/// <param name="separator"></param>
			/// <returns>A <see cref="string"/> that represents the current <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile.FileRow"/>.</returns>
			public string ToString( string separator ) {
				StringBuilder sb = new StringBuilder();
				int colCount = Count;
				for( int i = 0; i < colCount; i++ ) {
					string s = this[ i ];
					sb.Append( s );
					if( (i + 1) < colCount ) {
						sb.Append( separator );
					}
				}
				return sb.ToString();

			}
			#endregion
		}
		#region public bool ContainsValueSet( params string[] valuesInGivenOrder )
		/// <summary>
		/// Returns a value indicating whether the specified <see cref="String"/> array
		///  is contained in the <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile"/>.
		/// </summary>
		/// <param name="valuesInGivenOrder">The <see cref="String"/> array to locate in the 
		/// <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile"/>.</param>
		/// <returns><b>true</b> if the <i>String[]</i> parameter is a member 
		/// of the <see cref="WaterCharacteristics.Classes.Data.Util.ExcelParsedFile"/>; otherwise, <b>false</b>.</returns>
		public bool ContainsValueSet( params string[] valuesInGivenOrder ) {
			if( ColumnCount < valuesInGivenOrder.Length ) {
				return false;
			}
			foreach( FileRow r in Content ) {
				bool allSame = true;
				for( int i = 0; i < valuesInGivenOrder.Length; i++ ) {
					if( !string.Equals( r[ i ], valuesInGivenOrder[ i ] ) ) {
						allSame = false;
						break;
					}
				}
				if( allSame ) {
					return true;
				}
			}
			return false;
		}
		#endregion
	}

	public class InvalidColumnNameException : Exception {
		public InvalidColumnNameException( string message )
			: base( message ) {

		}
		public InvalidColumnNameException( string message, Exception innerException )
			: base( message, innerException ) {

		}
	}
}
