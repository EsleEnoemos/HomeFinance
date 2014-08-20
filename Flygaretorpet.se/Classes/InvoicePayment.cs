using System;

namespace Flygaretorpet.se.Classes {
	public class InvoicePayment : IUniqueID {
		#region public int ID
		/// <summary>
		/// Get/Sets the ID of the InvoicePayment
		/// </summary>
		/// <value></value>
		public int ID {
			get { return _iD; }
			set { _iD = value; }
		}
		private int _iD;
		#endregion
		#region public double Amount
		/// <summary>
		/// Get/Sets the Amount of the InvoicePayment
		/// </summary>
		/// <value></value>
		public double Amount {
			get { return _amount; }
			set { _amount = value; }
		}
		private double _amount;
		#endregion
		#region public DateTime Date
		/// <summary>
		/// Get/Sets the Date of the InvoicePayment
		/// </summary>
		/// <value></value>
		public DateTime Date {
			get { return _date; }
			set { _date = value; }
		}
		private DateTime _date;
		#endregion
		#region public string Comment
		/// <summary>
		/// Get/Sets the Comment of the InvoicePayment
		/// </summary>
		/// <value></value>
		public string Comment {
			get { return _comment; }
			set { _comment = value; }
		}
		private string _comment;
		#endregion

		#region public InvoicePayment()
		/// <summary>
		/// Initializes a new instance of the <b>InvoicePayment</b> class.
		/// </summary>
		public InvoicePayment() { }
		#endregion
		#region public InvoicePayment( int id, double amount, DateTime date, string comment )
		/// <summary>
		/// Initializes a new instance of the <b>InvoicePayment</b> class.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="amount"></param>
		/// <param name="date"></param>
		/// <param name="comment"></param>
		public InvoicePayment( int id, double amount, DateTime date, string comment ) {
			_iD = id;
			_amount = amount;
			_date = date;
			_comment = comment;
		}
		#endregion
	}
}