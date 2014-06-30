using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace HomeFinance {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			//DateTime dateTime = DateTime.Parse( "2012-01-02" );
			//dateTime = DateTime.Parse( "2012-01-01", new DateTimeFormatInfo(){FullDateTimePattern = "yyyy-MM-dd HH:mm:ss"} );
			//return;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new Form1() );
		}
	}
}
