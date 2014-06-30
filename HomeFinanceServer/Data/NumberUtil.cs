using System.Globalization;

namespace HomeFinanceServer.Data {
	/// <summary>
	/// 
	/// </summary>
	/// <remarks></remarks>
	/// <example></example>
	public static class NumberUtil {
		#region public static string InnerTrim( this string that )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="that"></param>
		/// <returns></returns>
		public static string InnerTrim( this string that ) {
			if( string.IsNullOrEmpty( that ) ) {
				return that;
			}
			if( that.IndexOf( ' ' ) > -1 ) {
				that = that.Replace( " ", "" );
			}
			return that;
		}
		#endregion
		
		#region public static double ToDouble( string str )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static double ToDouble( string str ) {
			if( str == null ) {
				return 0;
			}
			return ToDouble( str, 0 );
		}
		#endregion
		#region public static double? ToNullableDouble( string str )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static double? ToNullableDouble( string str ) {
			if( string.IsNullOrEmpty( str ) ) {
				return null;
			}
			str = FixString( str );
			double d;
			if( double.TryParse( str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, null, out d ) ) {
				return d;
			}
			return null;
		}
		#endregion
		#region public static double ToDouble( string str, double defaultValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double ToDouble( string str, double defaultValue ) {
			str = FixString( str );
			if( string.IsNullOrEmpty( str ) ) {
				return defaultValue;
			}
			double d;
			if(!double.TryParse( str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, null, out d )) {
				d = defaultValue;
			}
			return d;
		}
		#endregion
		#region public static float ToFloat( string str )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static float ToFloat( string str ) {
			return ToFloat( str, 0 );
		}
		#endregion
		#region public static float ToFloat( string str, float defaultValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float ToFloat( string str, float defaultValue ) {
			str = FixString( str );
			if( string.IsNullOrEmpty( str ) ) {
				return defaultValue;
			}
			float d;
			if( !float.TryParse( str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands | NumberStyles.AllowExponent, null, out d ) ) {
				d = defaultValue;
			}
			return d;
		}
		#endregion
		#region public static bool ValidateValidDouble( string str )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool ValidateValidDouble( string str ) {
			str = FixString( str );
			if( string.IsNullOrEmpty( str ) ) {
				return false;
			}
			double d;
			if( !double.TryParse( str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, null, out d ) ) {
				return false;
			}
			return true;
		}
		#endregion

		#region private static string FixString( string str )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static string FixString( string str ) {
			if( string.IsNullOrEmpty( str ) ) {
				return str;
			}
			str = str.Trim();
			if( string.IsNullOrEmpty( str ) ) {
				return str;
			}
			if( NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.Equals( "," ) && str.IndexOf( "." ) > -1 ) {
				str = str.Replace( ".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator );
			}
			if( NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.Equals( "." ) && str.IndexOf( "," ) > -1 ) {
				str = str.Replace( ",", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator );
			}
			str = str.InnerTrim();
			return str;
		}
		#endregion
		#region public static string FixDoubleParsableString( string str )
		/// <summary>
		/// Fixes commas/dots in the specified string according to the <see cref="NumberFormatInfo.CurrentInfo.NumberDecimalSeparator"/>
		/// This means that no matter what decimal separator is used, the string can still be parsed to a double value
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string FixDoubleParsableString( string str ) {
			return FixString( str );
		}
		#endregion

		#region public static int? ToNullableInt( string that )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="that"></param>
		/// <returns></returns>
		public static int? ToNullableInt( string that ) {
			if( string.IsNullOrEmpty( that ) ) {
				return null;
			}
			return that.ToInt();
		}
		#endregion
	}
}
