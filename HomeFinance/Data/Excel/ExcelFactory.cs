using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using SpreadsheetGear;

namespace HomeFinance.Data.Excel {
	/// <summary>
	/// Handels export to Excel
	/// </summary>
	/// <remarks></remarks>
	/// <example></example>
	public static class ExcelFactory {
		#region public static void ExportList( Stream streamToWriteOutputTo, List<string> headers, List<List<string>> data, string sheetName )
		/// <summary>
		/// Creates an Excel document with headers as in the headers-parameters.
		/// The rows of data will be filled with data from the data-parameter.
		/// Name of the sheet will be set to sheetName.
		/// The document is written to the given stream.
		/// </summary>
		/// <param name="streamToWriteOutputTo"></param>
		/// <param name="headers"></param>
		/// <param name="data"></param>
		/// <param name="sheetName"></param>
		public static void ExportList( Stream streamToWriteOutputTo, List<string> headers, List<List<string>> data, string sheetName ) {
			IWorkbookSet workBook = Factory.GetWorkbookSet();
			IWorkbook iBook = workBook.Workbooks.Add();
			IWorksheet iSheet = iBook.Worksheets.Add();
			iSheet.Name = sheetName;
			IRange cells = iSheet.Cells;
			int cellIndex = 0;
			foreach( string header in headers ) {
				cells[ 0, cellIndex++ ].Value = header;
			}
			cells[ 0, 0, 0, cellIndex ].Font.Bold = true;
			int rowIndex = 1;
			foreach( List<string> strings in data ) {
				cellIndex = 0;
				foreach( string s in strings ) {
					cells[ rowIndex, cellIndex ].NumberFormat = "@";
					cells[ rowIndex, cellIndex++ ].Value = s;
				}
				rowIndex++;
			}
			iBook.SaveToStream( streamToWriteOutputTo, FileFormat.Excel8 );
		}
		#endregion
		#region public static void ExportList( Stream streamToWriteOutputTo, ExportPart sheet )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="streamToWriteOutputTo"></param>
		/// <param name="sheet"></param>
		public static void ExportList( Stream streamToWriteOutputTo, ExportPart sheet ) {
			List<ExportPart> tmp = new List<ExportPart>( 1 );
			tmp.Add( sheet );
			ExportList( streamToWriteOutputTo, tmp );
		}
		#endregion

		#region public static void ExportList( Stream streamToWriteOutputTo, List<ExportPart> sheets )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="streamToWriteOutputTo"></param>
		/// <param name="sheets"></param>
		public static void ExportList( Stream streamToWriteOutputTo, List<ExportPart> sheets ) {
			ExportList( streamToWriteOutputTo, sheets, FileFormat.Excel8 );
		}
		#endregion
		#region public static void ExportList( Stream streamToWriteOutputTo, List<ExportPart> sheets, FileFormat format )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="streamToWriteOutputTo"></param>
		/// <param name="sheets"></param>
		/// <param name="format"></param>
		public static void ExportList( Stream streamToWriteOutputTo, List<ExportPart> sheets, FileFormat format ) {
			sheets = FixSheetNames( sheets );
			IWorkbookSet workBook = Factory.GetWorkbookSet();
			int maxRows = (format == FileFormat.OpenXMLWorkbook || format == FileFormat.OpenXMLWorkbookMacroEnabled) ? 1048575 : 65536;

			string tempSheetName = null;
			IWorkbook iBook = workBook.Workbooks.Add();
			try {
				// There's allways 1 sheet when creating a new book.
				// If a new sheet has the same name, an exception will be thrown.
				// So, this sheet is renamed, and in the end, deleted. (we can't delete it here since there must be at least 1 sheet in a book)
				if( iBook.Sheets[ "Sheet1" ] != null ) {
					tempSheetName = Guid.NewGuid().ToString().Substring( 0, 31 );
					iBook.Sheets[ "Sheet1" ].Name = tempSheetName;
				}
			} catch {
			}
			IColors colorPalett = iBook.Colors;
			Hashtable colorHash = new Hashtable();
			if( format == FileFormat.OpenXMLWorkbook ) {
				colorPalett = null;
				colorHash = null;
			}
			int colorPalettIndex = 0;
			if( colorPalett != null ) {
				colorPalett[ colorPalettIndex++ ] = Color.Black;
				colorPalett[ colorPalettIndex++ ] = Color.White;
				colorHash[ Color.Black.ToArgb() ] = null;
				colorHash[ Color.White.ToArgb() ] = null;
			}
			List<DataValidationInfo> dataValidationInfos = new List<DataValidationInfo>();
			foreach( ExportPart part in sheets ) {
				IWorksheet iSheet = iBook.Worksheets.Add();
				iSheet.Name = part.SheetName;
				IRange cells = iSheet.Cells;
				
				int cellIndex = 0;
				int rowIndex = AddExcelHeaders( cells, part, colorHash, colorPalett, ref colorPalettIndex );
				int startRowIndex = rowIndex;
				cellIndex = 0;
				int sheetCount = 1;
				int totalItemIndex = 0;
				Hashtable rowSpans = new Hashtable();
				if( part.DataValidationLists.Count > 0 ) {
					foreach( ExportPart.DataValidationList dvl in part.DataValidationLists ) {
						dataValidationInfos.Add( new DataValidationInfo() {
							DVL = dvl, Sheet = iSheet, StartRow = rowIndex
						} );
					}
				}
				foreach( CellDataList data in part.Data ) {
					foreach( CellData value in data ) {
						if( rowSpans.CheckIfRowSpanAndDecrease( cellIndex ) ) {
							continue;
						}
						if( value.URL != null ) {
							cells[ rowIndex, cellIndex ].Hyperlinks.Add( cells[ rowIndex, cellIndex ], value.URL, "", value.URLTip, value.Value );
						}
						if( value.TextFormat ) {
							cells[ rowIndex, cellIndex ].NumberFormat = "@";
						}
						if( !string.IsNullOrEmpty( value.NumberFormat ) ) {
							cells[ rowIndex, cellIndex ].NumberFormat = value.NumberFormat;
						}
						if( value.TextDirectionDegrees > 0 ) {
							cells[ rowIndex, cellIndex ].Orientation = value.TextDirectionDegrees;
						}
						if( value.BackgroundColor != Color.Transparent ) {
							if( colorHash != null && !colorHash.ContainsKey( value.BackgroundColor.ToArgb() ) && colorPalettIndex < 56 ) {
								colorHash[ value.BackgroundColor.ToArgb() ] = null;
								colorPalett[ colorPalettIndex++ ] = MakePalettValid( value.BackgroundColor );
							}
							cells[ rowIndex, cellIndex ].Interior.Color = value.BackgroundColor;
							//value.Value = "Color.FromArgb( {0}, {1}, {2} )".FillBlanks( value.BackgroundColor.R, value.BackgroundColor.G, value.BackgroundColor.B );
						}
						if( value.FontColor != SystemColors.WindowText ) {
							if( colorHash != null && !colorHash.ContainsKey( value.FontColor.ToArgb() ) && colorPalettIndex < 56 ) {
								colorHash[ value.FontColor.ToArgb() ] = null;
								colorPalett[ colorPalettIndex++ ] = MakePalettValid( value.FontColor );
							}
							cells[ rowIndex, cellIndex ].Font.Color = value.FontColor;
						}
						if( value.Bold ) {
							cells[ rowIndex, cellIndex ].Font.Bold = value.Bold;
						}
						if( value.Underline.HasValue ) {
							cells[ rowIndex, cellIndex ].Font.Underline = value.Underline.Value ? UnderlineStyle.Single : UnderlineStyle.None;
						}
						if( value.Pattern ) {
							cells[rowIndex, cellIndex].Interior.Pattern = Pattern.Down;
							cells[rowIndex, cellIndex].Interior.PatternColor = Color.White;
						}
						if (value.PatternColor != Color.Empty ) {
							if( colorHash != null && !colorHash.ContainsKey( value.PatternColor.ToArgb() ) && colorPalettIndex < 56 ) {
								colorHash[ value.PatternColor.ToArgb() ] = null;
								colorPalett[ colorPalettIndex++ ] = MakePalettValid( value.PatternColor );
							}
							cells[ rowIndex, cellIndex ].Interior.PatternColor = value.PatternColor;
						}
						
						cells[rowIndex, cellIndex].WrapText = value.WrapText;
						
						if( value.LeftBorder ) {
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeLeft ].Weight = BorderWeight.Thin;
						}
						if( value.LeftBorderColor != Color.Transparent ) {
							if( colorHash != null && !colorHash.ContainsKey( value.LeftBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
								colorHash[ value.LeftBorderColor.ToArgb() ] = null;
								colorPalett[ colorPalettIndex++ ] = MakePalettValid( value.LeftBorderColor );
							}
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeLeft ].Color = value.LeftBorderColor;
						}
						if( value.RightBorder ) {
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeRight ].Weight = BorderWeight.Thin;
						}
						if( value.RightBorderColor != Color.Transparent ) {
							if( colorHash != null && !colorHash.ContainsKey( value.RightBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
								colorHash[ value.RightBorderColor.ToArgb() ] = null;
								colorPalett[ colorPalettIndex++ ] = MakePalettValid( value.RightBorderColor );
							}
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeRight ].Color = value.RightBorderColor;
						}
						if( value.TopBorder ) {
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeTop ].Weight = BorderWeight.Thin;
						}
						if( value.TopBorderColor != Color.Transparent ) {
							if( colorHash != null && !colorHash.ContainsKey( value.TopBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
								colorHash[ value.TopBorderColor.ToArgb() ] = null;
								colorPalett[ colorPalettIndex++ ] = MakePalettValid( value.TopBorderColor );
							}
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeTop ].Color = value.TopBorderColor;
						}
						if( value.BottomBorder ) {
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeBottom ].Weight = BorderWeight.Thin;
						}
						if( value.BottomBorderColor != Color.Transparent ) {
							if( colorHash != null && !colorHash.ContainsKey( value.BottomBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
								colorHash[ value.BottomBorderColor.ToArgb() ] = null;
								colorPalett[ colorPalettIndex++ ] = MakePalettValid( value.BottomBorderColor );
							}
							cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeBottom ].Color = value.BottomBorderColor;
						}
						if( !string.IsNullOrEmpty( value.Comment ) ) {
							cells[ rowIndex, cellIndex ].AddComment( value.Comment ).Visible = false;
						}
						if ( value.CenterInCell ) {
							cells[rowIndex, cellIndex].Style.HorizontalAlignment = HAlign.Center;
							cells[rowIndex, cellIndex].Style.VerticalAlignment = VAlign.Center;
						}
						if( value.VerticalAlignment != VAlign.Bottom ) {
							cells[ rowIndex, cellIndex ].Style.VerticalAlignment = value.VerticalAlignment;
						}
						if( value.HorizontalAlignment != HAlign.General ) {
							cells[ rowIndex, cellIndex ].Style.HorizontalAlignment = value.HorizontalAlignment;
						}
						if( value.FontSize > 0f ) {
							cells[rowIndex, cellIndex].Font.Size = value.FontSize;
						}
						try {
							cells[ rowIndex, cellIndex ].Value = value.Value;
						} catch( ArgumentException ex ) {
							cells[ rowIndex, cellIndex ].Value = "Error setting value with " + (value.Value != null ? value.Value.Length + " characters" : "null") + ": " + ex.Message + " Try exporting as text instead...";
							cells[ rowIndex, cellIndex ].Font.Bold = true;
						} catch( Exception ex ) {
							cells[ rowIndex, cellIndex ].Value = "Error setting value: " + ex.Message;
							cells[ rowIndex, cellIndex ].Font.Bold = true;
						}
						//cells[ rowIndex, cellIndex ].ShrinkToFit = true; // krymper bara texten...
						if( value.ColumnWidth > 0 ) {
							cells[ rowIndex, cellIndex ].ColumnWidth = value.ColumnWidth;
						}
						if( value.RowHeight > 0 ) {
							cells[ rowIndex, cellIndex ].RowHeight = value.RowHeight;
						}
						if( value.ColSpan > 1 ) {
							cells[ rowIndex, cellIndex, rowIndex, cellIndex + value.ColSpan - 1 ].Merge();
							//cells[ rowIndex, cellIndex + value.ColSpan - 1 ].Merge();
						}
						if( value.RowSpan > 1 ) {
							cells[ rowIndex, cellIndex, rowIndex + value.RowSpan - 1, cellIndex ].Merge();
							rowSpans[ cellIndex ] = value.RowSpan;
						}
						if( value.ColSpan > 1 ) {
							cellIndex += value.ColSpan;
						} else {
							cellIndex++;
						}
					}
					cellIndex = 0;
					rowIndex++;
					
					//if( rowIndex >= ExportPart.MaxNumberOfExcelSheetRows && (part.Data.Count > (totalItemIndex + 1)) ) {
					if( rowIndex >= maxRows && (part.Data.Count > (totalItemIndex + 1)) ) {
						if( part.AutoFitColumnWidths ) {
							try {
								string colRef = "A:" + (char)('A' + (part.Headers.Count - 1));
								cells[ colRef ].AutoFitColumns();
								//cells[ 0, 1, rowIndex, 1 ].Rows.AutoFit();
								//cells[ 0, 1, rowIndex, 1 ].AutoFit();
								//double oldWidth = cells[ 0, 1, rowIndex, 1 ].ColumnWidth;
								//try {
								//    cells[ 0, 1, rowIndex, 1 ].ColumnWidth += 10;
								//} catch {
								//    cells[ 0, 1, rowIndex, 1 ].ColumnWidth = oldWidth;
								//}
							} catch {
							}
						}
						sheetCount++;
						iSheet = iBook.Worksheets.Add();
						iSheet.Name = part.SheetName + " (" + sheetCount + ")";
						cells = iSheet.Cells;
						rowIndex = AddExcelHeaders( cells, part, colorHash, colorPalett, ref colorPalettIndex );
						rowSpans = new Hashtable();
						if( part.DataValidationLists.Count > 0 ) {
							foreach( ExportPart.DataValidationList dvl in part.DataValidationLists ) {
								dataValidationInfos.Add( new DataValidationInfo() {
									DVL = dvl, Sheet = iSheet, StartRow = rowIndex
								} );
							}
						}
					}
					totalItemIndex++;
				}
				if( part.AutoFitColumnWidths ) {
					try {
						string colRef = "A:" + (char)('A' + (part.Headers.Count - 1));
						cells[ colRef ].AutoFitColumns();
					} catch {
					}
				}
			}
			AddDataValidationLists( iBook, sheets );
			foreach( DataValidationInfo dvi in dataValidationInfos ) {
				foreach( int dvlColumn in dvi.DVL.Columns ) {
					if( dvi.DVL.Values.Count > 0 ) {
						dvi.Sheet.Cells[ dvi.StartRow, dvlColumn, maxRows, dvlColumn ].Validation.Add( ValidationType.List, ValidationAlertStyle.Warning, ValidationOperator.Equal, "=" + dvi.DVL.Name, null );
					}
				}
			}
			if( tempSheetName != null ) {
				try {
					if( iBook.Sheets[ tempSheetName ] != null ) {
						iBook.Sheets[ tempSheetName ].Delete();
					}
				} catch {
				}
			}
			if( iBook.Sheets.Count > 0 ) {
				iBook.Sheets[ 0 ].Select();
			}
			iBook.SaveToStream( streamToWriteOutputTo, format );
			streamToWriteOutputTo.Flush();
			iBook = null;
			workBook.Dispose();
			workBook = null;
			GC.Collect( 3 );
		}
		#endregion
		private class DataValidationInfo {
			public ExportPart.DataValidationList DVL;
			public int StartRow;
			public IWorksheet Sheet;
		}
		private static void AddDataValidationLists( IWorkbook iBook, List<ExportPart> sheets ) {
			//List<ExportPart.DataValidationList> dvLists = new List<ExportPart.DataValidationList>();
			Dictionary<string,ExportPart.DataValidationList> dvLists = new Dictionary<string, ExportPart.DataValidationList>();
			foreach( ExportPart ep in sheets ) {
				foreach(ExportPart.DataValidationList dvList in ep.DataValidationLists) {
					dvLists[ dvList.Name ] = dvList;
				}
			}
			if( dvLists.Count > 0 ) {
				IWorksheet iSheet = iBook.Worksheets.Add();
				iSheet.Name = "ValidationLists";
				int colIndex = 0;
				IRange cells = iSheet.Cells;
				foreach(KeyValuePair<string, ExportPart.DataValidationList> kvp in dvLists) {
					ExportPart.DataValidationList dvl = kvp.Value;
					int row = 0;
					cells[ row, colIndex ].Value = dvl.Name;
					cells[ row, colIndex ].AddComment( "Dessa värden används i listorna och skall därför användas vid en import" ).Visible = false;
					cells[ row, colIndex + 1].Value = "Visningsvärde " + dvl.Name;
					cells[ row, colIndex + 1 ].AddComment( "Dessa värden är visningsnamn och skall inte användas vid import" ).Visible = false;
					row++;
					foreach( ExportPart.DataValidationList.DataValidationValue dv in dvl.Values ) {
						cells[ row, colIndex ].NumberFormat = "@";
						cells[ row, colIndex ].Value = dv.Value;
						cells[ row, colIndex + 1 ].NumberFormat = "@";
						cells[ row, colIndex + 1 ].Value = dv.DisplayValue;
						row++;
					}
					if( dvl.Values.Count > 0 ) {
						iBook.Names.Add( dvl.Name, "=" + cells[ 1, colIndex, row - 1, colIndex ].Address ).Visible = true;
					}
					colIndex += 2;
				}
				cells[ 0, 0, 0, colIndex + 1 ].Font.Bold = true;
				cells[ 0, 0, 0, colIndex + 1 ].AutoFitColumns();
			}

		}
		private static Color MakePalettValid( Color color ) {
			return color.A != 255 ? Color.FromArgb( 255, color ) : color;
		}

		#region private static bool CheckIfRowSpanAndDecrease( this Hashtable ths, int cellIndex )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ths"></param>
		/// <param name="cellIndex"></param>
		/// <returns></returns>
		private static bool CheckIfRowSpanAndDecrease( this Hashtable ths, int cellIndex ) {
			if( !ths.ContainsKey( cellIndex ) ) {
				return false;
			}
			int span = (int)ths[ cellIndex ];
			span--;
			if( span <= 1 ) {
				ths.Remove( cellIndex );
				return false;
			}
			ths[ cellIndex ] = span;
			return true;
		}
		#endregion

		//#region public static void ExportSearchResult( Stream streamToWriteOutputTo, StationList stations, List<IStationProgramParameterSearchFilter> searchConditions )
		///// <summary>
		///// 
		///// </summary>
		///// <param name="streamToWriteOutputTo"></param>
		///// <param name="stations"></param>
		///// <param name="searchConditions"></param>
		//public static void ExportSearchResult( Stream streamToWriteOutputTo, StationList stations, List<IStationProgramParameterSearchFilter> searchConditions ) {
		//    string serverURL = ServerURL;
		//    IWorkbookSet workBook = Factory.GetWorkbookSet();
		//    IWorkbook iBook = workBook.Workbooks.Add();
		//    IWorksheet iSheet = iBook.Worksheets.Add();
		//    iSheet.Name = "Station-Program-Parameter";
		//    IRange cells = iSheet.Cells;
		//    int cellIndex = 0;
		//    cells[ 0, cellIndex++ ].Value = "Namn";

		//    cells[0, cellIndex++].Value = "EU_CD";
		//    cells[ 0, cellIndex++ ].Value = "Vattenkategori";
		//    cells[ 0, cellIndex++ ].Value = "Distrikt";
		//    cells[ 0, cellIndex++ ].Value = "Huvudavrinningsområde";
		//    cells[ 0, cellIndex++ ].Value = "Län";
		//    cells[ 0, cellIndex++ ].Value = "Kommun";

		//    cells[ 0, cellIndex++ ].Value = "Program";
		//    cells[ 0, cellIndex++ ].Value = "Datum";
		//    cells[ 0, cellIndex++ ].Value = "Frekvens";
		//    cells[ 0, cellIndex++ ].Value = "Motivering";
		//    cells[ 0, 0, 0, cellIndex ].Font.Bold = true;
		//    int rowIndex = 1;
		//    if( stations != null ) {
		//        for( int i = 0; i < stations.Count; i++ ) {
		//            Station station = stations[ i ];
		//            StationProgramParameterClassificationList sppcl = SearchControl.GetLatestStationProgramParameterClassificaitons( station, searchConditions );
		//            //cells[ rowIndex, 0 ].Value = station.DisplayName;
		//            cellIndex = 0;
		//            if( serverURL != null ) {
		//                cells[ rowIndex, cellIndex++ ].Hyperlinks.Add( cells[ rowIndex, 0 ], serverURL + "Stations.aspx?" + Constants.STATION_EUID + "=" + station.EUID, "", "Klicka här för att öppna stationen i VISS", station.DisplayName );
		//            } else {
		//                cells[ rowIndex, cellIndex++ ].Value = station.DisplayName;
		//            }
		//            cells[ rowIndex, cellIndex++ ].Value = station.EUID;
		//            cells[ rowIndex, cellIndex++ ].Value = station.WaterType != null ? station.WaterType.TranslatedName : "";
		//            cells[ rowIndex, cellIndex++ ].Value = station.Area != null ? station.Area.TranslatedName : "";
		//            cells[ rowIndex, cellIndex++ ].Value = station.Basin != null ? station.Basin.TranslatedName : "";
		//            cells[ rowIndex, cellIndex++ ].Value = station.Counties != null ? station.Counties.ToString( ", " ) : "";
		//            cells[ rowIndex, cellIndex++ ].Value = station.Municipality != null ? station.Municipality.Name : "";
		//            if( sppcl.Count > 1 ) {
		//                cells[ rowIndex, 0, rowIndex + sppcl.Count - 1, 0 ].Merge();
		//                cells[ rowIndex, 0, rowIndex + sppcl.Count - 1, 0 ].VerticalAlignment = VAlign.Center;
		//            }
		//            for( int j = 0; j < sppcl.Count; j++ ) {
		//                if( sppcl[ j ].Program != null ) {
		//                    if( serverURL != null ) {
		//                        cells[ rowIndex, cellIndex + 1 ].Hyperlinks.Add( cells[ rowIndex, 1 ], serverURL + "Programs.aspx?" + Constants.PROGRAM_ID + "=" + sppcl[ j ].ProgramID, "", "Klicka här för att öppna programmet i VISS", sppcl[ j ].Program.TranslatedName );
		//                    } else {
		//                        cells[ rowIndex, cellIndex + 1 ].Value = sppcl[ j ].Program.TranslatedName;
		//                    }
		//                } else {
		//                    cells[ rowIndex, cellIndex + 1 ].Value = "";
		//                }
		//                cells[ rowIndex, cellIndex + 2 ].Value = DataFactory.Instance.FormatDate( sppcl[ j ].ClassificationDate );
		//                cells[ rowIndex, cellIndex + 3 ].Value = sppcl[ j ].Frequency != null ? sppcl[ j ].Frequency.TranslatedName : "";
		//                cells[ rowIndex, cellIndex + 4 ].Value = sppcl[ j ].ClassificationParameterText ?? "";
		//                rowIndex++;
		//            }
		//        }
		//    }
		//    try {
		//        if( iBook.Sheets[ "Sheet1" ] != null ) {
		//            iBook.Sheets[ "Sheet1" ].Delete();
		//        }
		//    } catch {
		//    }
		//    iBook.SaveToStream( streamToWriteOutputTo, FileFormat.Excel8 );
		//}
		//#endregion

		#region private static int AddExcelHeaders( IRange cells, ExportPart part, Hashtable colorHash, IColors colorPalett, ref int colorPalettIndex )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cells"></param>
		/// <param name="part"></param>
		/// <param name="colorHash"></param>
		/// <param name="colorPalett"></param>
		/// <param name="colorPalettIndex"></param>
		/// <returns>Rowindex for the next row that does not contain data</returns>
		private static int AddExcelHeaders( IRange cells, ExportPart part, Hashtable colorHash, IColors colorPalett, ref int colorPalettIndex ) {
			int rowIndex = 0;
			List<CellDataList> headers = new List<CellDataList>();
			headers.Add( part.Headers );
			if( part.AdditionalHeaders != null && part.AdditionalHeaders.Count > 0 ) {
				foreach( CellDataList list in part.AdditionalHeaders ) {
					headers.Add( list );
				}
			}
			Hashtable rowSpans = new Hashtable();
			foreach( CellDataList headerList in headers ) {
				int cellIndex = 0;
				foreach( CellData data in headerList ) {
					if( rowSpans.CheckIfRowSpanAndDecrease( cellIndex ) ) {
						continue;
					}
					if( data.URL != null ) {
						cells[ rowIndex, cellIndex ].Hyperlinks.Add( cells[ rowIndex, cellIndex ], data.URL, "", data.URLTip, data.Value );
					}
					if( data.TextFormat ) {
						cells[ rowIndex, cellIndex ].NumberFormat = "@";
					}
					if( data.TextDirectionDegrees > 0 ) {
						cells[ rowIndex, cellIndex ].Orientation = data.TextDirectionDegrees;
					}
					if( data.BackgroundColor != Color.Transparent ) {
						//SpreadsheetGear.Drawing.Color c = SpreadsheetGear.Drawing.Color.FromArgb( data.BackgroundColor.A, data.BackgroundColor.R, data.BackgroundColor.G, data.BackgroundColor.B );
						//cells[ rowIndex, cellIndex ].Interior.Color = c;
						if( colorHash != null && !colorHash.ContainsKey( data.BackgroundColor.ToArgb() ) && colorPalettIndex < 56 ) {
							colorHash[ data.BackgroundColor.ToArgb() ] = null;
							colorPalett[ colorPalettIndex++ ] = MakePalettValid( data.BackgroundColor );
						}
						cells[ rowIndex, cellIndex ].Interior.Color = data.BackgroundColor;
						//data.Value = "Color.FromArgb( {0}, {1}, {2} )".FillBlanks( data.BackgroundColor.R, data.BackgroundColor.G, data.BackgroundColor.B );
					}
					if( data.FontColor != SystemColors.WindowText ) {
						if( colorHash != null && !colorHash.ContainsKey( data.FontColor.ToArgb() ) && colorPalettIndex < 56 ) {
							colorHash[ data.FontColor.ToArgb() ] = null;
							colorPalett[ colorPalettIndex++ ] = MakePalettValid( data.FontColor );
						}
						cells[ rowIndex, cellIndex ].Font.Color = data.FontColor;
					}
					if( rowIndex == 0 || data.Bold ) {
						cells[ rowIndex, cellIndex ].Font.Bold = true;
					}
					if( data.Underline.HasValue ) {
						cells[ rowIndex, cellIndex ].Font.Underline = data.Underline.Value ? UnderlineStyle.Single :UnderlineStyle.None;
					}
					if( data.CenterInCell ) {
						cells[ rowIndex, cellIndex ].Style.HorizontalAlignment = HAlign.Center;
						cells[ rowIndex, cellIndex ].Style.VerticalAlignment = VAlign.Center;
					}
					if( data.FontSize > 0f ) {
						cells[ rowIndex, cellIndex ].Font.Size = data.FontSize;
					}
					if( data.LeftBorder ) {
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeLeft ].Weight = BorderWeight.Thin;
					}
					if( data.LeftBorderColor != Color.Transparent ) {
						if( colorHash != null && !colorHash.ContainsKey( data.LeftBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
							colorHash[ data.LeftBorderColor.ToArgb() ] = null;
							colorPalett[ colorPalettIndex++ ] = MakePalettValid( data.LeftBorderColor );
						}
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeLeft ].Color = data.LeftBorderColor;
					}
					if( data.RightBorder ) {
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeRight ].Weight = BorderWeight.Thin;
					}
					if( data.RightBorderColor != Color.Transparent ) {
						if( colorHash != null && !colorHash.ContainsKey( data.RightBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
							colorHash[ data.RightBorderColor.ToArgb() ] = null;
							colorPalett[ colorPalettIndex++ ] = MakePalettValid( data.RightBorderColor );
						}
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeRight ].Color = data.RightBorderColor;
					}
					if( data.TopBorder ) {
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeTop ].Weight = BorderWeight.Thin;
					}
					if( data.TopBorderColor != Color.Transparent ) {
						if( colorHash != null && !colorHash.ContainsKey( data.TopBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
							colorHash[ data.TopBorderColor.ToArgb() ] = null;
							colorPalett[ colorPalettIndex++ ] = MakePalettValid( data.TopBorderColor );
						}
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeTop ].Color = data.TopBorderColor;
					}
					if( data.BottomBorder ) {
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeBottom ].Weight = BorderWeight.Thin;
					}
					if( data.BottomBorderColor != Color.Transparent ) {
						if( colorHash != null && !colorHash.ContainsKey( data.BottomBorderColor.ToArgb() ) && colorPalettIndex < 56 ) {
							colorHash[ data.BottomBorderColor.ToArgb() ] = null;
							colorPalett[ colorPalettIndex++ ] = MakePalettValid( data.BottomBorderColor );
						}
						cells[ rowIndex, cellIndex ].Borders[ BordersIndex.EdgeBottom ].Color = data.BottomBorderColor;
					}
					if( !string.IsNullOrEmpty( data.Comment ) ) {
						cells[ rowIndex, cellIndex ].AddComment( data.Comment ).Visible = false;
					}
					cells[ rowIndex, cellIndex ].WrapText = data.WrapText;
					cells[ rowIndex, cellIndex ].Value = data.Value;
					if( data.ColumnWidth > 0 ) {
						cells[ rowIndex, cellIndex ].ColumnWidth = data.ColumnWidth;
					}
					if( data.RowHeight > 0 ) {
						cells[ rowIndex, cellIndex ].RowHeight = data.RowHeight;
					}
					if( data.ColSpan > 1 ) {
						cells[ rowIndex, cellIndex, rowIndex, cellIndex + data.ColSpan - 1 ].Merge();
					}
					if( data.RowSpan > 1 ) {
						cells[ rowIndex, cellIndex, rowIndex + data.RowSpan - 1, cellIndex ].Merge();
						rowSpans[ cellIndex ] = data.RowSpan;
					}
					if( data.ColSpan > 1 ) {
						cellIndex += data.ColSpan;
					} else {
						cellIndex++;
					}
				}
				rowIndex++;
			}
			//cells[ 0, 0, 0, cellIndex ].Font.Bold = true;
			return rowIndex;
		}
		#endregion


		#region private static SheetList FixSheetNames( List<ExportPart> sheets )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sheets"></param>
		/// <returns></returns>
		private static SheetList FixSheetNames( List<ExportPart> sheets ) {
			return SheetList.FromList( sheets );
		}
		#endregion
		#region public static void AutoFitColumns( this IRange cells )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cells"></param>
		public static void AutoFitColumns( this IRange cells ) {
			cells.Columns.AutoFit();
			try {
				foreach( IRange column in cells.Columns ) {
					column.ColumnWidth = (column.ColumnWidth + 2).ValueOrMax( 255 );
				}
				//cells[ 0, 1, rowIndex, 1 ].ColumnWidth += 10;
			} catch {
			}
		}
		#endregion

		/// <summary>
		/// <see cref="List{T}"/> subclass that contain uniquely named excel sheets.
		/// </summary>
		private class SheetList : List<ExportPart> {
			#region public void AddUnique( ExportPart sheet )
			/// <summary>
			/// Adds a new sheet to the list and appends an index to the sheet name if a sheet
			/// with that name already exist in the list.
			/// </summary>
			/// <param name="sheet">The list to add.</param>
			private void AddUnique(ExportPart sheet) {
				int index = 0;
				string compareTo = CreateValidSheetName(sheet.SheetName, index);
				while (_sheetNames.Contains(compareTo)) {
					compareTo = CreateValidSheetName( sheet.SheetName, ++index );
				}
				sheet.SheetName = compareTo;
				Add( sheet );
				_sheetNames.Add( sheet.SheetName );
			}
			private StringCollection _sheetNames = new StringCollection();
			#endregion

			#region private string CreateValidSheetName( string name, int index )
			/// <summary>
			/// Creates a valid Excel sheet name of the specified name and index.
			/// </summary>
			/// <param name="name">The sheet name to validate.</param>
			/// <param name="index">The index to append if the name has already been used.</param>
			/// <returns>A valid Excel sheet name based on the specified name.</returns>
			private string CreateValidSheetName(string name, int index) {
				string newName = name;
				char[] invalidExcelSheetNameChars = new[] {
					':', '\\', '/', '?', '*', '[', ']'
				};

				if (name.IndexOfAny(invalidExcelSheetNameChars) >= 0) {
					const string invalidExcelSheetNameCharClass = @"[:\\/?*\[\]]";
					newName = Regex.Replace( name, invalidExcelSheetNameCharClass, "_" );
				}

				// Max len of Excel sheet name.
				const int maxLen = 31;
				bool appendIndex = (index > 0);
				bool isLong = (newName.Length > maxLen);
				
				if ( appendIndex && isLong ) {
					int indexLen = index.ToString().Length;
					newName = newName.Substring( 0, maxLen - indexLen - 1 ) + '~' + index;
				} else if ( appendIndex && !isLong ) {
					newName += index;
				} else if ( !appendIndex && isLong ) {
					newName = newName.Substring( 0, maxLen );
				}

				return newName;
			}
			#endregion

			#region public static SheetList FromList( List<ExportPart> sheets )
			/// <summary>
			/// Creates a new <see cref="SheetList"/> instance from a generic list of 
			/// <see cref="ExportPart"/>s.
			/// </summary>
			/// <param name="sheets">The sheets to add.</param>
			/// <returns>A new <see cref="SheetList"/>.</returns>
			public static SheetList FromList(List<ExportPart> sheets) {
				SheetList sheetList = new SheetList();
				foreach (ExportPart part in sheets) {
					sheetList.AddUnique(part);
				}
				return sheetList;
			}
			#endregion
		}
	}
}
