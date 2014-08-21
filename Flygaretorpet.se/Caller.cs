using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Flygaretorpet.se {
	internal static class Caller {
		private static string authCookie;

		public static T Get<T>( string service, NameValueCollection queryString = null ) where T : class, new() {
			if( authCookie == null ) {
				authCookie = Login();
			}
			string url = string.Format( "{0}/Service/{1}", Plug.URL, service );
			if( queryString != null && queryString.Count > 0 ) {
				List<string> list = new List<string>();
				foreach( string key in queryString.AllKeys ) {
					if( string.IsNullOrEmpty( key ) ) {
						continue;
					}
					string[] values = queryString.GetValues( key ) ?? new string[0];
					foreach( string value in values ) {
						list.Add( string.Format( "{0}={1}", key, value ) );
					}
				}
				if( list.Count > 0 ) {
					url = string.Format( "{0}?{1}", url, string.Join( "&", list ) );
				}
			}
			HttpWebRequest req = HttpWebRequest.CreateHttp( url );
			req.Method = "GET";
			req.ContentLength = 0;
			req.KeepAlive = false;
			req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0 HomeFinance";
			req.AllowAutoRedirect = false;
			CookieContainer cookies = new CookieContainer( 1 );
			Cookie c = new Cookie( ".ASPXAUTH", authCookie, "/", Plug.URL.Replace( "http:", "" ).Replace( "/", "" ) );
			cookies.Add( c );
			req.CookieContainer = cookies;
			string result;
			using( HttpWebResponse res = (HttpWebResponse)req.GetResponse() ) {
				string authCookieValue = GetAuthCookieValue( res );
				if( !string.IsNullOrEmpty( authCookieValue ) ) {
					authCookie = authCookieValue;
				}
				using( Stream ins = res.GetResponseStream() ) {
					using( StreamReader sr = new StreamReader( ins, Encoding.UTF8 ) ) {
						result = sr.ReadToEnd();
						sr.Close();
					}
					ins.Close();
				}
				res.Close();
			}
			JsonSerializer ser = new JsonSerializer();
			using( StringReader sr = new StringReader( result ) ) {
				using( JsonTextReader tr = new JsonTextReader( sr ) ) {
					T data = ser.Deserialize<T>( tr );
					return data;
				}
			}
		}
		public static T Post<T>( string service, NameValueCollection formValues, NameValueCollection queryString = null ) where T : class, new() {
			if( authCookie == null ) {
				authCookie = Login();
			}
			string url = string.Format( "{0}/Service/{1}", Plug.URL, service );
			if( queryString != null && queryString.Count > 0 ) {
				List<string> list = new List<string>();
				foreach( string key in queryString.AllKeys ) {
					if( string.IsNullOrEmpty( key ) ) {
						continue;
					}
					string[] values = queryString.GetValues( key ) ?? new string[ 0 ];
					foreach( string value in values ) {
						list.Add( string.Format( "{0}={1}", key, HttpUtility.UrlEncode( value ) ) );
					}
				}
				if( list.Count > 0 ) {
					url = string.Format( "{0}?{1}", url, string.Join( "&", list ) );
				}
			}
			HttpWebRequest req = HttpWebRequest.CreateHttp( url );
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			List<string> post = new List<string>();
			foreach( string key in formValues.AllKeys ) {
				if( string.IsNullOrEmpty( key ) ) {
					continue;
				}
				string[] arr = formValues.GetValues( key );
				if( arr == null ) {
					continue;
				}
				foreach( string s in arr ) {
					post.Add( string.Format( "{0}={1}", key, HttpUtility.UrlEncode( s ) ) );
				}
			}
			byte[] bytes = string.Join( "&", post ).ToByteArray();
			req.ContentLength = bytes.Length;
			req.KeepAlive = false;
			req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0 HomeFinance";
			req.AllowAutoRedirect = false;
			CookieContainer cookies = new CookieContainer( 1 );
			Cookie c = new Cookie( ".ASPXAUTH", authCookie, "/", Plug.URL.Replace( "http:", "" ).Replace( "/", "" ) );
			cookies.Add( c );
			req.CookieContainer = cookies;
			string result;
			using( Stream os = req.GetRequestStream() ) {
				os.Write( bytes, 0, bytes.Length );
				os.Flush();
				os.Close();
				using( HttpWebResponse res = (HttpWebResponse)req.GetResponse() ) {
					string authCookieValue = GetAuthCookieValue( res );
					if( !string.IsNullOrEmpty( authCookieValue ) ) {
						authCookie = authCookieValue;
					}
					using( Stream ins = res.GetResponseStream() ) {
						using( StreamReader sr = new StreamReader( ins, Encoding.UTF8 ) ) {
							result = sr.ReadToEnd();
							sr.Close();
						}
						ins.Close();
					}
					res.Close();
				}
			}
			JsonSerializer ser = new JsonSerializer();
			using( StringReader sr = new StringReader( result ) ) {
				using( JsonTextReader tr = new JsonTextReader( sr ) ) {
					T data = ser.Deserialize<T>( tr );
					return data;
				}
			}
		}

		internal static string Login() {
			HttpWebRequest req = HttpWebRequest.CreateHttp( string.Format( "{0}Login.aspx", Plug.URL ) );
			req.Method = "POST";
			byte[] bytes = string.Format( "email={0}&password={1}", Plug.EMail, Plug.Password ).ToByteArray();
			req.ContentLength = bytes.Length;
			req.KeepAlive = false;
			req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
			req.ContentType = "application/x-www-form-urlencoded";
			req.AllowAutoRedirect = false;
			using( Stream stream = req.GetRequestStream() ) {
				stream.Write( bytes, 0, bytes.Length );
				stream.Flush();
				stream.Close();
				//.ASPXAUTH
				using( HttpWebResponse webResponse = (HttpWebResponse)req.GetResponse() ) {
					return GetAuthCookieValue( webResponse );
				}
			}
		}
		private static string GetAuthCookieValue( HttpWebResponse r ) {
			string s = r.Headers[ "Set-Cookie" ];
			if( string.IsNullOrEmpty( s ) ) {
				return null;
			}
			string[] strings = s.Split( '=' );
			if( string.Equals( strings[ 0 ], ".ASPXAUTH" ) ) {
				return strings[ 1 ].Substring( 0, strings[ 1 ].IndexOf( ";" ) );
			}
			return null;
		}
	}
	public static class Exts {
		public static byte[] ToByteArray( this string ths ) {
			if( string.IsNullOrEmpty( ths ) ) {
				return new byte[ 0 ];
			}
			List<byte> bb = new List<byte>();
			foreach( char c in ths ) {
				if( c > 255 ) {
					byte[] bytes = Encoding.UTF8.GetBytes( new[] { c } );
					bb.AddRange( bytes );
				} else {
					bb.Add( Convert.ToByte( c ) );
				}
			}
			return bb.ToArray();
		}
		public static string GetString( this byte[] ths ) {
			if( ths == null || ths.Length == 0 ) {
				return "";
			}
			StringBuilder sb = new StringBuilder();
			foreach( byte b in ths ) {
				sb.Append( (char)b );
				//sb.Append( Convert.ToString( b ) );
			}
			return sb.ToString();
		}
	}
}
