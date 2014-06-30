using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace HomeFinance {
	public class JSONWebServiceCaller {
		public T GetData<T>( string url ) where T : new() {
			StringReader sr = null;
			try {
				using( WebClient wc = WC( url ) ) {
					wc.BaseAddress = url;
					byte[] bytes = wc.DownloadData( url );
					string result = DecodeResult( bytes );
					//string result = wc.DownloadString( url );
					JsonSerializer ser = new JsonSerializer();
					sr = new StringReader( result );
					T data = ser.Deserialize<T>( new JsonTextReader( sr ) );
					return data;
				}
			} catch {
				return new T();
			} finally {
				if( sr != null ) {
					sr.Dispose();
				}
			}
		}
		public T PostData<T>( string url, NameValueCollection data ) {
			using( WebClient wc = WC( url ) ) {
				wc.BaseAddress = url;
				byte[] result = wc.UploadValues( url, "POST", data );
				JsonSerializer ser = new JsonSerializer();
				//string s = Encoding.UTF8.GetString( result );
				string s = DecodeResult( result );
				StringReader sr = new StringReader( s );
				T t = ser.Deserialize<T>( new JsonTextReader( sr ) );
				return t;
			}
		}
		private static string DecodeResult( byte[] bytes ) {
			string s = Encoding.UTF8.GetString( bytes );
			return s;
			//ByteBuffer buff = new ByteBuffer();
			//using( MemoryStream ms = new MemoryStream(bytes) ) {
			//    //ms.Write( bytes, 0, bytes.Length );
			//    //ms.Flush();
			//    using( GZipStream z = new GZipStream( ms, CompressionMode.Decompress ) ) {
			//        byte[] decBytes = new byte[1024];
			//        ms.Position = 0;
			//        int read = z.Read( decBytes, 0, 1024 ); // WHY???, first read always returns 0...
			//        decBytes = new byte[ 1024 ];
			//        ms.Position = 0;
			//        try {
			//            while( (read = z.Read( decBytes, 0, 1024 )) > 0 ) {
			//                buff.Append( decBytes, read );
			//            }
			//        } catch {}
			//    }
			//}
			//string s = Encoding.UTF8.GetString( buff.GetBytes() );
			//return s;
		}
		private static WebClient WC( string url ) {
			WebClient wc = new WebClient();
			//wc.Credentials = new NetworkCredential( Environment.UserName, "" );
			wc.Credentials = new NetworkCredential( GetUsername(), "" );
			wc.BaseAddress = url;
			wc.Encoding = Encoding.UTF8;
			return wc;
		}

		private static string GetUsername() {
			try {
				string un = ConfigurationManager.AppSettings[ "AccountName" ];
				if( !string.IsNullOrEmpty( un ) ) {
					return un;
				}
			} catch { }
			return Environment.UserName;
		}
	}
}
