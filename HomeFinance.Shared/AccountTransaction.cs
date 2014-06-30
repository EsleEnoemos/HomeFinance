using System;
using System.Globalization;

namespace HomeFinance.Shared {
	public class AccountTransaction {
		public int ID;
		public double Amount;
		public string Comment;
		public int UserID;
		#region public DateTime Date
		/// <summary>
		/// Get/Sets the Date of the AccountTransaction
		/// </summary>
		/// <value></value>
		[Newtonsoft.Json.JsonIgnore]
		public DateTime Date {
			get {
				if( _date == DateTime.MinValue ) {
					_date = FromSTR( _dateString );
				}
				return _date;
			}
			set {
				_date = value;
				_dateString = ToSTR( value );
			}
		}
		private DateTime _date;
		#endregion
		#region public string DateString
		/// <summary>
		/// Get/Sets the DateString of the AccountTransaction
		/// </summary>
		/// <value></value>
		public string DateString {
			get {
				if( string.IsNullOrEmpty( _dateString ) ) {
					_dateString = ToSTR( _date );
				}
				return _dateString;
			}
			set {
				_dateString = value;
				_date = FromSTR( value );
			}
		}
		private string _dateString;
		#endregion

		private static string ToSTR( DateTime d ) {
			return d.ToString( "yyyy-MM-dd" );
		}
		private static DateTime FromSTR( string s ) {
			try {
				return DateTime.Parse( s, new DateTimeFormatInfo() {
					FullDateTimePattern = "yyyy-MM-dd HH:mm:ss"
				} );
			} catch {
			}
			return new DateTime();
		}
	}
}