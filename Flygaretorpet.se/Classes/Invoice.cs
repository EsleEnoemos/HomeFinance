using System;
using System.Collections.Generic;

namespace Flygaretorpet.se.Classes {
	public class Invoice : IUniqueID {
		#region public int ID
		/// <summary>
		/// Get/Sets the ID of the Invoice
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
		/// Get/Sets the Amount of the Invoice
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
		/// Get/Sets the Date of the Invoice
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
		/// Get/Sets the Comment of the Invoice
		/// </summary>
		/// <value></value>
		public string Comment {
			get { return _comment; }
			set { _comment = value; }
		}
		private string _comment;
		#endregion
		#region public string Name
		/// <summary>
		/// Get/Sets the Name of the Invoice
		/// </summary>
		/// <value></value>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		private string _name;
		#endregion

		#region public List<InvoicePayment> Payments
		/// <summary>
		/// Get/Sets the Payments of the Invoice
		/// </summary>
		/// <value></value>
		public List<InvoicePayment> Payments {
			get { return _payments; }
			set { _payments = value; }
		}
		private List<InvoicePayment> _payments;
		#endregion
		public double Balance {
			get {
				double b = Amount;
				foreach( InvoicePayment p in Payments ) {
					b -= p.Amount;
				}
				return b;
			}
		}

		#region public Invoice()
		/// <summary>
		/// Initializes a new instance of the <b>Invoice</b> class.
		/// </summary>
		public Invoice() { }
		#endregion
		#region public Invoice( int id, string name, double amount, DateTime date, string comment )
		/// <summary>
		/// Initializes a new instance of the <b>Invoice</b> class.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="amount"></param>
		/// <param name="date"></param>
		/// <param name="comment"></param>
		public Invoice( int id, string name, double amount, DateTime date, string comment ) {
			_iD = id;
			_name = name;
			_amount = amount;
			_date = date;
			_comment = comment;
		}
		#endregion
	}
}