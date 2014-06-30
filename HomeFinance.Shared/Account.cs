using System;
using System.Collections.Generic;

namespace HomeFinance.Shared {
	public class Account {
		public int ID;
		public string Name;
		public DateTime CreatedDate;
		public int UserID;

		public List<int> PermittedUsers = new List<int>();
		public List<AccountTransaction> Transactions = new List<AccountTransaction>();
	}
}
