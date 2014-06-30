using System;
using System.Collections.Generic;
using System.Text;

namespace HomeFinance.Data.Excel {
	/// <summary>
	/// Class containing a list of <see cref="CellData"/> with easy to use Add-methods
	/// </summary>
	/// <remarks></remarks>
	/// <example></example>
	public class CellDataList : List<CellData> {
		#region public CellDataList()
		/// <summary>
		/// Initializes a new instance of the <b>CellDataList</b> class.
		/// </summary>
		public CellDataList() {
		}
		#endregion
		#region public CellDataList( IEnumerable<CellData> cells )
		/// <summary>
		/// Initializes a new instance of the <b>CellDataList</b> class.
		/// </summary>
		/// <param name="cells"></param>
		public CellDataList( IEnumerable<CellData> cells )
			: base( cells ) {

		}
		#endregion
		#region public CellDataList( IEnumerable<string> cellData )
		/// <summary>
		/// Initializes a new instance of the <b>CellDataList</b> class.
		/// </summary>
		/// <param name="cellData"></param>
		public CellDataList( IEnumerable<string> cellData )
			: this( cellData, false ) {

		}
		#endregion
		#region public CellDataList( IEnumerable<string> cellData, bool textFormat )
		/// <summary>
		/// Initializes a new instance of the <b>CellDataList</b> class.
		/// </summary>
		/// <param name="cellData"></param>
		/// <param name="textFormat"></param>
		public CellDataList( IEnumerable<string> cellData, bool textFormat ) {
			foreach( string s in cellData ) {
				Add( s, textFormat );
			}
		}
		#endregion

		#region public CellData Add( int data )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public CellData Add( int data ) {
			return Add( data.ToString() );
		}
		#endregion
		#region public CellData Add( double data )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public CellData Add( double data ) {
			return Add( data.ToString() );
		}
		#endregion
		#region public CellData Add( double data, bool textFormat )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="textFormat"></param>
		/// <returns></returns>
		public CellData Add( double data, bool textFormat ) {
			return Add( data.ToString(), textFormat );
		}
		#endregion
		#region public CellData Add( double? data )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public CellData Add( double? data ) {
			return Add( data.HasValue ? data.Value.ToString() : "" );
		}
		#endregion
		#region public CellData Add( double? data, bool textFormat )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="textFormat"></param>
		/// <returns></returns>
		public CellData Add( double? data, bool textFormat ) {
			return Add( data.HasValue ? data.Value.ToString() : "", textFormat );
		}
		#endregion
		#region public CellData Add( string data )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public CellData Add( string data ) {
			CellData cd = new CellData( data );
			Add( cd );
			return cd;
		}
		#endregion
		#region public CellData Add( DateTime? date )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public CellData Add( DateTime? date ) {
			CellData cd = (date == null) ? new CellData( "" ) : new CellData( date.Format() );
			Add( cd );
			return cd;
		}
		#endregion
		#region public CellData Add( DateTime date )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public CellData Add( DateTime date ) {
			CellData cd = new CellData( date.Format() );
			Add( cd );
			return cd;
		}
		#endregion
		#region public CellData Add( string data, int colSpan )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="colSpan"></param>
		/// <returns></returns>
		public CellData Add( string data, int colSpan ) {
			CellData cd = new CellData( data, colSpan );
			Add( cd );
			return cd;
		}
		#endregion
		#region public CellData Add( string data, bool textFormat )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="textFormat">Set to false to let excel format the content of the cell. Default value of this is true.</param>
		/// <returns></returns>
		public CellData Add( string data, bool textFormat ) {
			CellData cd = new CellData( data, 1, 0, textFormat );
			Add( cd );
			return cd;
		}
		#endregion

		#region public string ToString( string separator )
		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="CellDataList"/>.
		/// </summary>
		/// <param name="separator"></param>
		/// <returns>A <see cref="string"/> that represents the current <see cref="CellDataList"/>.</returns>
		public string ToString( string separator ) {
			StringBuilder sb = new StringBuilder();
			for( int i = 0; i < Count; i++ ) {
				CellData cd = this[ i ];
				sb.Append( cd.Value );
				if( (i + 1) < Count ) {
					sb.Append( separator );
				}
			}
			return sb.ToString();
		}
		#endregion
	}
}
