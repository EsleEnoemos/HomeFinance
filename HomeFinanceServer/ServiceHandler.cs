using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace HomeFinanceServer {
	public class ServiceHandler : IHttpHandler {
		public bool IsReusable {
			get {
				return false;
			}
		}
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
		public void ProcessRequest( HttpContext context ) {
			string path = context.Request.Path;
			if( string.IsNullOrEmpty( path ) ) {
				return;
			}
			if( path.StartsWith( "/" ) ) {
				path = path.Substring( 1 );
			}
			string[] parts = path.Split( '/' );
			if( parts.Length < 2 ) {
				return;
			}
			Assembly ass = Assembly.GetExecutingAssembly();
			Type type = ass.GetType( string.Format( "HomeFinanceServer.Services.{0}", parts[ 0 ] ) );
			if( type == null ) {
				return;
			}
			object serviceInstance = ass.CreateInstance( type.FullName );
			if( serviceInstance == null ) {
				return;
			}
			MethodInfo serviceMethod = type.GetMethod( parts[ 1 ] );
			if( serviceMethod == null ) {
				return;
			}
			object[] parameters = new object[]{context};
			object result = serviceMethod.Invoke( serviceInstance, parameters );
			if( result != null ) {
				JavaScriptSerializer js = new JavaScriptSerializer();
				try {
					js.MaxJsonLength = int.MaxValue;
				} catch {
				}
				string str = js.Serialize( result );
				//byte[] resultBytes;
				//using( MemoryStream ms = new MemoryStream() ) {
				//    using( GZipStream z = new GZipStream( ms, CompressionMode.Compress ) ) {
				//        byte[] stringBytes = Encoding.UTF8.GetBytes( str );
				//        z.Write( stringBytes, 0, stringBytes.Length );
				//        z.Flush();
				//        resultBytes = ms.ToArray();
				//    }
				//}
				HttpResponse res = context.Response;
				res.ClearHeaders();
				res.Clear();
				res.ContentType = "application/x-javascript";
				//res.ContentType = "binary";
				res.ContentEncoding = Encoding.UTF8;
				int len = Encoding.UTF8.GetByteCount( str );
				res.Headers.Add( "Content-Length", len.ToString() );
				//res.Headers.Add( "Content-Length", resultBytes.Length.ToString() );
				res.Write( str );
				//res.BinaryWrite( resultBytes );
				res.Flush();
			}
		}
	}
}