using System;
using System.Collections.Generic;

namespace HomeFinance.Shared {
	public class Bill {
		public int ID;
		public double TotalUsedKWh;
		public double TotalPriceElectricity;
		public double TotalPriceGroundFee;

		public long GuestLastReadingTicks;
		public double GuestLastReadingKWh;
		public long GuestCurrentReadingTicks;
		public double GuestCurrentReadingKWh;
		public int GuestPartInGroundFee;
		public string OCR;
		public DateTime CreatedDate;
		public int CreatedByUserID;

		public override string ToString() {
			return string.Format( "{0} --> {1}", new DateTime( GuestLastReadingTicks ).ToString( "yyyy-MM-dd" ), new DateTime( GuestCurrentReadingTicks ).ToString( "yyyy-MM-dd" ) );
		}

	}
	public class BillList : List<Bill> {
		//#region private static XmlSerializer Serializer
		///// <summary>
		///// Gets the Serializer of the Bill
		///// </summary>
		///// <value></value>
		//private static XmlSerializer Serializer {
		//    get {
		//        return _serializer ?? (_serializer = new XmlSerializer( typeof( BillList ) ));
		//    }
		//}
		//private static XmlSerializer _serializer;
		//#endregion
		//#region private static FileInfo SaveFile
		///// <summary>
		///// Gets the File of the Bill
		///// </summary>
		///// <value></value>
		//private static FileInfo SaveFile {
		//    get {
		//        return _file ?? (_file = new FileInfo( string.Format( "{0}\\ElectricBills.xml", new DirectoryInfo( Application.StartupPath ).FullName ) ));
		//    }
		//}
		//private static FileInfo _file;
		//#endregion

		#region public BillList SortByDate()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public BillList SortByDate() {
			if( Count > 1 ) {
				Sort( delegate( Bill x, Bill y ) {
					return x.GuestLastReadingTicks.CompareTo( y.GuestLastReadingTicks );
				} );
			}
			return this;
		}
		#endregion

		//#region public void Save()
		///// <summary>
		///// 
		///// </summary>
		//public void Save() {
		//    if( File.Exists( SaveFile.FullName ) ) {
		//        File.Delete( SaveFile.FullName );
		//    }
		//    using( Stream s = File.OpenWrite( SaveFile.FullName ) ) {
		//        try {
		//            Serializer.Serialize( s, this );
		//            s.Flush();
		//            s.Close();
		//        } catch {
		//        }
		//    }
		//}
		//#endregion
		//#region public static BillList Load()
		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public static BillList Load() {
		//    if( !File.Exists( SaveFile.FullName ) ) {
		//        return new BillList();
		//    }
		//    using( Stream s = File.OpenRead( SaveFile.FullName ) ) {
		//        try {
		//            return ((Serializer.Deserialize( s ) as BillList) ?? new BillList()).SortByDate();
		//        } catch {
		//        }
		//    }
		//    return new BillList();
		//}
		//#endregion
	}
}
