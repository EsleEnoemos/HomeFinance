using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using HomeFinance.Shared;
using HomeFinanceServer.Auth;
using HomeFinanceServer.Data;

namespace HomeFinanceServer.Services {
	public class BillService {
		public List<Bill> Get( HttpContext context ) {
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = "GetElectricBills";
				List<Bill> list = new List<Bill>();
				while( cmd.Read() ) {
					list.Add( new Bill {
						ID = cmd.GetInt( "ElectricBill_ID" ),
						TotalUsedKWh = cmd.GetDouble( "TotalUsedKWh" ),
						TotalPriceElectricity = cmd.GetDouble( "TotalPriceElectricity" ),
						TotalPriceGroundFee = cmd.GetDouble( "TotalPriceGroundFee" ),
						GuestLastReadingTicks = cmd.GetLong( "GuestLastReadingTicks" ),
						GuestLastReadingKWh = cmd.GetDouble( "GuestLastReadingKWh" ),
						GuestCurrentReadingTicks = cmd.GetLong( "GuestCurrentReadingTicks" ),
						GuestCurrentReadingKWh = cmd.GetDouble( "GuestCurrentReadingKWh" ),
						GuestPartInGroundFee = cmd.GetInt( "GuestPartInGroundFee" ),
						OCR = cmd.GetString( "OCR" ),
						CreatedDate = cmd.GetDateTime( "CreatedDate" ),
						CreatedByUserID = cmd.GetInt( "CreatedByUser_ID" )
					} );
				}
				return list;
			}
		}
		public Bill Save( HttpContext context ) {
			HttpRequest req = context.Request;
			Bill bill = new Bill {
				ID = req.GetInt( "ElectricBill_ID" ), TotalUsedKWh = req.GetDouble( "TotalUsedKWh" ), TotalPriceElectricity = req.GetDouble( "TotalPriceElectricity" ), TotalPriceGroundFee = req.GetDouble( "TotalPriceGroundFee" ), GuestLastReadingTicks = req.GetLong( "GuestLastReadingTicks" ), GuestLastReadingKWh = req.GetDouble( "GuestLastReadingKWh" ), GuestCurrentReadingTicks = req.GetLong( "GuestCurrentReadingTicks" ), GuestCurrentReadingKWh = req.GetDouble( "GuestCurrentReadingKWh" ), GuestPartInGroundFee = req.GetInt( "GuestPartInGroundFee" ), OCR = req.GetString( "OCR" ), CreatedDate = new DateTime( req.GetLong( "CreatedDate" ) ), CreatedByUserID = req.GetInt( "CreatedByUser_ID" )
			};
			using( DBCommand cmd = DBCommand.New ) {
				cmd.CommandText = "UpdateElectricBill";
				SqlParameter id = cmd.Add( "@ElectricBill_ID", SqlDbType.Int, ParameterDirection.InputOutput, bill.ID );
				cmd.AddWithValue( "@TotalUsedKWh", bill.TotalUsedKWh );
				cmd.AddWithValue( "@TotalPriceElectricity", bill.TotalPriceElectricity );
				cmd.AddWithValue( "@TotalPriceGroundFee", bill.TotalPriceGroundFee );
				cmd.AddWithValue( "@GuestLastReadingTicks", bill.GuestLastReadingTicks );
				cmd.AddWithValue( "@GuestLastReadingKWh", bill.GuestLastReadingKWh );
				cmd.AddWithValue( "@GuestCurrentReadingTicks", bill.GuestCurrentReadingTicks );
				cmd.AddWithValue( "@GuestCurrentReadingKWh", bill.GuestCurrentReadingKWh );
				cmd.AddWithValue( "@GuestPartInGroundFee", bill.GuestPartInGroundFee );
				cmd.AddWithValue( "@OCR", DBCommand.NullZero( bill.OCR ) );
				FinanceUser user = FinanceUser.Load( context.User.Identity.Name );
				cmd.AddWithValue( "@CreatedByUser_ID", user.ID );
				cmd.ExecuteNonQuery();
				if( bill.ID <= 0 ) {
					bill.ID = (int)id.Value;
				}
				bill.CreatedByUserID = user.ID;
			}
			return bill;
		}
	}
}