using System.Collections.Specialized;
using System.Web;

namespace HomeFinanceServer.Data {
	public static class Exts {
		public static string DisplayFormat( this string ths ) {
			if( string.IsNullOrEmpty( ths ) ) {
				return ths;
			}
			if( ths.Contains( "\n" ) || ths.Contains( "\r" ) ) {
				ths = ths.Replace( "\n\r", "<br/>" ).Replace( "\n", "<br/>" ).Replace( "\r", "<br/>" );
			}
			return ths;
		}

		public static string IfTrue( this string ths, bool that ) {
			return that ? ths : null;
		}
		public static double ToDouble( this string str ) {
			return str == null ? 0 : NumberUtil.ToDouble( str );
		}
		public static int ToInt( this string str ) {
			int i;
			return int.TryParse( str, out i ) ? i : 0;
		}
		public static long ToLong( this string str ) {
			long l;
			return long.TryParse( str, out l ) ? l : 0;
		}
		public static int GetInt( this HttpRequest req, string name ) {
			return string.Equals( req.HttpMethod, "POST" ) ? req.Form[ name ].ToInt() : req.QueryString[ name ].ToInt();
		}
		public static double GetDouble( this HttpRequest req, string name ) {
			return string.Equals( req.HttpMethod, "POST" ) ? req.Form[ name ].ToDouble() : req.QueryString[ name ].ToDouble();
		}
		public static long GetLong( this HttpRequest req, string name ) {
			return string.Equals( req.HttpMethod, "POST" ) ? req.Form[ name ].ToLong() : req.QueryString[ name ].ToLong();
		}
		public static string GetString( this HttpRequest req, string name ) {
			return string.Equals( req.HttpMethod, "POST" ) ? req.Form[ name ] : req.QueryString[ name ];
		}
	}
}