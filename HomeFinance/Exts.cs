// ReSharper disable CheckNamespace

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using HomeFinance.Shared;

public static class Exts {
	// ReSharper restore CheckNamespace
	public static int ToInt( this string ths ) {
		int i;
		return int.TryParse( ths, out i ) ? i : 0;
	}
	public static int ToInt( this string ths, int defaultIfFail ) {
		int i;
		return int.TryParse( ths, out i ) ? i : defaultIfFail;
	}
	public static long ToLong( this string str, long defaultIfFail = 0 ) {
		long l;
		return long.TryParse( str, out l ) ? l : defaultIfFail;
	}
	#region public static double ToDouble( string str )
	/// <summary>
	/// 
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static double ToDouble( this string str ) {
		if( str == null ) {
			return 0;
		}
		return ToDouble( str, 0 );
	}
	#endregion
	#region public static double ToDouble( string str, double defaultValue )
	/// <summary>
	/// 
	/// </summary>
	/// <param name="str"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static double ToDouble( this string str, double defaultValue ) {
		str = FixString( str );
		if( string.IsNullOrEmpty( str ) ) {
			return defaultValue;
		}
		double d;
		if( !double.TryParse( str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, null, out d ) ) {
			d = defaultValue;
		}
		return d;
	}
	#endregion

	public static string Format( this System.DateTime ths ) {
		return ths.ToString( "yyyy-MM-dd HH:mm" );
	}
	public static string Format( this System.DateTime? ths ) {
		return !ths.HasValue ? null : ths.Value.Format();
	}
	public static bool StartsWithNumber( this string str ) {
		return !string.IsNullOrEmpty( str ) && char.IsNumber( str[ 0 ] );
	}
	public static double ValueOrMax( this double value, double max ) {
		return value > max ? max : value;
	}
	public static string ToString( this List<string> lst, string separator ) {
		StringBuilder sb = new StringBuilder();
		for( int i = 0; i < lst.Count; i++ ) {
			sb.Append( lst[ i ] );

			if( (i+1) < lst.Count ) {
				sb.Append( separator );
			}
		}
		return sb.ToString();
	}
	public static string FillBlanks( this string str, params object[] args ) {
		return string.Format( str, args );
	}
	public static void Add( this NameValueCollection self, string name, int value ) {
		self.Add( name, value.ToString() );
	}
	public static void Add( this NameValueCollection self, string name, double value ) {
		self.Add( name, value.ToString() );
	}
	public static void Add( this NameValueCollection self, string name, long value ) {
		self.Add( name, value.ToString() );
	}
	public static User GetByID( this List<User> list, int id ) {
		foreach( User user in list ) {
			if( id == user.ID ) {
				return user;
			}
		}
		return null;
	}
	public static User GetByName( this List<User> list, string name ) {
		foreach( User user in list ) {
			if( string.Equals( user.Username, name ) ) {
				return user;
			}
		}
		return null;
	}
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
	public static string Format( this double d ) {
		return d.ToString( "N2" );
	}
}
