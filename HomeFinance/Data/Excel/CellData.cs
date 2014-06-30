using System;
using System.Drawing;
using SpreadsheetGear;

namespace HomeFinance.Data.Excel {
	/// <summary>
	/// </summary>
	/// <remarks></remarks>
	/// <example></example>
	public class CellData : ICloneable  {
		#region public string Value
		/// <summary>
		/// Get/Sets the Value of the <see cref="CellData"/>
		/// </summary>
		public string Value;
		#endregion
		#region public int ColSpan
		/// <summary>
		/// Get/Sets the ColSpan of the <see cref="CellData"/>
		/// </summary>
		public int ColSpan;
		#endregion
		#region public int RowSpan
		/// <summary>
		/// Get/Sets the RowSpan of the <see cref="CellData"/>
		/// </summary>
		public int RowSpan;
		#endregion
		#region public int TextDirectionDegrees
		/// <summary>
		/// Get/Sets the TextDirectionDegrees of the <see cref="CellData"/>
		/// </summary>
		public int TextDirectionDegrees;
		#endregion
		#region public bool TextFormat
		/// <summary>
		/// If true, cell in the &quot;NumberFormat&quot; of the cell in the Excel-sheet will be set to @ (general text)
		/// The default value is true
		/// </summary>
		public bool TextFormat = true;
		#endregion
		#region public string NumberFormat
		/// <summary>
		/// Get/Sets the NumberFormat of the <see cref="CellData"/>
		/// </summary>
		public string NumberFormat = "";
		#endregion
		#region public System.Drawing.Color BackgroundColor
		/// <summary>
		/// Get/Sets the BackgroundColor of the <see cref="CellData"/>
		/// </summary>
		public Color BackgroundColor = Color.Transparent;
		#endregion
		#region public System.Drawing.Color FontColor
		/// <summary>
		/// Get/Sets the FontColor of the <see cref="CellData"/>
		/// </summary>
		public Color FontColor = SystemColors.WindowText;
		#endregion
		#region public bool Bold
		/// <summary>
		/// Get/Sets the Bold of the <see cref="CellData"/>
		/// </summary>
		public bool Bold = false;
		#endregion
		#region public bool? Underline
		/// <summary>
		/// Get/Sets the Underline of the <see cref="CellData"/>
		/// </summary>
		public bool? Underline = null;
		#endregion
		#region public bool Pattern
		/// <summary>
		/// Get/Sets the Pattern of the <see cref="CellData"/>. This is the pattern of the 
		/// cell background.
		/// </summary>
		public bool Pattern = false;
		#endregion
		#region public Color PatternColor
		/// <summary>
		/// Get/Sets the PatternColor of the <see cref="CellData"/>
		/// </summary>
		public Color PatternColor = Color.Empty;
		#endregion
		#region public bool LeftBorder
		/// <summary>
		/// Get/Sets the LeftBorder of the <see cref="CellData"/>
		/// </summary>
		public bool LeftBorder;
		#endregion
		#region public Color LeftBorderColor
		/// <summary>
		/// Get/Sets the LeftBorderColor of the <see cref="CellData"/>
		/// </summary>
		public Color LeftBorderColor = Color.Transparent;
		#endregion
		#region public bool RightBorder
		/// <summary>
		/// Get/Sets the RightBorder of the <see cref="CellData"/>
		/// </summary>
		public bool RightBorder;
		#endregion
		#region public Color RightBorderColor
		/// <summary>
		/// Get/Sets the RightBorderColor of the <see cref="CellData"/>
		/// </summary>
		public Color RightBorderColor = Color.Transparent;
		#endregion
		#region public bool TopBorder
		/// <summary>
		/// Get/Sets the TopBorder of the <see cref="CellData"/>
		/// </summary>
		public bool TopBorder;
		#endregion
		#region public Color TopBorderColor
		/// <summary>
		/// Get/Sets the TopBorderColor of the <see cref="CellData"/>
		/// </summary>
		public Color TopBorderColor = Color.Transparent;
		#endregion
		#region public bool BottomBorder
		/// <summary>
		/// Get/Sets the BottomBorder of the <see cref="CellData"/>
		/// </summary>
		public bool BottomBorder;
		#endregion
		#region public Color BottomBorderColor
		/// <summary>
		/// Get/Sets the BottomBorderColor of the <see cref="CellData"/>
		/// </summary>
		public Color BottomBorderColor = Color.Transparent;
		#endregion
		#region public double ColumnWidth
		/// <summary>
		/// Get/Sets the ColumnWidth of the <see cref="CellData"/>
		/// </summary>
		public double ColumnWidth = 0;
		#endregion
		#region public double RowHeight
		/// <summary>
		/// Get/Sets the RowHeight of the <see cref="CellData"/>
		/// </summary>
		public double RowHeight = 0;
		#endregion
		#region public string URL
		/// <summary>
		/// Get/Sets the URL of the <see cref="CellData"/>
		/// </summary>
		public string URL;
		#endregion
		#region public string URLTip
		/// <summary>
		/// Get/Sets the URLTip of the <see cref="CellData"/>
		/// </summary>
		public string URLTip = "";
		#endregion
		#region public string Comment
		/// <summary>
		/// Get/Sets the Comment of the <see cref="CellData"/>
		/// </summary>
		public string Comment;
		#endregion
		#region public bool WrapText
		/// <summary>
		/// Get/Sets the WrapText of the <see cref="CellData"/>
		/// </summary>
		public bool WrapText;
		#endregion

		#region public double FontSize
		/// <summary>
		/// Get/Sets the FontSize of the <see cref="CellData"/>
		/// </summary>
		public double FontSize;
		#endregion

		#region public bool CenterInCell
		/// <summary>
		/// Get/Sets the CenterInCell of the <see cref="CellData"/>
		/// </summary>
		public bool CenterInCell;
		#endregion
		#region public VAlign VerticalAlignment
		/// <summary>
		/// Get/Sets the VerticalAlignment of the <see cref="CellData"/>
		/// </summary>
		public VAlign VerticalAlignment = VAlign.Bottom;
		#endregion
		#region public HAlign HorizontalAlignment
		/// <summary>
		/// Get/Sets the HorizontalAlignment of the <see cref="CellData"/>
		/// </summary>
		public HAlign HorizontalAlignment = HAlign.General;
		#endregion

		#region public CellData( string value )
		/// <summary>
		/// Initializes a new instance of the <b>CellData</b> class.
		/// </summary>
		/// <param name="value"></param>
		public CellData( string value )
			: this( value, 1, 0, false ) {
		}
		#endregion
		#region public CellData( string value, int colSpan )
		/// <summary>
		/// Initializes a new instance of the <b>CellData</b> class.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="colSpan"></param>
		public CellData( string value, int colSpan )
			: this( value, colSpan, 0, false ) {

		}
		#endregion
		#region public CellData( string value, int colSpan, int textDirectionDegrees )
		/// <summary>
		/// Initializes a new instance of the <b>CellData</b> class.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="colSpan"></param>
		/// <param name="textDirectionDegrees"></param>
		public CellData( string value, int colSpan, int textDirectionDegrees )
			: this( value, colSpan, textDirectionDegrees, false ) {
		}
		#endregion
		#region public CellData( string value, int colSpan, int textDirectionDegrees, bool textFormat )
		/// <summary>
		/// Initializes a new instance of the <b>CellData</b> class.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="colSpan"></param>
		/// <param name="textDirectionDegrees"></param>
		/// <param name="textFormat">Set to false to let excel format the content of the cell. Default value of this is true.</param>
		public CellData( string value, int colSpan, int textDirectionDegrees, bool textFormat ) {
			Value = value;
			ColSpan = colSpan;
			TextDirectionDegrees = textDirectionDegrees;
			TextFormat = textFormat;
		}
		#endregion

		#region public CellData( string value, string numberFormat )
		/// <summary>
		/// Initializes a new instance of the <b>CellData</b> class.
		/// </summary>
		/// <param name="value">The cell value</param>
		/// <param name="numberFormat">The Excel NumberFormat of the cell.</param>
		public CellData(string value, string numberFormat) {
			Value = value;
			NumberFormat = numberFormat;
		}
		#endregion
		#region public object Clone()
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public object Clone() {
			return MemberwiseClone();
		}
		#endregion
	}
}
