using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HomeFinanceServer.Data {
	/// <summary>
	/// This is a utility class for working with databases.
	/// It handles connections, executes queries/updates, and, most importantly, encapsulates the cleaning up of all resources (such as closing connections and readers), in an automated way.
	/// </summary>
	/// <remarks></remarks>
	/// <example></example>
	public class DBCommand : IDisposable {
		private bool isDisposed;
		private SqlCommand cmd;
		private const string transactionName = "myOwnTransaction";
		private bool isPrepared;
		private List<SqlDataReader> readers = new List<SqlDataReader>();
		private SqlDataReader currentReader;
		#region public static int TotalExecutions
		/// <summary>
		/// Gets the TotalExecutions of the DBCommand
		/// </summary>
		/// <value></value>
		public static int TotalExecutions {
			get {
				return _totalExecutions;
			}
		}
		private static int _totalExecutions;
		#endregion
		#region public static string CommandLogFile
		/// <summary>
		/// Get/Sets the CommandLogFile of the DBCommand
		/// If set, each command executed will be written to the specified file.
		/// <remarks>Setting this will severely effect the performance toward the database</remarks>
		/// </summary>
		/// <value></value>
		public static string CommandLogFile {
			get { return _commandLogFile; }
			set { _commandLogFile = value; }
		}
		private static string _commandLogFile;
		#endregion
		#region public string CurrentCommandLogFile
		/// <summary>
		/// Get/Sets the CurrentCommandLogFile of the DBCommand
		/// </summary>
		/// <value></value>
		public string CurrentCommandLogFile {
			get {
				return _currentCommandLogFile;
			}
			set {
				_currentCommandLogFile = value;
			}
		}
		private string _currentCommandLogFile;
		#endregion
		#region public static List<string> SkipCommandLogFilters
		/// <summary>
		/// Gets the SkipCommandLogFilters of the DBCommand
		/// Procedure calls starting with any of these filters will not be logged
		/// </summary>
		/// <value></value>
		public static List<string> SkipCommandLogFilters {
			get {
				return _skipCommandLogFilters;
			}
		}
		private static List<string> _skipCommandLogFilters = new List<string>();
		#endregion

		#region private static string ConnStr
		/// <summary>
		/// Gets the ConnStr of the DBCommand
		/// </summary>
		/// <value></value>
		private static string ConnStr {
			get {
				return _connStr ?? (_connStr = ConfigurationManager.ConnectionStrings[ "DBConnection" ].ConnectionString);
			}
		}
		private static string _connStr;
		#endregion
		#region internal static DBCommand New
		/// <summary>
		/// Gets the New of the DBCommand
		/// </summary>
		/// <value></value>
		internal static DBCommand New {
			get {
				return new DBCommand( new SqlConnection( ConnStr ), CommandType.StoredProcedure );
			}
		}
		#endregion

		#region public SqlConnection Con
		/// <summary>
		/// Gets the Con of the DBCommand
		/// </summary>
		/// <value></value>
		public SqlConnection Con {
			get {
				return _con;
			}
		}
		private SqlConnection _con;
		#endregion
		#region public string CommandText
		/// <summary>
		/// Get/Sets the CommandText of the DBCommand
		/// </summary>
		/// <value></value>
		public string CommandText {
			get {
				return cmd.CommandText;
			}
			set {
				cmd.CommandText = value;
			}
		}
		#endregion
		#region public int ConnectionTimeout
		/// <summary>
		/// Get/Sets the ConnectionTimeout of the DBCommand
		/// </summary>
		/// <value></value>
		public int ConnectionTimeout {
			get {
				return Con.ConnectionTimeout;
			}
			set {
				SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder( Con.ConnectionString );
				csb.ConnectTimeout = value;
				Con.ConnectionString = csb.ToString();
			}
		}
		#endregion
		#region public int CommandTimeout
		/// <summary>
		/// Get/Sets the CommandTimeout of the DBCommand
		/// </summary>
		/// <value></value>
		public int CommandTimeout {
			get {
				return cmd.CommandTimeout;
			}
			set {
				cmd.CommandTimeout = value;
			}
		}
		#endregion
		#region public bool UseTransaction
		/// <summary>
		/// Get/Sets the UseTransaction of the DBCommand
		/// </summary>
		/// <value></value>
		public bool UseTransaction {
			get {
				return _useTransaction;
			}
			set {
				if( !isPrepared ) {
					_useTransaction = value;
				} else {
					throw new ReadOnlyException( "UseTransaction can not be changed once a command has been executed" );
				}
			}
		}
		private bool _useTransaction;
		#endregion
		#region public CommandType CommandType
		/// <summary>
		/// Get/Sets the CommandType of the DBCommand
		/// </summary>
		/// <value></value>
		public CommandType CommandType {
			get {
				return cmd.CommandType;
			}
			set {
				cmd.CommandType = value;
			}
		}
		#endregion
		#region private SqlTransaction Transaction
		/// <summary>
		/// Gets the Transaction of the DBCommand
		/// </summary>
		/// <value></value>
		private SqlTransaction Transaction {
			get {
				return _transaction ?? (_transaction = Con.BeginTransaction( IsolationLevel.Snapshot, transactionName ));
			}
		}
		private SqlTransaction _transaction;
		#endregion
		#region public object this[ string columnName ]
		/// <summary>
		/// Gets the <see cref="Object"/> item identified by the given arguments of the DBCommand
		/// </summary>
		/// <value></value>
		public object this[ string columnName ] {
			get {
				return currentReader[ columnName ];
			}
		}
		#endregion
		#region public object this[ int column ]
		/// <summary>
		/// Gets the <see cref="Object"/> item identified by the given arguments of the DBCommand
		/// </summary>
		/// <value></value>
		public object this[ int column ] {
			get {
				return currentReader[ column ];
			}
		}
		#endregion

		#region public DBCommand( SqlConnection con, CommandType commandType )
		/// <summary>
		/// Initializes a new instance of the <b>DBCommand</b> class.
		/// </summary>
		/// <param name="con"></param>
		/// <param name="commandType"></param>
		public DBCommand( SqlConnection con, CommandType commandType )
			: this( con, commandType, null ) {
		}
		#endregion
		#region public DBCommand( SqlConnection con, CommandType commandType, string commandText )
		/// <summary>
		/// Initializes a new instance of the <b>DBCommand</b> class.
		/// </summary>
		/// <param name="con"></param>
		/// <param name="commandType"></param>
		/// <param name="commandText"></param>
		public DBCommand( SqlConnection con, CommandType commandType, string commandText ) {
			_con = con;
			cmd = _con.CreateCommand();
			cmd.CommandType = commandType;
			cmd.CommandText = commandText;
		}
		#endregion
		#region ~DBCommand()
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before 
		/// the <b>DBCommand</b> is reclaimed by garbage collection.
		/// </summary>
		~DBCommand() {
			Close();
		}
		#endregion
		#region public void Dispose()
		/// <summary>
		/// Releases the resources used by the <b>DBCommand</b>.
		/// </summary>
		public void Dispose() {
			Dispose( true );
			GC.SuppressFinalize( this );
		}
		#endregion
		#region private void Dispose( bool disposing )
		/// <summary>
		/// Releases the resources used by the <b>DBCommand</b>.
		/// </summary>
		/// <param name="disposing">Set to <b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
		private void Dispose( bool disposing ) {
			if( disposing ) {
				Close();
			}
		}
		#endregion

		#region public void SetValues( params Tuple<string,object >[] nameValues )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nameValues"></param>
		public void SetValues( params Tuple<string, object>[] nameValues ) {
			foreach( Tuple<string, object> nameValue in nameValues ) {
				cmd.Parameters[ nameValue.Item1 ].Value = nameValue.Item2;
			}
		}
		#endregion
		#region public SqlParameterCollection Add( params SqlParameter[] parameters )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public SqlParameterCollection Add( params SqlParameter[] parameters ) {
			return AddRange( parameters );
		}
		#endregion
		#region public SqlParameterCollection AddRange( IEnumerable<SqlParameter> parameters )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public SqlParameterCollection AddRange( IEnumerable<SqlParameter> parameters ) {
			foreach( SqlParameter sqlParameter in parameters ) {
				cmd.Parameters.Add( sqlParameter );
			}
			return cmd.Parameters;
		}
		#endregion
		#region public SqlParameter Add( SqlParameter parameter )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public SqlParameter Add( SqlParameter parameter ) {
			cmd.Parameters.Add( parameter );
			return parameter;
		}
		#endregion
		#region public SqlParameter Add( string parameterName, SqlDbType sqlType, int size )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="sqlType"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public SqlParameter Add( string parameterName, SqlDbType sqlType, int size ) {
			return cmd.Parameters.Add( parameterName, sqlType, size );
		}
		#endregion
		#region public SqlParameter Add( string parameterName, SqlDbType sqlType )
		/// <summary>
		/// Adds a parameter to the command
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="sqlType"></param>
		/// <returns></returns>
		public SqlParameter Add( string parameterName, SqlDbType sqlType ) {
			return Add( parameterName, sqlType, ParameterDirection.Input );
		}
		#endregion
		#region public SqlParameter Add( string parameterName, SqlDbType sqlType, ParameterDirection direction )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="sqlType"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public SqlParameter Add( string parameterName, SqlDbType sqlType, ParameterDirection direction ) {
			return Add( parameterName, sqlType, direction, null );
		}
		#endregion
		#region public SqlParameter Add( string parameterName, SqlDbType sqlType, ParameterDirection direction, object parameterValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="sqlType"></param>
		/// <param name="direction"></param>
		/// <param name="parameterValue"></param>
		/// <returns></returns>
		public SqlParameter Add( string parameterName, SqlDbType sqlType, ParameterDirection direction, object parameterValue ) {
			SqlParameter param = cmd.Parameters.Add( parameterName, sqlType );
			param.Direction = direction;
			param.Value = parameterValue;
			return param;
		}
		#endregion
		#region public SqlParameter AddWithValue( string parameterName, object parameterValue )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="parameterValue"></param>
		/// <returns></returns>
		public SqlParameter AddWithValue( string parameterName, object parameterValue ) {
			if (parameterValue == null) parameterValue = DBNull.Value;
			return cmd.Parameters.AddWithValue( parameterName, parameterValue );
		}
		#endregion
		#region public SqlParameter AddWithValue( string parameterName, object parameterValue, SqlDbType sqlType )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="parameterValue"></param>
		/// <param name="sqlType"></param>
		/// <returns></returns>
		public SqlParameter AddWithValue( string parameterName, object parameterValue, SqlDbType sqlType ) {
			if( parameterValue == null ) parameterValue = DBNull.Value;
			SqlParameter p = cmd.Parameters.AddWithValue( parameterName, parameterValue );
			p.SqlDbType = sqlType;
			return p;
		}
		#endregion
		#region public SqlParameter AddWithValue( string parameterName, object parameterValue, SqlDbType sqlType, ParameterDirection direction )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="parameterValue"></param>
		/// <param name="sqlType"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public SqlParameter AddWithValue( string parameterName, object parameterValue, SqlDbType sqlType, ParameterDirection direction ) {
			if( parameterValue == null ) parameterValue = DBNull.Value;
			SqlParameter p = cmd.Parameters.AddWithValue( parameterName, parameterValue );
			p.SqlDbType = sqlType;
			p.Direction = direction;
			return p;
		}
		#endregion
		#region public void RemoveParameter( SqlParameter parameter )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameter"></param>
		public void RemoveParameter( SqlParameter parameter ) {
			cmd.Parameters.Remove( parameter );
		}
		#endregion
		#region public void Commit()
		/// <summary>
		/// 
		/// </summary>
		public void Commit() {
			if( UseTransaction ) {
				Transaction.Commit();
			}
		}
		#endregion
		#region public void Rollback()
		/// <summary>
		/// 
		/// </summary>
		public void Rollback() {
			if( UseTransaction ) {
				Transaction.Rollback( transactionName );
			}
		}
		#endregion
		#region public bool Read()
		/// <summary>
		/// Returns the result of a call to the Read-method of the current <see cref="SqlDataReader"/>
		/// If no <see cref="SqlDataReader"/> is available, a call to <see cref="ExecuteReader"/> is performed first
		/// </summary>
		/// <returns></returns>
		public bool Read() {
			if( currentReader == null ) {
				ExecuteReader();
				if( currentReader == null ) {
					return false;
				}
			}
			return currentReader.Read();
		}
		#endregion
		#region public bool NextResult()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool NextResult() {
			return currentReader.NextResult();
		}
		#endregion
		#region public void WhileRead(string errorIdentifier, bool closeWhenFinished,Action action )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="errorIdentifier"></param>
		/// <param name="closeWhenFinished"></param>
		/// <param name="action"></param>
		/// <exception cref="Exception"></exception>
		public void WhileRead( string errorIdentifier, bool closeWhenFinished, Action action ) {
			try {
				while( Read() ) {
					action();
				}
			} catch( Exception ex ) {
				throw new Exception( ex.Message, ex );
			} finally {
				if( closeWhenFinished ) {
					Close();
				}
			}
		}
		#endregion

		#region public object GetValue( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public object GetValue( int column ) {
			return IsDBNull( column ) ? null : currentReader.GetValue( column );
		}
		#endregion
		#region public object GetValue( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public object GetValue( string columnName ) {
			return GetValue( GetOrdinal( columnName ) );
		}
		#endregion
		#region public byte GetTinyInt( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public byte GetTinyInt( int column ) {
			return GetTinyInt( column, 0 );
		}
		#endregion
		#region public byte GetTinyInt( int column, byte valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public byte GetTinyInt( int column, byte valueIfNull ) {
			return IsDBNull( column ) ? valueIfNull : currentReader.GetByte( column );
		}
		#endregion
		#region public byte GetTinyInt( string columnName, byte valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public byte GetTinyInt( string columnName, byte valueIfNull ) {
			return GetTinyInt( GetOrdinal( columnName ), valueIfNull );
		}
		#endregion
		#region public byte GetTinyInt( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public byte GetTinyInt( string columnName ) {
			return GetTinyInt( GetOrdinal( columnName ) );
		}
		#endregion
		#region public int GetInt( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public int GetInt( int column ) {
			return GetInt( column, 0 );
		}
		#endregion
		#region public int GetInt( int column, int valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public int GetInt( int column, int valueIfNull ) {
			return currentReader.IsDBNull( column ) ? valueIfNull : currentReader.GetInt32( column );
		}
		#endregion
		#region public int GetInt( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public int GetInt( string columnName ) {
			return GetInt( currentReader.GetOrdinal( columnName ) );
		}
		#endregion
		#region public int GetInt( string columnName, int valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public int GetInt( string columnName, int valueIfNull ) {
			return GetInt( currentReader.GetOrdinal( columnName ), valueIfNull );
		}
		#endregion
		#region public int? GetNullableInt( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public int? GetNullableInt( int column ) {
			if( IsDBNull( column ) ) {
				return null;
			}
			return GetInt( column );
		}
		#endregion
		#region public int? GetNullableInt( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public int? GetNullableInt( string columnName ) {
			return GetNullableInt( GetOrdinal( columnName ) );
		}
		#endregion
		#region public int GetInt32( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public int GetInt32( int column ) {
			return GetInt( column, 0 );
		}
		#endregion
		#region public int GetInt32( int column, int valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public int GetInt32( int column, int valueIfNull ) {
			return currentReader.IsDBNull( column ) ? valueIfNull : currentReader.GetInt32( column );
		}
		#endregion
		#region public int GetInt32( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public int GetInt32( string columnName ) {
			return GetInt( currentReader.GetOrdinal( columnName ) );
		}
		#endregion
		#region public int GetInt32( string columnName, int valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public int GetInt32( string columnName, int valueIfNull ) {
			return GetInt( currentReader.GetOrdinal( columnName ), valueIfNull );
		}
		#endregion
		#region public long GetLong( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public long GetLong( int column ) {
			return GetLong( column, 0 );
		}
		#endregion
		#region public long GetLong( int column, long valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public long GetLong( int column, long valueIfNull ) {
			return currentReader.IsDBNull( column ) ? valueIfNull : currentReader.GetInt64( column );
		}
		#endregion
		#region public long GetLong( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public long GetLong( string columnName ) {
			return GetLong( columnName, 0 );
		}
		#endregion
		#region public long GetLong( string columnName, long valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public long GetLong( string columnName, long valueIfNull ) {
			return GetLong( currentReader.GetOrdinal( columnName ), valueIfNull );
		}
		#endregion
		#region public string GetString( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetString( int column ) {
			return GetString( column, null );
		}
		#endregion
		#region public string GetString( int column, string valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public string GetString( int column, string valueIfNull ) {
			return currentReader.IsDBNull( column ) ? valueIfNull : currentReader.GetString( column );
		}
		#endregion
		#region public bool GetBoolean( int column, bool valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public bool GetBoolean( int column, bool valueIfNull ) {
			return currentReader.IsDBNull( column ) ? valueIfNull : currentReader.GetBoolean( column );
		}
		#endregion
		#region public bool GetBoolean( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public bool GetBoolean( int column ) {
			return GetBoolean( column, false );
		}
		#endregion
		#region public bool GetBoolean( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public bool GetBoolean( string columnName ) {
			return GetBoolean( columnName, false );
		}
		#endregion
		#region public bool GetBoolean( string columnName, bool valueIfFalse )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfFalse"></param>
		/// <returns></returns>
		public bool GetBoolean( string columnName, bool valueIfFalse ) {
			return GetBoolean( currentReader.GetOrdinal( columnName ), valueIfFalse );
		}
		#endregion
		#region public bool GetBit( int column, bool valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public bool GetBit( int column, bool valueIfNull ) {
			return GetBoolean( column, valueIfNull );
		}
		#endregion
		#region public bool GetBit( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public bool GetBit( int column ) {
			return GetBoolean( column );
		}
		#endregion
		#region public bool GetBit( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public bool GetBit( string columnName ) {
			return GetBoolean( columnName );
		}
		#endregion
		#region public bool GetBit( string columnName, bool valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public bool GetBit( string columnName, bool valueIfNull ) {
			return GetBoolean( columnName, valueIfNull );
		}
		#endregion
		#region public double GetDouble( int column, double valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public double GetDouble( int column, double valueIfNull ) {
			return currentReader.IsDBNull( column ) ? valueIfNull : currentReader.GetDouble( column );
		}
		#endregion
		#region public double GetDouble( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public double GetDouble( int column ) {
			return GetDouble( column, 0 );
		}
		#endregion
		#region public double GetDouble( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public double GetDouble( string columnName ) {
			return GetDouble( columnName, 0 );
		}
		#endregion
		#region public decimal GetDecimal( int column, decimal valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public decimal GetDecimal( int column, decimal valueIfNull ) {
			return IsDBNull( column ) ? valueIfNull : currentReader.GetDecimal( column );
		}
		#endregion
		#region public decimal GetDecimal( string columnName, decimal valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public decimal GetDecimal( string columnName, decimal valueIfNull ) {
			return GetDecimal( GetOrdinal( columnName ), valueIfNull );
		}
		#endregion
		#region public decimal GetDecimal( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public decimal GetDecimal( string columnName ) {
			return GetDecimal( GetOrdinal( columnName ), 0 );
		}
		#endregion
		#region public double GetDouble( string columnName, double valueIFNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIFNull"></param>
		/// <returns></returns>
		public double GetDouble( string columnName, double valueIFNull ) {
			return GetDouble( currentReader.GetOrdinal( columnName ), valueIFNull );
		}
		#endregion
		#region public double? GetNullableDouble( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public double? GetNullableDouble( int column ) {
			if( IsDBNull( column ) ) {
				return null;
			}
			return currentReader.GetDouble( column );
		}
		#endregion
		#region public double? GetNullableDouble( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public double? GetNullableDouble( string columnName ) {
			return GetNullableDouble( GetOrdinal( columnName ) );
		}
		#endregion
		#region public DateTime GetDateTime( int column, DateTime valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public DateTime GetDateTime( int column, DateTime valueIfNull ) {
			return currentReader.IsDBNull( column ) ? valueIfNull : currentReader.GetDateTime( column );
		}
		#endregion
		#region public DateTime GetDateTime( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public DateTime GetDateTime( int column ) {
			return GetDateTime( column, DateTime.MinValue );
		}
		#endregion
		#region public DateTime GetDateTime( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public DateTime GetDateTime( string columnName ) {
			return GetDateTime( columnName, DateTime.MinValue );
		}
		#endregion
		#region public DateTime GetDateTime( string columnName, DateTime valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public DateTime GetDateTime( string columnName, DateTime valueIfNull ) {
			return GetDateTime( GetOrdinal( columnName ), valueIfNull );
		}
		#endregion
		#region public DateTime? GetNullableDateTime( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public DateTime? GetNullableDateTime( int column ) {
			if( IsDBNull( column ) ) {
				return null;
			}
			return currentReader.GetDateTime( column );
		}
		#endregion
		#region public DateTime? GetNullableDateTime( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public DateTime? GetNullableDateTime( string columnName ) {
			return GetNullableDateTime( GetOrdinal( columnName ) );
		}
		#endregion
		#region public object ExecuteScalar()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object ExecuteScalar() {
			PrepareExecution();
			if( readers.Count > 0 && !readers[ readers.Count - 1 ].IsClosed ) {
				try {
					readers[ readers.Count - 1 ].Close();
					readers[ readers.Count - 1 ].Dispose();
				} catch {
				}
			}
			return cmd.ExecuteScalar();
		}
		#endregion
		#region public string GetDataTypeName( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string GetDataTypeName( int column ) {
			return currentReader.GetDataTypeName( column );
		}
		#endregion

		#region public bool IsDBNull( int column )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <returns>True if d b is null, otherwise false.</returns>
		public bool IsDBNull( int column ) {
			return currentReader.IsDBNull( column );
		}
		#endregion
		#region public bool IsDBNull( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns>True if d b is null, otherwise false.</returns>
		public bool IsDBNull( string columnName ) {
			return IsDBNull( currentReader.GetOrdinal( columnName ) );
		}
		#endregion
		#region public int GetOrdinal( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public int GetOrdinal( string columnName ) {
			if( currentReader == null ) {
				ExecuteReader();
				if( currentReader == null ) {
					return -1;
				}
			}
			return currentReader.GetOrdinal( columnName );
		}
		#endregion

		#region public string GetString( string columnName, string valueIfNull )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
		public string GetString( string columnName, string valueIfNull ) {
			return GetString( currentReader.GetOrdinal( columnName ), valueIfNull );
		}
		#endregion
		#region public string GetString( string columnName )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public string GetString( string columnName ) {
			return GetString( columnName, null );
		}
		#endregion

		#region public int ExecuteNonQuery()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int ExecuteNonQuery() {
			PrepareExecution();
			if( readers.Count > 0 && !readers[ readers.Count - 1 ].IsClosed ) {
				try {
					readers[ readers.Count - 1 ].Close();
					readers[ readers.Count - 1 ].Dispose();
				} catch {
				}
			} 
			return cmd.ExecuteNonQuery();
		}
		#endregion
		#region public SqlDataReader ExecuteReader()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public SqlDataReader ExecuteReader() {
			PrepareExecution();
			if( readers.Count > 0 && !readers[ readers.Count - 1 ].IsClosed ) {
				try {
					readers[ readers.Count - 1 ].Close();
					readers[ readers.Count - 1 ].Dispose();
				} catch {}
			}
			SqlDataReader reader = cmd.ExecuteReader();
			readers.Add( reader );
			currentReader = reader;
			return reader;
		}
		#endregion
		#region private void PrepareExecution()
		/// <summary>
		/// Opens the connection if needed
		/// </summary>
		private void PrepareExecution() {
			if( !isPrepared ) {
				isPrepared = true;
				if(Con.State != ConnectionState.Open) {
					Con.Open();
				}
				if(UseTransaction && _transaction == null) {
					cmd.Transaction = Transaction;
				}
				try {
					_totalExecutions++;
				} catch{}
			}
			#region Command logging, if a log file is specified
			string commandLog = CurrentCommandLogFile ?? CommandLogFile;
			if( !string.IsNullOrEmpty( commandLog ) ) {
				if( SkipCommandLogFilters.Count > 0 ) {
					foreach(string filter in SkipCommandLogFilters) {
						if( !string.IsNullOrEmpty( filter ) ) {
							if( CommandText.ToLower().StartsWith( filter.ToLower() ) ) {
								return;
							}
						}
					}
				}
				lock( logFileLock ) {
					try {
						StringBuilder sb = new StringBuilder( CommandText );
						if(cmd.Parameters.Count > 0) {
							sb.Append( " " );
						}
						for(int i = 0; i < cmd.Parameters.Count; i++) {
							SqlParameter p = cmd.Parameters[ i ];
							sb.Append( p.ParameterName );
							sb.Append( " = " );
							if(p.Value == DBNull.Value || p.Value == null) {
								sb.Append( "NULL" );
							} else {
								bool addQuote = false;
								switch(p.SqlDbType) {
									case SqlDbType.Char:
									case SqlDbType.DateTime:
									case SqlDbType.NChar:
									case SqlDbType.NText:
									case SqlDbType.NVarChar:
									case SqlDbType.SmallDateTime:
									case SqlDbType.Text:
									case SqlDbType.VarChar:
									case SqlDbType.Variant:
									case SqlDbType.Xml:
									case SqlDbType.Date:
									case SqlDbType.Time:
									case SqlDbType.DateTime2:
										addQuote = true;
										break;
								}
								if(addQuote) {
									sb.Append( '\'' );
								}
								string value = p.Value.ToString();
								if( !addQuote && value.IndexOf( ',' ) > -1 ) {
									value = value.Replace( ',', '.' );
								}
								sb.Append( value );
								if(addQuote) {
									sb.Append( '\'' );
								}
							}
							if( (i + 1) < cmd.Parameters.Count ) {
								sb.Append( ", " );
							}
						}
						sb.Append( Environment.NewLine );
						sb.Append( "GO" );
						sb.Append( Environment.NewLine );
						System.IO.File.AppendAllText( commandLog, sb.ToString() );
					} catch(Exception) {
					}
				}
			}
			#endregion
		}
		private object logFileLock = new object();
		#endregion

		#region public void Close()
		/// <summary>
		/// Closes all connections and releases all resources
		/// </summary>
		public void Close() {
			if( isDisposed ) {
				return;
			}
			isDisposed = true;
			if( _transaction != null ) {
				_transaction.Dispose();
			}
			if( readers.Count > 0 ) {
				for( int i = 0; i < readers.Count; i++ ) {
					SqlDataReader reader = readers[ i ];
					try {
						if( !reader.IsClosed ) { // don't close if the user of this command has already closed it
							reader.Close();
							reader.Dispose();
						}
					} catch {
					}
				}
			}
			if( cmd != null ) {
				cmd.Dispose();
				cmd = null;
			}
			if( Con != null ) {
				try {
					Con.Close();
					Con.Dispose();
				} catch {
				}
				_con = null;
			}
		}
		#endregion

		#region internal void ClearParameters()
		/// <summary>
		/// 
		/// </summary>
		public void ClearParameters() {
			cmd.Parameters.Clear();
		}
		#endregion

		#region public static object QuickExecuteScalar( SqlConnection con, CommandType commandType, string command )
		/// <summary>
		/// 
		/// </summary>
		/// <param name="con"></param>
		/// <param name="commandType"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public static object QuickExecuteScalar( SqlConnection con, CommandType commandType, string command ) {
			DBCommand cmd = new DBCommand( con, commandType, command );
			try {
				return cmd.ExecuteScalar();
			} finally {
				cmd.Close();
			}
		}
		#endregion
		#region public static List<string> GetTableColumnNames( SqlConnection con, string tableName, bool addTableNameToFields )
		/// <summary>
		/// Returns a list of names for the columns in the specified database table.
		/// The names are ordered by ordinal position
		/// </summary>
		/// <param name="con">A <see cref="SqlConnection"/> to the database</param>
		/// <param name="tableName">Name of the table</param>
		/// <param name="addTableNameToFields">If true, the tableName is pre-pended to each column name</param>
		/// <returns>A <see cref="List<string>"/> with the names of the columns</returns>
		public static List<string> GetTableColumnNames( SqlConnection con, string tableName, bool addTableNameToFields ) {
			List<string> names = new List<string>();
			DBCommand c = new DBCommand( con, CommandType.Text );
			c.CommandText = @"SELECT column_name
FROM information_schema.columns
WHERE table_name = @table_name
ORDER BY ordinal_position";
			c.AddWithValue( "@table_name", tableName );
			try {
				while( c.Read() ) {
					names.Add( c.GetString( "COLUMN_NAME" ) );
				}
			} catch {
			} finally {
				c.Close();
			}
			return names;
		}
		#endregion

		public static object NullZero( string str ) {
			if( string.IsNullOrEmpty( str ) ) {
				return DBNull.Value;
			}
			return str;
		}
	}
}
