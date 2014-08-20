using System.Collections.Generic;

namespace Flygaretorpet.se.Classes {
	public class House : IUniqueID {
		#region public int ID
		/// <summary>
		/// Get/Sets the ID of the House
		/// </summary>
		/// <value></value>
		public int ID {
			get { return _iD; }
			set { _iD = value; }
		}
		private int _iD;
		#endregion
		#region public string Name
		/// <summary>
		/// Get/Sets the Name of the House
		/// </summary>
		/// <value></value>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		private string _name;
		#endregion

		#region public List<Invoice> Invoices
		/// <summary>
		/// Get/Sets the Invoices of the House
		/// </summary>
		/// <value></value>
		public List<Invoice> Invoices {
			get {
				return _invoices;
			}
			set { _invoices = value; }
		}
		private List<Invoice> _invoices;
		#endregion

		#region public House()
		/// <summary>
		/// Initializes a new instance of the <b>House</b> class.
		/// </summary>
		public House() { }
		#endregion
		#region public House( int id, string name )
		/// <summary>
		/// Initializes a new instance of the <b>House</b> class.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		public House( int id, string name ) {
			_iD = id;
			_name = name;
		}
		#endregion
	}
}