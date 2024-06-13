using System;

namespace AccountsModuleService
{
	#region Enums
	/// <summary>
	/// Enum for Selecting the Type of Config File - Web.Config (or) App.Config 
	/// </summary>
	public enum ConfigFileType
	{
		/// <summary>
		/// Sets the ConfigFileType to Application config
		/// </summary>
		APP_CONFIG,
		/// <summary>
		/// Sets the ConfigFileType to Web config
		/// </summary>
		WEB_CONFIG
	}
	
	/// <summary>
	/// Enum for Setting the Database Type (Oracle Or SQL Server)
	/// </summary>
	public enum DatabaseType
	{
		/// <summary>
		/// Sets the DatabaseType to Oracle
		/// </summary>
		ORACLE,
		/// <summary>
		/// Sets the DatabaseType to SQLSERVER
		/// </summary>
		SQLSERVER
	}
	

	#endregion Enums

	/// <summary>
	/// Class which contains global constants variables such as connection string, database type.
	/// </summary>
	public class Constants
	{
		#region Private Declarations
		/// <summary>
		/// Contains Global Connection String
		/// </summary>
		protected internal static string m_globalConnectionString	=	null;	
		/// <summary>
		/// Determines if the Global Connection String is avaialable 
		/// </summary>
		protected internal static bool m_isConnectionDeclared		=	false;	
		/// <summary>
		/// Contains Global Database Type
		/// </summary>
		protected internal static DatabaseType m_GlobalDatabaseType;
		/// <summary>
		/// If the Global Database Type is set then the Value is set to True
		/// </summary>
		protected internal static bool m_isDatabaseTypeSet		=	false;	
																			
		#endregion Private Declarations
		
		#region Constructors 
		/// <summary>
		/// Constructor for the Constants Class 
		/// Since it Contains only Static Methods a New Instance of the Class is not Required 
		/// So the Constuctor is set as private 
		/// </summary>
		private Constants()
		{

		}
		#endregion Constructors 

		#region Properties
		/// <summary>
		/// Gets or Sets the GlobalConnectionString 
		/// </summary>
		public static string GlobalConnectionstring
		{
			get 
			{
				return m_globalConnectionString ;
				
			}
			set
			{
				if (value	!=	string.Empty) 
				{
					m_globalConnectionString	=	(string)value;	
					m_isConnectionDeclared		=	true;
				}
			}
		}

		/// <summary>
		/// Returns True if the Value of the Global Database Connection String is Initialised 
		/// </summary>
		public static bool IsGlobalDatabaseConnection
		{
			get
			{
				return m_isConnectionDeclared;
			}
		}
							
		/// <summary>
		/// Gets or Sets the Value of the Global Database Type 
		/// This Property sets the DatabaseType for the Global Connection String Provided 
		/// </summary>
		public static DatabaseType GlobalConnectionType 
		{
			get
			{
				return m_GlobalDatabaseType;
			}

			set
			{
				m_GlobalDatabaseType	=	value;
				m_isDatabaseTypeSet		=	true;
			}
		}

		/// <summary>
		/// Gets or Sets if the Global Database Type is Set
		/// </summary>
		public static bool IsGlobalDatabaseTypeSet
		{
			get
			{
				return m_isDatabaseTypeSet;
			}
			set
			{
				m_isDatabaseTypeSet		=	value;
			}
		}


		#endregion Properties

	}
}
