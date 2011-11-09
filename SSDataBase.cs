// Copyright 2009, 2010, 2011 Kevin Schott

// This file is part of SecondSight.

// SecondSight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// SecondSight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with SecondSight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.OleDb;

namespace SecondSight
{
	#region Support Structs and Enums
	
    public enum SSTable {Current, Dispensed, DBInfo, MergeInfo, MergeItems}

   	#endregion
	
    public class SSDataBase
    {
    	//Constants
    	private const int NUM_SEARCH_PARAMS = 21;
        private const float SCORE_THRESHOLD = 2.5F;
    	private static readonly string [] RLIMITS_FILTERSTRING = new string[]
    		{"SKU", "SphereOD", "CylinderOD", "AxisOD", "AddOD", 
    		"SphereOS", "CylinderOS", "AxisOS", "AddOS", 
    		"Type", "Gender", "Size", "Tint", "DateAdded", "DateDispensed"};

        //Member variables
        private string DBPath;
        private SQLiteConnection DBConn;
        private SQLiteCommand DBcmd;
        private SQLiteParameter[] DBParams;
        private SQLiteParameter[] Sparams;		//Search parameters
        private DataTable dbresults, tresults;  //tresults used for split multifocals only
        private DataTable invresults; //Inventory results - used for full inventory display
        
        public DataTable DBResults{get{return dbresults;}}
        public DataTable DBResultsAux{get{return tresults;}}
        public DataTable InvResults{get{return invresults;}}
        public string MyPath{get{return DBPath;}}
        
        #region Member Functions

        /// <summary>
        /// Creates a new database and populates it with the appropriate SecondSight tables
        /// </summary>
        /// <remarks>Only used when the file does not already exist (Use OpenDB for existing files)</remarks>
        /// <param name="path">The full file path to the new database</param>
        /// <param name="dbname">The name of the database</param>
        /// <param name="dblocation">The physical location of the database (where the physical inventory is and/or where the clinic is held)</param>
        /// <exception cref="System.IO.IOException">Thrown when the file already exists</exception>
        public void CreateNewDB(string path, string dbname, string dblocation)
        {
            if (File.Exists(path))
            	throw new IOException(String.Format("File already exists: {0}", path));

            //Close the currently open connection and clear the data tables
            DBConn.Close();
            dbresults.Clear();
            tresults.Clear();
            invresults.Clear();
            
            //File doesn't exist, create and configure
            DBPath = path;
            DBConn.ConnectionString = new SQLiteConnectionStringBuilder(String.Format(@"Data Source={0}", DBPath)).ConnectionString;
            DBConn.Open();

			using (SQLiteTransaction dbt = DBConn.BeginTransaction())
            {				
				//This table has a unique integer, SKU, but it is not the primary key because the 
				//SKU numbers are reused from year to year in a fixed-size inventory.
				//Instead, the primary key is the special SQLite column ROWID
				DBcmd.CommandText = "CREATE TABLE CurrentInventory(SKU INTEGER UNIQUE, SphereOD REAL, " +
	                "CylinderOD REAL, AxisOD INTEGER, AddOD REAL, SphereOS REAL, CylinderOS REAL, " +
					"AxisOS INTEGER, AddOS REAL, Type TEXT, Gender TEXT, Size TEXT, " +
	                "Tint TEXT, DateAdded DATETIME, Comment TEXT)";
	            DBcmd.ExecuteNonQuery();
	
	            //This table is not indexed by SKU, since it could have entries from multiple years, thus duplicate SKUs.
	            //Table does not use a compound primary key (SKU, Date) because SQLite searches much faster with an 
	            //integer primary key.  (The primary key is ROWID, a special column name in SQLite).
	            DBcmd.CommandText = "CREATE TABLE DispensedInventory(SKU INTEGER, SphereOD REAL, " +
	                "CylinderOD REAL, AxisOD INTEGER, AddOD REAL, SphereOS REAL, CylinderOS REAL, " +
					"AxisOS INTEGER, AddOS REAL, Type TEXT, Gender TEXT, Size TEXT, " +
	                "Tint TEXT, DateAdded DATETIME, DateDispensed DATETIME, Comment TEXT)";
	            DBcmd.ExecuteNonQuery();
	
	            //Table contains a single field, used for database information
	            DBcmd.CommandText = "CREATE TABLE DBInfo(Name TEXT, Location TEXT, DateCreated DATETIME)";
	            DBcmd.ExecuteNonQuery();
	
	            //Insert information field
	            DBcmd.CommandText = @"INSERT INTO DBInfo VALUES(@pdbname, @pdbloc, @pdbdate)";
	            DBParams[16].Value = dbname;
	            DBParams[17].Value = dblocation;
	            DBParams[18].Value = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
	            DBcmd.ExecuteNonQuery();
	            
	            //This table holds duplicate records - only used when importing a database from REIMS
	            DBcmd.CommandText = "CREATE TABLE DupedInventory(SKU INTEGER UNIQUE, SphereOD REAL, " +
	                "CylinderOD REAL, AxisOD INTEGER, AddOD REAL, SphereOS REAL, CylinderOS REAL, " +
					"AxisOS INTEGER, AddOS REAL, Type TEXT, Gender TEXT, Size TEXT, " +
	                "Tint TEXT, DateAdded DATETIME, Comment TEXT)";
	            DBcmd.ExecuteNonQuery();
				
				dbt.Commit(); //Commit the transaction
           	}
			
            return;
        }

        /// <summary>
        /// Opens an existing database file.
        /// <remarks>Should be called on init and whenever the user attempts to open a different database.</remarks>
        /// </summary>
        /// <param name="path">The full file path of the database file to open</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when the file to open does not exist</exception>
        /// <exception cref="System.Data.SQLite.SQLiteException">Thrown when the file is not a valid SecondSight database</exception>
        public void OpenDB(string path)
        {
            //Check if the file exists
            if (!File.Exists(path))
            	throw new FileNotFoundException("File not found.");
            
            //Close the currently open connection
            DBConn.Close();
            
            DBPath = path;
            DBConn.ConnectionString = new SQLiteConnectionStringBuilder(String.Format(@"Data Source={0}", DBPath)).ConnectionString;
            
            try {
            	DBConn.Open();
            } catch (SQLiteException sqlex) {
            	throw new SQLiteException("File is not a valid SecondSight database.", sqlex.InnerException);
            }
            
            //Check to make sure this is a valid SecondSight database
            DataTable sdt = DBConn.GetSchema("Tables");

            try {
                if (!((string)sdt.Rows[0][2] == "CurrentInventory"
                        && (string)sdt.Rows[1][2] == "DispensedInventory"
                        && (string)sdt.Rows[2][2] == "DBInfo"
                        && (string)sdt.Rows[3][2] == "DupedInventory")) {
            		throw new SQLiteException("File is not a valid SecondSight database.");
            	}
            } catch (IndexOutOfRangeException e) {
            	throw new SQLiteException("File is not a valid SecondSight database." , e.InnerException);
            }
            
            sdt.Reset();
            sdt = DBConn.GetSchema("Columns");
            
            try {
            	if (sdt.Rows[13][11].ToString() == "text") {  //Old DB format for dates, update and convert
            		DBConvert(0);
            	} else if(sdt.Rows[13][11].ToString() != "datetime") {//Not old format, not new format, so unknown format
            		throw new SQLiteException ("File is not a valid SecondSight database.");
            	}
            } catch (IndexOutOfRangeException) { //Should never get here
            	throw new SQLiteException ("File is not a valid SecondSight database.");
            }
        }

        /// <summary>
        /// Close the currently open database.
        /// </summary>
        /// <exception cref="System.Exception">Thrown when the database could not be closed.</exception>
        public void CloseDB()
        {

        	try {
	        	DBConn.Close();
	        	invresults.Clear();
	        	dbresults.Clear();
	        	tresults.Clear();
        	} catch (Exception e) {
        		throw new Exception("Could not close database.", e.InnerException);
        	}
        }

        /// <summary>
        /// Search using pre-determined parameters.
        /// </summary>
        /// <param name="sprec">The paramaters to search by</param>
        /// <param name="deye">The dominant eye</param>
        /// <param name="splitmf">Whether or not to split a multifocal search into two searches - Distance and Closeup</param>
        public void RxSearch(SpecsRecord sprec, DomEye deye, bool splitmf) 
        {
        	//Clear results for the search
        	dbresults.Clear(); 
        	tresults.Clear();
        	
        	//Split multifocals: two searches, back to back, for single vision glasses.
        	//First search is for specified sphere power, second is for sphere power + add power
        	if(splitmf) 
        	{
            	float odadd, osadd;
        		odadd = sprec.AddOD;
        		osadd = sprec.AddOS;
        		sprec.AddOD = 0;
        		sprec.AddOS = 0;
        		sprec.Type = SpecType.Single;
	        	RxSearchRange(sprec, deye, dbresults);  //Search for first set of matches
				RxScoreResults(sprec, deye, dbresults); //Rank the matches
				sprec.SphereOD += odadd;
				sprec.SphereOS += osadd;
				RxSearchRange(sprec, deye, tresults);  //Search for second set of matches
				RxScoreResults(sprec, deye, tresults); //Rank the matches
        	}
        	else
        	{
        		RxSearchRange(sprec, deye, dbresults);  //Search for possible matches
				RxScoreResults(sprec, deye, dbresults); //Rank the matches
        	}
        }

        /// <summary>
        /// Inserts a single record into the database into a specified table.  Not all tables are valid and
        /// an exception will be thrown if an invalid destination table is specified
        /// </summary>
        /// <param name="sprec">The pre-built SpecsRecord to insert</param>
        /// <param name="_table">The table to insert the record into</param>
        public void Insert(SpecsRecord sprec, SSTable _table)
        {
            string destination = "";

            if (_table == SSTable.Current) {
                destination = "CurrentInventory";
            } else if (_table == SSTable.MergeItems) {
                destination = "MergeItems";
            }

            DBcmd.CommandText = @"INSERT INTO " + destination + @" VALUES(@psku, @psod,  @pcod, @paxod, " +
                @"@padod, @psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pcom)";
            //DBcmd.CommandText = @"INSERT INTO CurrentInventory VALUES(@psku, @psod,  @pcod, @paxod, " +
            //    @"@padod, @psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pcom)";

            DBParams[0].Value = sprec.SKU;
            DBParams[1].Value = sprec.SphereOD;
            DBParams[2].Value = sprec.CylOD;
            DBParams[3].Value = sprec.AxisOD;
            DBParams[4].Value = sprec.AddOD;
            DBParams[5].Value = sprec.SphereOS;
            DBParams[6].Value = sprec.CylOS;
            DBParams[7].Value = sprec.AxisOS;
            DBParams[8].Value = sprec.AddOS;
            DBParams[9].Value = sprec.Type;
            DBParams[10].Value = sprec.Gender;
            DBParams[11].Value = sprec.Size;
            DBParams[12].Value = sprec.Tint;
            DBParams[13].Value = sprec.DateAdded;
            DBParams[15].Value = sprec.Comment;

            try {
                DBcmd.ExecuteNonQuery();
            }
            catch (InvalidOperationException ioe) {
            	throw new InvalidOperationException(ioe.Message, ioe.InnerException);
            }
            catch (SQLiteException sqle) {
                throw new SQLiteException(sqle.Message, sqle.InnerException);
            }
        }

        //Imports data from an old REIMS FoxPro database into the current database
        //path - the directory path for GLSKU.DBF and DISPENSE.DBF
        //These file names are mandatory
        //This function should only be used with a fresh (empty) SecondSight database

        /// <summary>
        /// Imports data from an old REIMS Visual FoxPro database into the the current SecondSight database
        /// </summary>
        /// <param name="path">The full directory path to GLSKU.DBF and DISPENSE.DBF</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when one of the mandatory FoxPro files is not found</exception>
        /// <exception cref="System.Data.SQLite.SQLiteException">Thrown when a duplicate SKU is found when adding to the CurrentInventory table</exception>
        /// <exception cref="System.Data.SQLite.SQLiteException">Thrown when a record cannot be entered into the DupedInventory table</exception>
        /// <exception cref="System.Data.SQLite.SQLiteException">Thrown when a record cannot be entered into the DispensedInventory table</exception>
        public void ImportREIMS(string path)
        {
        	//Check for GLSKU.DBF and DISPENSE.DBF
        	if(!File.Exists(path + "GLSKU.DBF"))
        		throw new FileNotFoundException("File Not Found", "GLSKU.DBF");
        	if(!File.Exists(path + "DISPENSE.DBF"))
        		throw new FileNotFoundException("File Not Found", "DISPENSE.DBF");
        	
        	//Set up the FoxPro connection and transfer medium data set
        	DataTable rdt = new DataTable();
        	
        	#region Current Inventory Import
        	using (OleDbConnection rcnn = new OleDbConnection(
        		String.Format(@"Provider=vfpoledb;Data Source={0} ;Collating Sequence=general;", path)))
        	{
        		using (OleDbCommand rcmd = rcnn.CreateCommand())
        		{
		        	OleDbDataAdapter rda;
		        	rcmd.Connection = rcnn;
		
		        	//Populate the Data Set and close the FoxPro database
		        	rcnn.Open();
		        	rcmd.CommandText = @"SELECT * FROM GLSKU";
		        	rda = new OleDbDataAdapter(rcmd);
		        	rda.Fill(rdt);
        		}
        	}
        	
        	//String processing for every record in the table
        	//Trims out all the unnecessary characters from the SKU and fills in any blank fields with default values
        	for(int i = 0; i < rdt.Rows.Count; i++)
        	{
        		String temp = rdt.Rows[i][0].ToString();
        		temp = temp.Substring(temp.IndexOf(":")+1);
        		rdt.Rows[i][0] = temp.Substring(0, temp.IndexOf(" "));
        		
        		//Type - Sets any blanks to S and changes B(ifcoal) to M(ultifocal)
        		if(rdt.Rows[i][1].ToString() == " ")
        			rdt.Rows[i][1] = "S";
        		else if(rdt.Rows[i][1].ToString() == "B")
        			rdt.Rows[i][1] = "M";
        		
        		//Gender - Sets any blanks to unisex (U)
        		if(rdt.Rows[i][10].ToString() == " ")
        			rdt.Rows[i][10] = "U";
        		
        		//Size - Sets any blanks to medium (M)
        		if(rdt.Rows[i][12].ToString() == " ")
        			rdt.Rows[i][12] = "M";
        		
        		//Tint - Sets any blanks to none (N)
        		if(rdt.Rows[i][13].ToString() == " ")
        			rdt.Rows[i][13] = "N";
        		
        		//Date - Sets any blank date to January 01, 2000 (01/01/00)
        		if(rdt.Rows[i][14].ToString() == " ")
        			rdt.Rows[i][14] = "01/01/00";
        	}
        	
        	ArrayList duperec = new ArrayList();  //Will be an array of DataRow objects
        	
        	//This section is responsible for copying the values from the DataTable
        	//to the SecondSight database

        	//Import into CurrentInventory
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())  //Transaction wrapper for bulk insert
        	{
        		DBcmd.CommandText = @"INSERT INTO CurrentInventory VALUES(@psku, @psod, @pcod, @paxod, @padod, " +
            		@"@psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pcom)";
        		
        		DBParams[15].Value = ""; //Comment field - not present in REIMS database
        		
        		//Sets parameter values and executes a single insert.  Happens once for every record
        		//in the GLSKU table from REIMS
        		for(int i = 0; i < rdt.Rows.Count; i++)
        		{
        			DBParams[0].Value = rdt.Rows[i][0];
        			DBParams[1].Value = rdt.Rows[i][2];
        			DBParams[2].Value = rdt.Rows[i][3];
        			DBParams[3].Value = rdt.Rows[i][4];
        			DBParams[4].Value = rdt.Rows[i][5];
        			DBParams[5].Value = rdt.Rows[i][6];
        			DBParams[6].Value = rdt.Rows[i][7];
        			DBParams[7].Value = rdt.Rows[i][8];
        			DBParams[8].Value = rdt.Rows[i][9];
        			DBParams[9].Value = rdt.Rows[i][1];
        			DBParams[10].Value = rdt.Rows[i][10];
        			DBParams[11].Value = rdt.Rows[i][12];
        			DBParams[12].Value = rdt.Rows[i][13];
        			DBParams[13].Value = rdt.Rows[i][14];
        			
					try
					{
	        			DBcmd.ExecuteNonQuery();
					}
					catch (SQLiteException e)
					{
						if (e.ErrorCode == SQLiteErrorCode.Constraint) //Dupicate SKU
						{
							//Add record to an arraylist so they can later be inserted into 
							//the dupes table
							duperec.Add(rdt.Rows[i]);
						}
					}
        		}
        		dbt.Commit();
        	}
        	#endregion
        	
        	#region Duped Inventory Storage
        	
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
        		DBcmd.CommandText = @"INSERT INTO DupedInventory VALUES(@psku, @psod, @pcod, @paxod, @padod, " +
            		@"@psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pcom)";
        		
        		DBParams[15].Value = ""; //Comment field - not present in REIMS database
        		//Sets parameter values and executes a single insert.  Happens once for ever duplicate SKU
        		//that was found in GLSKU.DBF from REIMS
        		for(int i = 0; i < duperec.Count; i++)
        		{
        			DBParams[0].Value = ((DataRow)duperec[i])[0];
        			DBParams[1].Value = ((DataRow)duperec[i])[2];
        			DBParams[2].Value = ((DataRow)duperec[i])[3];
        			DBParams[3].Value = ((DataRow)duperec[i])[4];
        			DBParams[4].Value = ((DataRow)duperec[i])[5];
        			DBParams[5].Value = ((DataRow)duperec[i])[6];
        			DBParams[6].Value = ((DataRow)duperec[i])[7];
        			DBParams[7].Value = ((DataRow)duperec[i])[8];
        			DBParams[8].Value = ((DataRow)duperec[i])[9];
        			DBParams[9].Value = ((DataRow)duperec[i])[1];
        			DBParams[10].Value = ((DataRow)duperec[i])[10];
        			DBParams[11].Value = ((DataRow)duperec[i])[12];
        			DBParams[12].Value = ((DataRow)duperec[i])[13];
        			DBParams[13].Value = ((DataRow)duperec[i])[14];
        			
					try {
	        			DBcmd.ExecuteNonQuery();
					}
					catch (SQLiteException e) {
                        throw e;
					}
        		}
        		dbt.Commit();
        	}
        	#endregion
        	
        	#region Dispensed Inventory Import
        	rdt.Clear();
        	using (OleDbConnection rcnn = new OleDbConnection(
        		String.Format(@"Provider=vfpoledb;Data Source={0} ;Collating Sequence=general;", path)))
        	{
        		using (OleDbCommand rcmd = rcnn.CreateCommand())
        		{
		        	OleDbDataAdapter rda;
		        	rcmd.Connection = rcnn;
		
		        	//Populate the Data Set and close the FoxPro database
		        	rcnn.Open();
		        	rcmd.CommandText = @"SELECT * FROM DISPENSE";
		        	rda = new OleDbDataAdapter(rcmd);
		        	rda.Fill(rdt);
        		}
        	}
        	
        	//String processing for every record in the table
        	//Trims out all the unnecessary characters from the SKU and fills in any blank fields with default values
        	for(int i = 0; i < rdt.Rows.Count; i++)
        	{
        		String temp = rdt.Rows[i][0].ToString();
        		temp = temp.Substring(temp.IndexOf(":")+1);
        		rdt.Rows[i][0] = temp.Substring(0, temp.IndexOf(" "));
        		
        		//Gender - Sets any blanks to unisex (U)
        		if(rdt.Rows[i][10].ToString() == " ")
        			rdt.Rows[i][10] = "U";
        		
        		//Size - Sets any blanks to medium (M)
        		if(rdt.Rows[i][12].ToString() == " ")
        			rdt.Rows[i][12] = "M";
        		
        		//Tint - Sets any blanks to none (N)
        		if(rdt.Rows[i][13].ToString() == " ")
        			rdt.Rows[i][13] = "N";
        		
        		//Date - Sets any blank date to January 01, 2000 (01/01/00)
        		if(rdt.Rows[i][14].ToString() == " ")
        			rdt.Rows[i][14] = "01/01/00";
        	}
        	
        	//Import into DispensedInventory
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
        		DBcmd.CommandText = @"INSERT INTO DispensedInventory VALUES(@psku, @psod, @pcod, @paxod, @padod, " +
            		@"@psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pddate, @pcom)";
        		
        		DBParams[15].Value = ""; //Comment field - not present in REIMS database
        		//Sets parameter values and executes a single insert.  Happens once for every record
        		//in the DISPENSE table from REIMS
        		for(int i = 0; i < rdt.Rows.Count; i++)
        		{
        			DBParams[0].Value = rdt.Rows[i][0];
        			DBParams[1].Value = rdt.Rows[i][2];
        			DBParams[2].Value = rdt.Rows[i][3];
        			DBParams[3].Value = rdt.Rows[i][4];
        			DBParams[4].Value = rdt.Rows[i][5];
        			DBParams[5].Value = rdt.Rows[i][6];
        			DBParams[6].Value = rdt.Rows[i][7];
        			DBParams[7].Value = rdt.Rows[i][8];
        			DBParams[8].Value = rdt.Rows[i][9];
        			DBParams[9].Value = rdt.Rows[i][1];
        			DBParams[10].Value = rdt.Rows[i][10];
        			DBParams[11].Value = rdt.Rows[i][12];
        			DBParams[12].Value = rdt.Rows[i][13];
        			DBParams[13].Value = rdt.Rows[i][14];
        			DBParams[14].Value = rdt.Rows[i][14]; //Dispensed Date - not present in REIMS so set = add date
        			
					try {
	        			DBcmd.ExecuteNonQuery();
					}
					catch (SQLiteException e) {
                        throw e;
					}
        		}
        		dbt.Commit();
        	}
        	#endregion    	
        }

        /// <summary>
        /// Dispenses glasses.  Moves a record from the CurrentInventory table to the DispensedInventory table
        /// </summary>
        /// <param name="sprec">A valid SpecsRecord populated by data from the CurrentInventory table</param>
        /// <param name="ismain">Used to distinguish between main and auxiliary tables for a split multifocals search</param>
        /// <exception cref="System.Exception">Thrown when a record could not be inserted into DispensedInventory or deleted from CurrentInventory</exception>
        public void Dispense(SpecsRecord sprec, bool ismain)
        {
            //Make a copy of the record described by sprec to the DispensedInventory table
            DBcmd.CommandText = @"INSERT INTO DispensedInventory VALUES(@psku, @psod, @pcod, @paxod, @padod, " +
                @"@psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pddate, @pcom)";

            DBParams[0].Value = sprec.SKU;
            DBParams[1].Value = sprec.SphereOD;
            DBParams[2].Value = sprec.CylOD;
            DBParams[3].Value = sprec.AxisOD;
            DBParams[4].Value = sprec.AddOD;
            DBParams[5].Value = sprec.SphereOS;
            DBParams[6].Value = sprec.CylOS;
            DBParams[7].Value = sprec.AxisOS;
            DBParams[8].Value = sprec.AddOS;
            DBParams[9].Value = sprec.Type;
            DBParams[10].Value = sprec.Gender;
            DBParams[11].Value = sprec.Size;
            DBParams[12].Value = sprec.Tint;
            DBParams[13].Value = sprec.DateAdded;
            DBParams[14].Value = sprec.DateDispensed;
            DBParams[15].Value = sprec.Comment;

            try {
                DBcmd.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw ex;
            }

            //Delete the record from the CurrentInventory table
            DBcmd.CommandText = @"DELETE FROM CurrentInventory WHERE SKU = @psku";

            try {
                DBcmd.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw ex;
            }
            
            //Clean up the DataTables for display purposes - This does not affect the backend DB
            if(ismain)  //Main table cleanup
            {
	            int dbrc = dbresults.Rows.Count;
	            for (int i = 0; i < dbrc; i++)
	            {
	            	if (Convert.ToUInt16(dbresults.Rows[i][0]) == sprec.SKU)
	            	{
	            		dbresults.Rows.Remove(dbresults.Rows[i]);
	            		break;
	            	}
	            }
            }
            else  //Auxiliary table cleanup (only happens for split multifocals)
            {
	            int dbrc = tresults.Rows.Count;
	            for(int i = 0; i < dbrc; i++)
	            {
	            	if (Convert.ToUInt16(tresults.Rows[i][0]) == sprec.SKU)
	            	{
	            		tresults.Rows.Remove(tresults.Rows[i]);
	            		break;
	            	}
	            }
            }
        }

        /// <summary>
        /// Permanently deletes a record from the database
        /// </summary>
        /// <remarks>NOT FOR DISPENSING</remarks>
        /// <param name="sku">The SKU corresponding to the record to be deleted</param>
        /// <exception cref="System.Exception">Thrown when the record could not be deleted</exception>
        public void Delete(uint sku, SSTable _table) 
        {
            if (_table == SSTable.MergeItems) {
                DBcmd.CommandText = @"DELETE FROM MergeItems WHERE SKU = @psku";
            } else {
                DBcmd.CommandText = @"DELETE FROM CurrentInventory WHERE SKU = @psku";
            }

            try {
                DBParams[0].Value = sku;
                DBcmd.ExecuteNonQuery();
            } catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Merges the data from one SecondSight database into the currently open one
        /// </summary>
        /// <remarks>This merge is only performed on CurrentInventory.  Any records with duplicate SKUs are ignored (ie. no overwriting)</remarks>
        /// <param name="mergeDB">The SecondSight database to be merged into the current one</param>
        /// <exception cref="System.Data.SQLite.SQLiteException">Thrown if any part of the merge fails</exception>
        public void SmallMerge(SSDataBase mergeDB)
        {
            //Attach the merge database to the master
            DBcmd.CommandText = "ATTACH '" + mergeDB.MyPath + "' AS TOMERGE";
            DBcmd.ExecuteNonQuery();

            using(SQLiteTransaction dbt = DBConn.BeginTransaction())
            {
                DBcmd.CommandText = "INSERT INTO CurrentInventory SELECT * FROM TOMERGE.CurrentInventory WHERE SKU NOT IN (SELECT SKU FROM CurrentInventory)";

                try {
                    DBcmd.ExecuteNonQuery();
                } catch (SQLiteException ex) {
                    dbt.Rollback();
                    throw new Exception("Failed to merge.", ex.InnerException);
                }

                dbt.Commit();
            }

            //Detach the database
            DBcmd.CommandText = "DETACH TOMERGE";
            DBcmd.ExecuteNonQuery();
        }
        
        //Gets the full inventory and stores it in the invresults DataTable
        //Used by the inventory display in the Add New Item and Full Inventory View tabs 
        public void GetCurrentInventory()
        {
        	invresults.Clear();
        	SQLiteDataAdapter da;
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
        		DBcmd.CommandText = @"SELECT * FROM CurrentInventory ORDER BY SKU";
        		da = new SQLiteDataAdapter(DBcmd);
        		try
        		{
	        		da.Fill(invresults);
        		}
        		catch
        		{
        			invresults.Clear();
        		}
        	}
        }

        /// <summary>
        /// Gets the full contents of a single table and stores it in a DataTable
        /// </summary>
        /// <param name="_dt">The DataTable to store the data in</param>
        /// <param name="_table">Which table to read the data from</param>
        /// <exception cref="System.Data.SQLite.SQLiteException">Thrown when the table is empty and the DataTable can't be filled</exception>
        public void GetTable(DataTable _dt, SSTable _table)
        {
            SQLiteDataAdapter da;
            string commandstring = "";

            //Select the appropriate command string
            switch (_table)
            {
                case SSTable.Current:
                    commandstring = @"SELECT * FROM CurrentInventory ORDER BY SKU";
                    break;
                case SSTable.Dispensed:
                    commandstring = @"SELECT * FROM DispensedInventory ORDER BY SKU";
                    break;
                case SSTable.DBInfo:
                    commandstring = @"SELECT * FROM DBInfo";
                    break;
                case SSTable.MergeInfo:
                    commandstring = @"SELECT * FROM MergeInfo";
                    break;
                case SSTable.MergeItems:
                    commandstring = @"SELECT * FROM MergeItems ORDER BY SKU";
                    break;
                default:
                    break;
            }

            //Build and execute the transaction
            using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
        		DBcmd.CommandText = commandstring;
        		da = new SQLiteDataAdapter(DBcmd);
        		try {
	        		da.Fill(_dt);
        		}
        		catch (Exception ex) {
                    throw ex;
                }
        	}
        }
        
        //Searches current inventory for certain SKUs and stores the results
        //in the passed-in DataTable dt
        public void SKUSearch(int sku, DataTable dt)
        {
        	SQLiteDataAdapter da;
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
        		DBcmd.CommandText = "SELECT * FROM CurrentInventory WHERE SKU = @psku";
        		da = new SQLiteDataAdapter(DBcmd);
        		DBParams[0].Value = sku;
        		try {
	        		da.Fill(dt);
        		}
        		catch (SQLiteException e) {
        			if(e.ErrorCode == SQLiteErrorCode.IOErr) {
        				throw new SQLiteException("IO Error", e.InnerException);
        			}
        		}
        		dbt.Commit();
        	}
        }
        
        //Query everything in the duplicates table and store results in dt
        public void GetDuplicates(DataTable dt)
        {
        	SQLiteDataAdapter da;
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
        		DBcmd.CommandText = "SELECT * FROM DupedInventory ORDER BY SKU";
        		da = new SQLiteDataAdapter(DBcmd);
        		try
        		{
        			da.Fill(dt);
        		}
        		catch (SQLiteException e)
        		{
        			if(e.ErrorCode == SQLiteErrorCode.IOErr)
        				dt.Clear();
        			else
        				Console.WriteLine(e.Message);
        		}
        	}
        }
        
        //Query everything in the DBInfo table and store results in dt
        public void GetDBInfo(DataTable dt)
        {
        	SQLiteDataAdapter da;
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
        		DBcmd.CommandText = "SELECT * FROM DBInfo";
        		da = new SQLiteDataAdapter(DBcmd);
        		try {
        			da.Fill(dt);
        		}
        		catch (SQLiteException e) {
        			if(e.ErrorCode == SQLiteErrorCode.IOErr)
        				dt.Clear();
        		}
        	}
        }
        
        /// <summary>
        /// Retrieves the first unused SKU in CurrentInventory
        /// </summary>
        /// <returns>The first unused SKU</returns>
        public int GetNextFreeSKU()
        {
        	int numrecords = invresults.Rows.Count;
        	int currentsku = 1; //Current SKU to check, return value if it does not exist in database
        	int tsku; //Temporary int to hold converted SKU values from the database
        	
        	if(numrecords > 0) {
        		tsku = Convert.ToInt16(invresults.Rows[0][0]);
        		if(tsku > 1) {
        			currentsku = 1;
        		} else {
        			currentsku = tsku;
        		}
        	}
        	
        	for(int i = 0; i < numrecords; i++) {
        		tsku = Convert.ToInt16(invresults.Rows[i][0]);
        		if(tsku != currentsku) {
    				return currentsku;
        		}
        		currentsku++;
        	}
        	return currentsku;
        }
        
        /// <summary>
        /// Retrieves the first unused SKU in CurrentInventory within a specified range
        /// </summary>
        /// <param name="smin">The minimum SKU to check</param>
        /// <param name="smax">The maximum SKU to check</param>
        /// <returns>The first unused SKU in the range</returns>
        public int GetNextFreeSKU(int smin, int smax)
        {
			int numrecords = invresults.Rows.Count;
			int currentsku = smin;
			int imin = 0;  //Index in the data table of the sku specified by smin 
			int currentindex = 0;  //Current index
			
			if(smin < 1) {
				smin = currentsku = 1;
			}
			
			//Find starting index
			while(imin < numrecords && Convert.ToInt16(invresults.Rows[imin][0]) < smin) {
				imin++;
			}
			currentindex = imin;
			
			//Find ending index
			while(currentindex < numrecords && currentsku == Convert.ToInt16(invresults.Rows[currentindex][0])) {
				if(currentsku == smax) {
					throw new IndexOutOfRangeException("No free SKUs in range.");
				}
				currentsku++;
				currentindex++;
			}
        	return currentsku;
        }
        
        /// <summary>
        /// Converts the currently open database into the newest format
        /// </summary>
        /// <remarks>The _cvtype parameter corrseponds to SecondSight database versions that need changing.
        ///     0 = Version 0.90 </remarks>
        /// <param name="_cvtype">Integer representation of the convertable database version</param>
        private void DBConvert(int _cvtype)
        {
        	DataTable tcitable = new DataTable();  //Temporary table to hold current inventory
        	DataTable tditable = new DataTable();  //Temporary table to hold dispensed inventory
        	DataTable tinfotable = new DataTable(); //Temporary table to hold DB info
        	
        	if(_cvtype == 0) {  //Convert from version 0.90
        		SQLiteDataAdapter da;
	        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
	        	{
	        		DBcmd.CommandText = @"SELECT * FROM CurrentInventory ORDER BY SKU"; 
	        		da = new SQLiteDataAdapter(DBcmd);
	        		try {
		        		da.Fill(tcitable);  //Store CurrentInventory in a temporary table for conversion
	        		} catch {
	        			tcitable.Clear();
	        		}
	        		da.Dispose();
	        		
	        		DBcmd.CommandText = @"SELECT * FROM DispensedInventory ORDER BY SKU";
	        		da = new SQLiteDataAdapter(DBcmd);
	        		try {
		        		da.Fill(tditable);  //Store DispensedInventory in a temporary table for conversion
	        		} catch {
	        			tditable.Clear();
	        		}
	        		da.Dispose();
	        		
	        		DBcmd.CommandText = @"SELECT * FROM DBInfo";
	        		da = new SQLiteDataAdapter(DBcmd);
	        		try {
	        			da.Fill(tinfotable); //Store DB Info in a temporary table for conversion
	        		} catch {
	        			tinfotable.Clear();
	        		}
	        	}
	        	
	        	//Alter the tables.  Because SQLite only supports a limited subset of ALTER TABLE,
	        	//this alteration is done by recreating the database file
	        	DBConn.Close();  //Close the connection and delete the old database file
	        	if(File.Exists(DBPath)) {
	        		File.Delete(DBPath);
	        	}
	        	
	        	//Run the create new db function to recreate the database based on the info in the DBInfo table
	        	CreateNewDB(DBPath, tinfotable.Rows[0][0].ToString(), tinfotable.Rows[0][1].ToString());
	        	
	        	//Update the date created to the correct value
	        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
	        	{
	        		DBcmd.CommandText = "UPDATE DBInfo SET DateCreated = @pdbdate";
	        		DBParams[18].Value = String.Format("{0:yyyy-MM-dd}", tinfotable.Rows[0][2].ToString());
	        		DBcmd.ExecuteNonQuery();
	        	}
	        	
	        	//Add the data back into the newly formatted tables
	        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
	        	{
	        		DBcmd.CommandText = @"INSERT INTO CurrentInventory VALUES(@psku, @psod, @pcod, @paxod, @padod, " +
            			@"@psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pcom)";
	        		
	        		//Set up each parameter
	        		foreach (DataRow row in tcitable.Rows) {
	        			DBParams[0].Value = row[0];
	        			DBParams[1].Value = row[1];
	        			DBParams[2].Value = row[2];
	        			DBParams[3].Value = row[3];
	        			DBParams[4].Value = row[4];
	        			DBParams[5].Value = row[5];
	        			DBParams[6].Value = row[6];
	        			DBParams[7].Value = row[7];
	        			DBParams[8].Value = row[8];
	        			DBParams[9].Value = row[9];
	        			DBParams[10].Value = row[10];
	        			DBParams[11].Value = row[11];
	        			DBParams[12].Value = row[12];
	        			DBParams[13].Value = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(row[13].ToString()));
	        			DBParams[15].Value = row[14];
	        			
	        			try {
	        				DBcmd.ExecuteNonQuery();
	        			} catch (SQLiteException sqle) {
	        				throw new SQLiteException (sqle.Message, sqle.InnerException);
	        			}
		        	}
	        		
	        		DBcmd.CommandText = @"INSERT INTO DispensedInventory VALUES(@psku, @psod, @pcod, @paxod, @padod, " +
            		@"@psos, @pcos, @paxos, @pados, @ptype, @pgen, @psize, @ptint, @padate, @pddate, @pcom)";
	        		
	        		//Set up each parameter
	        		foreach (DataRow row in tditable.Rows) {
	        			DBParams[0].Value = row[0];
	        			DBParams[1].Value = row[1];
	        			DBParams[2].Value = row[2];
	        			DBParams[3].Value = row[3];
	        			DBParams[4].Value = row[4];
	        			DBParams[5].Value = row[5];
	        			DBParams[6].Value = row[6];
	        			DBParams[7].Value = row[7];
	        			DBParams[8].Value = row[8];
	        			DBParams[9].Value = row[9];
	        			DBParams[10].Value = row[10];
	        			DBParams[11].Value = row[11];
	        			DBParams[12].Value = row[12];
	        			DBParams[13].Value = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(row[13].ToString()));
	        			DBParams[14].Value = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(row[14].ToString()));
	        			DBParams[15].Value = row[15];
	        			
	        			try {
	        				DBcmd.ExecuteNonQuery();
	        			} catch (SQLiteException sqle) {
	        				throw new SQLiteException (sqle.Message, sqle.InnerException);
	        			}
		        	}
	        		
	        		dbt.Commit();
	        	}
        	}
        }

        /// <summary>
        /// Builds and runs a report query from a list of filters
        /// </summary>
        /// <param name="_table">Table to search on.  Only Current and Dispensed are valid values for this function.</param>
        /// <param name="_summaries">Whether the report is going to be a summary (true) or a full list (false)</param>
        /// <param name="_groupby">The index of the selected Group By value.</param>
        /// <param name="_filters">Dictionary of filters.  The key is the name of the key to filter by (e.g. SphereOD), and the value
        /// is a LinkedList of values to limit by.  If there are more than the required number of values for a particular key, 
        /// an additional clause is added for that key using a logical OR.</param>
        /// <param name="_lq">The string builder that will hold the plain-english version of the query.</param>
        /// <example>
        /// If the key "Size" is in the dictionary and the LinkedList contains "Small" and "Medium", then the resulting portion of the 
        /// WHERE clause would read "AND (Size = 'Small' OR size = 'Medium') ".
        /// </example>
        /// <returns>The data table containing the results of the query.</returns>
        public DataTable ReportQuery(SSTable _table, bool _summaries, int _groupby, Dictionary<int, LinkedList<string>> _filters)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder selectstring = new StringBuilder("SELECT ");
            StringBuilder sourcestring = new StringBuilder();
            StringBuilder filterstring = new StringBuilder();
            StringBuilder casestring = new StringBuilder();
            StringBuilder groupbystring = new StringBuilder();
            StringBuilder combostring = new StringBuilder(); //Combination of case, source and filter strings
            
            //Builds the Selection and Case strings based on the selected Group By field (for summaries)
            if (_summaries) {
                if (_groupby < 9) { //Rx parameters - grouped by predefined ranges
                    selectstring.Append("t.range as Range, Count(*) AS Count ");
                    casestring.Append("FROM (SELECT CASE ");
                    if (_groupby == 1 || _groupby == 5) { //Sphere
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" < -6.25 THEN '-6.25+' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -6.00 AND -4.00 THEN '-4.00 - -6.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -3.75 AND -2.25 THEN '-2.25 - -3.75' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -2.00 AND -1.25 THEN '-1.25 - -2.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -1.00 AND -0.50 THEN '-0.50 - -1.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -0.25 AND 0.25 THEN '-0.25 - 0.25' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 0.50 AND 1.00 THEN '0.50 - 1.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 1.25 AND 2.00 THEN '1.25 - 2.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 2.25 AND 3.75 THEN '2.25 - 3.75' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 4.00 AND 6.00 THEN '4.00 - 6.00' ");
                        casestring.Append("ELSE '6.25+' ");
                    } else if (_groupby == 2 || _groupby == 6) { //Cylinder
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" < -3.00 THEN 'High (-3.00+)' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -2.75 AND -1.50 THEN 'Medium (-1.50 - -2.75)' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -1.25 AND -0.75 THEN 'Low (-0.75 - -1.25)' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN -0.50 AND 0 THEN 'Trivial (0.00 - -0.50)' ");
                        casestring.Append("ELSE '-' ");
                    } else if (_groupby == 3 || _groupby == 7) { //Axis
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 0 AND 25 THEN 'With the Rule (0-25, 155-180)' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 155 AND 180 THEN 'With the Rule (0-25, 155-180)' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 26 AND 64 THEN 'Oblique (26-64, 116-154)' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 116 AND 154 THEN 'Oblique (26-64, 116-154)' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 65 AND 115 THEN 'Against the Rule (65-115)' ");
                        casestring.Append("ELSE '-' ");
                    } else { //Add
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 0.25 AND 1.00 THEN '0.25 - 1.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 1.25 AND 2.00 THEN '1.25 - 2.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 2.25 AND 3.00 THEN '2.25 - 3.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 3.25 AND 4.00 THEN '3.25 - 4.00' ");
                        casestring.Append("WHEN ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(" BETWEEN 4.25 AND 5.00 THEN '4.25 - 5.00' ");
                        casestring.Append("ELSE '0.00' ");
                    }
                    casestring.Append("END AS range ");
                    groupbystring.Append("GROUP BY t.range");
                } else if (_groupby == 9) { //Type parameters - grouped by themselves (include "B" in category "M")
                    selectstring.Append("t.types as Range, Count(*) AS Count ");
                    casestring.Append("FROM (SELECT CASE ");
                    casestring.Append("WHEN Type = 'B' THEN 'M' ELSE Type END AS types ");
                    groupbystring.Append("GROUP BY t.types");
                } else if (_groupby == 11) { //Size parameters - grouped by themselves (include "C" in category "S")
                    selectstring.Append("t.sizes as Range, Count(*) AS Count ");
                    casestring.Append("FROM (SELECT CASE ");
                    casestring.Append("WHEN Size = 'C' THEN 'S' ELSE Size END AS sizes ");
                    groupbystring.Append("GROUP BY t.sizes");
                } else if (_groupby < 13) { //Other attribute parameters - grouped by themselves
                    selectstring.Append(RLIMITS_FILTERSTRING[_groupby]).Append(", Count(*) AS Count ");
                    groupbystring.Append("GROUP BY ").Append(RLIMITS_FILTERSTRING[_groupby]);
                } else { //Date parameters - grouped by month
                    selectstring.Append("strftime(\"%m/%Y\", ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(") as 'Month/Year', Count(*) AS Count ");
                    groupbystring.Append("GROUP BY strftime(\"%m/%Y\", ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(") ORDER BY strftime(\"%Y-%m\", ").Append(RLIMITS_FILTERSTRING[_groupby]).Append(")");
                }
            } else {
                selectstring.Append("* ");
            }

            //Build sourcestring - Determined by _table parameter
            if(_table == SSTable.Current) {
                sourcestring.Append("FROM CurrentInventory ");
            } else if (_table == SSTable.Dispensed) {
                sourcestring.Append("FROM DispensedInventory ");
            } else {
                throw new ArgumentException("Invalid value for _table; must be Current or Dispensed");
            }

            //Build the filterstring (the WHERE clause)
            filterstring.Append("WHERE SKU > 0 ");

            if (_filters.Count > 0) {
                //Loop through each dictionary entry and parse it into a clause in the query string
                foreach (KeyValuePair<int, LinkedList<string>> entry in _filters) {
                    filterstring.Append("AND ( ").Append(RLIMITS_FILTERSTRING[entry.Key]);
                    if (entry.Key < 9) { //Rx or date fields - use BETWEEN and pairs of values
                        LinkedList<string>.Enumerator lliter = entry.Value.GetEnumerator();
                        lliter.MoveNext();
                        filterstring.Append(" BETWEEN ").Append(lliter.Current);
                        lliter.MoveNext();
                        filterstring.Append(" AND ").Append(lliter.Current);
                        while (lliter.MoveNext()) {
                            filterstring.Append(" OR ").Append(RLIMITS_FILTERSTRING[entry.Key]).Append(" BETWEEN ").Append(lliter.Current);
                            lliter.MoveNext();
                            filterstring.Append(" AND ").Append(lliter.Current);
                        }
                    } else if (entry.Key > 12) { //date fields - use BETWEEN and pairs of values and SQL DATE function
                        LinkedList<string>.Enumerator lliter = entry.Value.GetEnumerator();
                        lliter.MoveNext();
                        filterstring.Append(" BETWEEN DATE(\'").Append(lliter.Current).Append("\')");
                        lliter.MoveNext();
                        filterstring.Append(" AND DATE(\'").Append(lliter.Current).Append("\')");
                        while (lliter.MoveNext()) {
                            filterstring.Append(" OR DATE(\'").Append(RLIMITS_FILTERSTRING[entry.Key]).Append(") BETWEEN DATE(\'").Append(lliter.Current).Append(")");
                            lliter.MoveNext();
                            filterstring.Append(" AND DATE(\'").Append(lliter.Current).Append("\')");
                        }
                    } else { //Selection fields
                        LinkedList<string>.Enumerator lliter = entry.Value.GetEnumerator();
                        lliter.MoveNext();
                        filterstring.Append("='").Append(lliter.Current).Append("'");
                        if (entry.Key == 11 && lliter.Current == "S") { //If Size is small, include Child size in query (for legacy databases)
                            filterstring.Append(" OR ").Append(RLIMITS_FILTERSTRING[entry.Key]).Append("='C'");
                        }
                        if (entry.Key == 10 && lliter.Current == "M") { //Include "B" as another way of specifying multifocal lenses (for legacy databases)
                            filterstring.Append(" OR ").Append(RLIMITS_FILTERSTRING[entry.Key]).Append("='B'");
                        }
                        while (lliter.MoveNext()) {
                            filterstring.Append(" OR ").Append(RLIMITS_FILTERSTRING[entry.Key]).Append("='").Append(lliter.Current).Append("'");
                            if (entry.Key == 11 && lliter.Current == "S") { //If Size is small, include Child size in query (for legacy databases)
                                filterstring.Append(" OR ").Append(RLIMITS_FILTERSTRING[entry.Key]).Append("='C'");
                            }
                            if (entry.Key == 10 && lliter.Current == "M") { //Include "B" as another way of specifying multifocal lenses (for legacy databases)
                                filterstring.Append(" OR ").Append(RLIMITS_FILTERSTRING[entry.Key]).Append("='B'");
                            }
                        }
                    }
                    filterstring.Append(" ) ");
                }
            }

            combostring.Append(casestring).Append(sourcestring).Append(filterstring).Append(casestring.Length > 0 ? ") t " : "");
            sb.Append(selectstring).Append(combostring).Append(groupbystring);

            //Execute the query and store the results in the DataTable to return
            DataTable dt = new DataTable();
            SQLiteDataAdapter da;
        	using (SQLiteTransaction dbt = DBConn.BeginTransaction())
        	{
                DBcmd.CommandText = sb.ToString();
        		da = new SQLiteDataAdapter(DBcmd);
        		try {
        			da.Fill(dt);
        		}
        		catch (SQLiteException e) {
        			if(e.ErrorCode == SQLiteErrorCode.IOErr)
        				dt.Clear();
                    else {
                        throw e;
                    }
        		}
        	}

            return dt;
        }
        
        //Returns true if there's an open database connection
        public bool IsOpen()
        {
        	if(DBConn.State == ConnectionState.Open)
        		return true;
        	else
        		return false;
        }
        
        #endregion
        
        #region SSDataBase Utility Functions
        /// <summary>
        /// Search for glasses with a range of prescriptions.
        /// </summary>
        /// <remarks>When this function completes, a DataTable will be filled with the results</remarks>
        /// <param name="sprec">The SpecsRecord that you want to match as closely as possible</param>
        /// <param name="deye">Dominant eye selection</param>
        /// <param name="results">The DataTable to store the results in</param>
        private void RxSearchRange(SpecsRecord sprec, DomEye deye, DataTable results)
        {
        	float tsph, tcyl;	//Used to determine tolerances based on cylinder power
        	int taxis, atol;	//Axis tolerance
        	
        	Sparams[20].Value = sprec.Type;
        	
        	//Build the search strings and set the temporary variables
        	if(deye == DomEye.OS) //OS is dominant eye
        	{
        		DBcmd.CommandText = "SELECT * FROM CurrentInventory WHERE (Type = @type" +
                    (sprec.Type == "M" ? " OR Type = 'B'" : "") +") AND " +
	        		"((AxisOS BETWEEN @axmin1 AND @axmax1) OR (AxisOS BETWEEN @axmin2 AND @axmax2)) AND " +
	        		"(((SphereOS BETWEEN @smin0 AND @smax0) AND (CylinderOS BETWEEN @cmin0 AND @cmax0)) OR " +
        			"((SphereOS BETWEEN @smin1 AND @smax1) AND (CylinderOS BETWEEN @cmin1 AND @cmax1)) OR " +
	        		"((SphereOS BETWEEN @smin2 AND @smax2) AND (CylinderOS BETWEEN @cmin2 AND @cmax2)) OR " +
	        		"((SphereOS BETWEEN @smin3 AND @smax3) AND (CylinderOS BETWEEN @cmin3 AND @cmax3))) " +
        			"ORDER BY SKU";
        		tsph = sprec.SphereOS;
        		tcyl = sprec.CylOS;
        		taxis = sprec.AxisOS;
        	}
        	else //OD or OU is dominant (extra processing for OU later)
        	{
        		DBcmd.CommandText = "SELECT * FROM CurrentInventory WHERE (Type = @type" +
                    (sprec.Type == "M" ? " OR Type = 'B'" : "") + ") AND " +
	        		"((AxisOD BETWEEN @axmin1 AND @axmax1) OR (AxisOD BETWEEN @axmin2 AND @axmax2)) AND " +
	        		"(((SphereOD BETWEEN @smin0 AND @smax0) AND (CylinderOD BETWEEN @cmin0 AND @cmax0)) OR " +
        			"((SphereOD BETWEEN @smin1 AND @smax1) AND (CylinderOD BETWEEN @cmin1 AND @cmax1)) OR " +
	        		"((SphereOD BETWEEN @smin2 AND @smax2) AND (CylinderOD BETWEEN @cmin2 AND @cmax2)) OR " +
	        		"((SphereOD BETWEEN @smin3 AND @smax3) AND (CylinderOD BETWEEN @cmin3 AND @cmax3))) " +
        			"ORDER BY SKU";
        		tsph = sprec.SphereOD;
        		tcyl = sprec.CylOD;
        		taxis = sprec.AxisOD;
        	}
        	
        	//First set of Sparams, always + or - 0.50
        	Sparams[0].Value = tsph - 0.50;	//Sphere
        	Sparams[1].Value = tsph + 0.50;
        	Sparams[8].Value = tcyl - 0.50; //Cylinder
        	Sparams[9].Value = tcyl + 0.50;
        	
        	//Set Sparams based on cylinder power range
        	if(tcyl <= -1.50F)
        	{
        		if(tsph > 0) //Positive sphere requested
        		{
	        		Sparams[2].Value = tsph - 0.75;
	        		Sparams[3].Value = tsph + 0.25;
	        		Sparams[4].Value = tsph - 1;
	        		Sparams[5].Value = tsph;
	        		Sparams[6].Value = tsph - 1.25;
	        		Sparams[7].Value = tsph - 0.25;
        		}
        		else //Negative sphere requested
        		{
        			Sparams[2].Value = tsph - 0.25;
        			Sparams[3].Value = tsph + 0.75;
        			Sparams[4].Value = tsph;
        			Sparams[5].Value = tsph + 1;
        			Sparams[6].Value = tsph + 0.25;
        			Sparams[7].Value = tsph + 1.25;
        		}
        		Sparams[10].Value = tcyl;
        		Sparams[11].Value = tcyl + 1;
        		Sparams[12].Value = tcyl + 0.5;
        		Sparams[13].Value = tcyl + 1.5;
        		Sparams[14].Value = tcyl + 1;
        		Sparams[15].Value = tcyl + 2;
        	}
        	else if(tcyl <= -1.00F)
        	{
        		if(tsph > 0) //Positive sphere requested
        		{
	        		Sparams[2].Value = tsph - 0.75;
	        		Sparams[3].Value = tsph + 0.25;
	        		Sparams[4].Value = tsph - 1;
	        		Sparams[5].Value = tsph;
	        		Sparams[6].Value = tsph - 0.75;
	        		Sparams[7].Value = tsph + 0.25;
        		}
        		else //Negative sphere requested
        		{
        			Sparams[2].Value = tsph - 0.25;
        			Sparams[3].Value = tsph + 0.75;
        			Sparams[4].Value = tsph;
        			Sparams[5].Value = tsph + 1;
        			Sparams[6].Value = tsph - 0.25;
        			Sparams[7].Value = tsph + 0.75;
        		}
        		Sparams[10].Value = tcyl;
        		Sparams[11].Value = tcyl + 1;
        		Sparams[12].Value = tcyl + 0.5;
        		Sparams[13].Value = tcyl + 1.5;
        		Sparams[14].Value = tcyl;
        		Sparams[15].Value = tcyl + 1;
        	}
        	else if(tcyl <= -0.50F)
        	{
        		if(tsph > 0) //Positive sphere requested
        		{
	        		Sparams[2].Value = tsph - 0.75;
	        		Sparams[3].Value = tsph + 0.25;
	        		Sparams[4].Value = tsph - 0.75;
	        		Sparams[5].Value = tsph + 0.25;
	        		Sparams[6].Value = tsph - 0.75;
	        		Sparams[7].Value = tsph + 0.25;
        		}
        		else //Negative sphere requested
        		{
        			Sparams[2].Value = tsph - 0.25;
        			Sparams[3].Value = tsph + 0.75;
        			Sparams[4].Value = tsph - 0.25;
        			Sparams[5].Value = tsph + 0.75;
        			Sparams[6].Value = tsph - 0.25;
        			Sparams[7].Value = tsph + 0.75;
        		}
        		Sparams[10].Value = tcyl;
        		Sparams[11].Value = tcyl + 1;
        		Sparams[12].Value = tcyl;
        		Sparams[13].Value = tcyl + 1;
        		Sparams[14].Value = tcyl;
        		Sparams[15].Value = tcyl + 1;
        	}
        	
        	//Determine axis tolerances based on cylinder power
        	if(tcyl <= -4)
        		atol = 7;
        	else if(tcyl <= -2.5)
        		atol = 8;
        	else if(tcyl <= -2)
        		atol = 9;
        	else if(tcyl <= -1.5)
        		atol = 10;
        	else if(tcyl <= -1.25)
        		atol = 13;
        	else if(tcyl <= -1)
        		atol = 15;
        	else if(tcyl <= -0.75)
        		atol = 20;
        	else
        		atol = 25;
        	
        	//Set axis range parameters
        	//Determine if the axis range will cross the 0 or 180 degree lines, and adjust if necessary
        	if(taxis > (180-atol)) { //Split past 180
        		Sparams[16].Value = taxis - atol;
        		Sparams[17].Value = 180;
        		Sparams[18].Value = 0;
        		Sparams[19].Value = (taxis + atol) - 180;
        	}
        	else if(taxis < (atol)) { //Split past 0 
        		Sparams[16].Value = 0;
        		Sparams[17].Value = taxis + atol;
        		Sparams[18].Value = (taxis - atol) + 180;
        		Sparams[19].Value = 180;
        	}
        	else {
        		Sparams[16].Value = Sparams[18].Value = taxis - atol;
        		Sparams[17].Value = Sparams[19].Value = taxis + atol;
        	}

        	//Execute the search
        	SQLiteDataAdapter da = new SQLiteDataAdapter(DBcmd);
			try {
				da.Fill(results);
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
			}
			
			//Remove results based on gender and size selection, if applicable
			if (sprec.Gender != null && sprec.Size != null) {
                //Loop through the results and discard any that either aren't Unisex (those show up for all Gender searches) or
                //don't match the Gender selection.  Also discard those that don't match the Size selection.  Don't discard a size of "Child"
                //if the selection is "Small"
				int rcount = results.Rows.Count;
                string gender = "";
                string size = "";

				for(int i = 0; i < rcount; i++) 
                {
                    gender = (string)results.Rows[i][11];
                    size = (string)results.Rows[i][12];
                    if (gender != SpecGender.Uni && gender != sprec.Gender) {
						results.Rows[i].Delete();
                    } else if (size != sprec.Size) {
                        if (!(size == SpecSize.Child && sprec.Size == SpecSize.Small)) {
                            results.Rows[i].Delete();
                        }
                    }
				}
			} else if (sprec.Gender != null) {
                //Loop through the results and discard any that either aren't Unisex (those show up for all gender searches) or
                //don't match the gender selection
				int rcount = results.Rows.Count;
                string gender = "";

				for (int i = 0; i < rcount; i++) 
                {
                    gender = (string)results.Rows[i][11];
					if (gender != SpecGender.Uni && gender != sprec.Gender) { 
						results.Rows[i].Delete();
                    }
				}
			} else if (sprec.Size != null) {
                //Loop through the results and discard any that don't match the Size selection.  Don't discard those that are "Child" sized
                //if the selection is "Small"
				int rcount = results.Rows.Count;
                string size = "";

				for(int i = 0; i < rcount; i++) 
                {
                    size = (string)results.Rows[i][12];
					if(size != sprec.Size) {
						if (!(size == SpecSize.Child && sprec.Size == SpecSize.Small)) {
                            results.Rows[i].Delete();
                        }
                    }
				}
			}
			results.AcceptChanges();  //Makes the deletions permanent to prevent later errors

			//Additional processing for OU search
			#region OU Search Processing
			if(deye == DomEye.OU)
			{
				tsph = sprec.SphereOS;
				tcyl = sprec.CylOS;
				taxis = sprec.AxisOS;
				int rcount;
				double tsmin1, tsmin2, tsmin3, tsmin4;
				double tsmax1, tsmax2, tsmax3, tsmax4;
				double tcmin1, tcmin2, tcmin3, tcmin4;
				double tcmax1, tcmax2, tcmax3, tcmax4;
				int taxmin1, taxmin2, taxmax1, taxmax2;

				//First set of Sparams, always + or - 0.50
	        	tsmin1 = tsmin2 = tsmin3 = tsmin4 = tsph - 0.50;	//Sphere
	        	tsmax1 = tsmax2 = tsmax3 = tsmax4 = tsph + 0.50;
	        	tcmin1 = tcmin2 = tcmin3 = tcmin4 = tcyl - 0.50; //Cylinder
	        	tcmax1 = tcmax2 = tcmax3 = tcmax4 = tcyl + 0.50;
	        	
	        	//Set Sparams based on cylinder power range
	        	if(tcyl <= -1.50F)
	        	{
	        		if(tsph > 0) //Positive sphere requested
	        		{
		        		tsmin2 = tsph - 0.75;
		        		tsmax2 = tsph + 0.25;
		        		tsmin3 = tsph - 1;
		        		tsmax3 = tsph;
		        		tsmin4 = tsph - 1.25;
		        		tsmax4 = tsph - 0.25;
	        		}
	        		else //Negative sphere requested
	        		{
	        			tsmin2 = tsph - 0.25;
	        			tsmax2 = tsph + 0.75;
	        			tsmin3 = tsph;
	        			tsmax3 = tsph + 1;
	        			tsmin4 = tsph + 0.25;
	        			tsmax4 = tsph + 1.25;
	        		}
	        		tcmin2 = tcyl;
	        		tcmax2 = tcyl + 1;
	        		tcmin3 = tcyl + 0.5;
	        		tcmax3 = tcyl + 1.5;
	        		tcmin4 = tcyl + 1;
	        		tcmax4 = tcyl + 2;
	        	}
	        	else if(tcyl <= -1.00F)
	        	{
	        		if(tsph > 0) //Positive sphere requested
	        		{
		        		tsmin2 = tsph - 0.75;
		        		tsmax2 = tsph + 0.25;
		        		tsmin3 = tsph - 1;
		        		tsmax3 = tsph;
		        		tsmin4 = tsph - 0.75;
		        		tsmax4 = tsph + 0.25;
	        		}
	        		else //Negative sphere requested
	        		{
	        			tsmin2 = tsph - 0.25;
	        			tsmax2 = tsph + 0.75;
	        			tsmin3 = tsph;
	        			tsmax3 = tsph + 1;
	        			tsmin4 = tsph - 0.25;
	        			tsmax4 = tsph + 0.75;
	        		}
	        		tcmin2 = tcyl;
	        		tcmax2 = tcyl + 1;
	        		tcmin3 = tcyl + 0.5;
	        		tcmax3 = tcyl + 1.5;
	        		tcmin4 = tcyl;
	        		tcmax4 = tcyl + 1;
	        	}
	        	else if(tcyl <= -0.50F)
	        	{
	        		if(tsph > 0) //Positive sphere requested
	        		{
		        		tsmin2 = tsph - 0.75;
		        		tsmax2 = tsph + 0.25;
		        		tsmin3 = tsph - 0.75;
		        		tsmax3 = tsph + 0.25;
		        		tsmin4 = tsph - 0.75;
		        		tsmax4 = tsph + 0.25;
	        		}
	        		else //Negative sphere requested
	        		{
	        			tsmin2 = tsph - 0.25;
	        			tsmax2 = tsph + 0.75;
	        			tsmin3 = tsph - 0.25;
	        			tsmax3 = tsph + 0.75;
	        			tsmin4 = tsph - 0.25;
	        			tsmax4 = tsph + 0.75;
	        		}
	        		tcmin2 = tcyl;
	        		tcmax2 = tcyl + 1;
	        		tcmin3 = tcyl;
	        		tcmax3 = tcyl + 1;
	        		tcmin4 = tcyl;
	        		tcmax4 = tcyl + 1;
	        	}
				
				if(tcyl <= -4)
	        		atol = 7;
	        	else if(tcyl <= -2.5)
	        		atol = 8;
	        	else if(tcyl <= -2)
	        		atol = 9;
	        	else if(tcyl <= -1.5)
	        		atol = 10;
	        	else if(tcyl <= -1.25)
	        		atol = 13;
	        	else if(tcyl <= -1)
	        		atol = 15;
	        	else if(tcyl <= -0.75)
	        		atol = 20;
	        	else
	        		atol = 25;
	        	
	        	if(taxis > (180-atol)) //Split past 180
	        	{
	        		taxmin1 = taxis - atol;
	        		taxmax1 = 180;
	        		taxmin2 = 0;
	        		taxmax2 = (taxis + atol) - 180;
	        	}
	        	else if(taxis < (atol)) //Split past 0
	        	{
	        		taxmin1 = 0;
	        		taxmax1 = taxis + atol;
	        		taxmin2 = (taxis - atol) + 180;
	        		taxmax2 = 180;
	        	}
	        	else
	        	{
	        		taxmin1 = taxmin2 = taxis - atol;
	        		taxmax1 = taxmax2 = taxis + atol;
	        	}
		        	
		        rcount = results.Rows.Count;
				for (int i = 0; i < rcount; i++)
				{
					int tax = Convert.ToInt16(results.Rows[i][8]);
					double ts = Convert.ToSingle(results.Rows[i][6]);
					double tc = Convert.ToSingle(results.Rows[i][7]);
					if(!((tax >= taxmin1 && tax <= taxmax1) || (tax >= taxmin2 && tax <= taxmax2)))
					{
						results.Rows[i].Delete();
					}
					
					if(!(((ts >= tsmin1 && ts <= tsmax1) && (tc >= tcmin1 && tc <= tcmax1)) ||
					     ((ts >= tsmin2 && ts <= tsmax2) && (tc >= tcmin2 && tc <= tcmax2)) ||
					     ((ts >= tsmin3 && ts <= tsmax3) && (tc >= tcmin3 && tc <= tcmax3)) ||
					     ((ts >= tsmin4 && ts <= tsmax4) && (tc >= tcmin4 && tc <= tcmax4))))
					{
						results.Rows[i].Delete();
					}
				}
				results.AcceptChanges();
			}
			#endregion
        }
        
        //Score each record in the DBResults DataTable
        /// <summary>
        /// Score each record in the DataTable
        /// </summary>
        /// <remarks>The scores will be inserted into the DataTable that contained the unscored records</remarks>
        /// <param name="sprec">The original prescription to score against</param>
        /// <param name="deye">Dominant eye selection</param>
        /// <param name="results">DataTable that contains the unscored results, contains the scored results when function completes</param>
        private void RxScoreResults(SpecsRecord sprec, DomEye deye, DataTable results)
        {
        	int recordcount = results.Rows.Count;
        	float ds, ss; //OD and OS Sphere
        	float dc, sc; //OD and OS Cyl
        	float dax, sax; //OD and OS Axis
        	float dad, sad; //OD and OS Add
        	
        	for(int i = 0; i < recordcount; i++)
        	{
        		float score = 0;
        		int axmult = 0;

        		ds = Convert.ToSingle(results.Rows[i][2]);
        		ss = Convert.ToSingle(results.Rows[i][6]);
        		dc = Convert.ToSingle(results.Rows[i][3]);
        		sc = Convert.ToSingle(results.Rows[i][7]);
        		dax = Convert.ToInt16(results.Rows[i][4]);
        		sax = Convert.ToInt16(results.Rows[i][8]);
        		dad = Convert.ToSingle(results.Rows[i][5]);
        		sad = Convert.ToSingle(results.Rows[i][9]);
        		
        		switch (deye) {
        			case DomEye.OD :  //OD is dominant eye
        				
        				//Determine axis divisor
        				if(sprec.CylOD <= -3)
        					axmult = 8;
        				else if(sprec.CylOD <= -1.5)
        					axmult = 16;
        				else
        					axmult = 32;
        					
        				score = (Math.Abs(sprec.SphereOD - ds)) + (Math.Abs(sprec.CylOD - dc)) + 
        					((sprec.Type == SpecType.Multi) ? Math.Abs(sprec.AddOD - dad)/10 : 0) +
        					(((Math.Abs(sprec.AxisOD - dax)) >= 90 ? 180-Math.Abs((float)sprec.AxisOD - dax) : Math.Abs((float)sprec.AxisOD - dax))/axmult);
        				
        				if(sprec.SphereOD > 0)  //Positive sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOD - ds) == (dc - sprec.CylOD)/2 && 
        					   sprec.SphereOD > ds && 
        					   Math.Abs(dc - sprec.CylOD) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.55F;
        					}
        					else if(sprec.SphereOD == ds && Math.Abs(sprec.CylOD - dc) <= 0.75F &&
        					        Math.Abs(sprec.CylOD - dc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOD < ds && sprec.CylOD > dc) || (sprec.SphereOD > ds && sprec.CylOD < dc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ds - sprec.SphereOD) == Math.Abs(sprec.CylOD - dc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					
        					if(sprec.SphereOD < ds) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;	
        				}
        				else					//Negative sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOD - ds) == (dc - sprec.CylOD)/2 && 
        					   sprec.SphereOD > ds && 
        					   Math.Abs(dc - sprec.CylOD) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.50F;
        					}
        					else if(sprec.SphereOD == ds && Math.Abs(sprec.CylOD - dc) <= 0.75F &&
        					        Math.Abs(sprec.CylOD - dc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOD > ds && sprec.CylOD < dc) || (sprec.SphereOD < ds && sprec.CylOD > dc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ds - sprec.SphereOD) == Math.Abs(sprec.CylOD - dc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					
        					if(sprec.SphereOD < ds) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;
        				}
        				
        				//Correct overplussing
        				if(sprec.SphereOD > ds && sprec.SphereOD > 0)
        					score += 0.25F;
        				break;
        				
        			case DomEye.OS :  //OS is dominant eye
        				
        				//Determine axis divisor
        				if(sprec.CylOS <= -3)
        					axmult = 8;
        				else if(sprec.CylOS <= -1.5)
        					axmult = 16;
        				else
        					axmult = 32;        				
        				
        				score = (Math.Abs(sprec.SphereOS - ss)) + (Math.Abs(sprec.CylOS - sc)) +
        					((sprec.Type == SpecType.Multi) ? Math.Abs(sprec.AddOS - sad)/10 : 0) +
        					(((Math.Abs(sprec.AxisOS - sax)) >= 90 ? 180-Math.Abs((float)sprec.AxisOS - sax) : Math.Abs((float)sprec.AxisOS - sax))/axmult);
        				
        				if(sprec.SphereOS > 0)  //Positive sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOS - ss) == (sc - sprec.CylOS)/2 && 
        					   sprec.SphereOS > ss && 
        					   Math.Abs(sc - sprec.CylOS) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.55F;
        					}
        					else if(sprec.SphereOS == ss && Math.Abs(sprec.CylOS - sc) <= 0.75F &&
        					        Math.Abs(sprec.CylOS - sc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOS < ss && sprec.CylOS > sc) || (sprec.SphereOS > ss && sprec.CylOS < sc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ss - sprec.SphereOS) == Math.Abs(sprec.CylOS - sc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					if(sprec.SphereOS < ss) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;
        				}
        				else					//Negative sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOS - ss) == (sc - sprec.CylOS)/2 && 
        					   sprec.SphereOS > ss && 
        					   Math.Abs(sc - sprec.CylOS) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.50F;
        					}
        					else if(sprec.SphereOS == ss && Math.Abs(sprec.CylOS - sc) <= 0.75F &&
        					        Math.Abs(sprec.CylOS - sc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOS > ss && sprec.CylOS < sc) || (sprec.SphereOS < ss && sprec.CylOS > sc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ss - sprec.SphereOS) == Math.Abs(sprec.CylOS - sc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					
        					if(sprec.SphereOS > ss) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;
        				}
        				
        				//Correct overplussing
        				if(sprec.SphereOS > ss && sprec.SphereOS > 0)
        					score += 0.25F;
        				break;
        			
        			case DomEye.OU :  //Both eyes are dominant
        				
        				//Determine axis divisor for OD
        				if(sprec.CylOD <= -3)
        					axmult = 8;
        				else if(sprec.CylOD <= -1.5)
        					axmult = 16;
        				else
        					axmult = 32;
        				
        				//Determine OD base score
        				score = (Math.Abs(sprec.SphereOD - ds)) + (Math.Abs(sprec.CylOD - dc)) + 
        					((sprec.Type == SpecType.Multi) ? Math.Abs(sprec.AddOD - dad)/10 : 0) +
        					(((Math.Abs(sprec.AxisOD - dax)) >= 90 ? 180-Math.Abs((float)sprec.AxisOD - dax) : Math.Abs((float)sprec.AxisOD - dax))/axmult);
        				
        				if(sprec.SphereOD > 0)  //Positive sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOD - ds) == (dc - sprec.CylOD)/2 && 
        					   sprec.SphereOD > ds && 
        					   Math.Abs(dc - sprec.CylOD) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.55F;
        					}
        					else if(sprec.SphereOD == ds && Math.Abs(sprec.CylOD - dc) <= 0.75F &&
        					        Math.Abs(sprec.CylOD - dc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOD < ds && sprec.CylOD > dc) || (sprec.SphereOD > ds && sprec.CylOD < dc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ds - sprec.SphereOD) == Math.Abs(sprec.CylOD - dc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					
        					if(sprec.SphereOD < ds) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;	
        				}
        				else					//Negative sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOD - ds) == (dc - sprec.CylOD)/2 && 
        					   sprec.SphereOD > ds && 
        					   Math.Abs(dc - sprec.CylOD) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.50F;
        					}
        					else if(sprec.SphereOD == ds && Math.Abs(sprec.CylOD - dc) <= 0.75F &&
        					        Math.Abs(sprec.CylOD - dc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOD > ds && sprec.CylOD < dc) || (sprec.SphereOD < ds && sprec.CylOD > dc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ds - sprec.SphereOD) == Math.Abs(sprec.CylOD - dc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOD - dc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					
        					if(sprec.SphereOD < ds) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;
        				}
        				
        				//Correct overplussing
        				if(sprec.SphereOD > ds && sprec.SphereOD > 0)
        					score += 0.25F;
        				
        				//Determine axis divisor for OS
        				if(sprec.CylOS <= -3)
        					axmult = 8;
        				else if(sprec.CylOS <= -1.5)
        					axmult = 16;
        				else
        					axmult = 32; 
        				
        				//Determine and include OS base score
        				score += (Math.Abs(sprec.SphereOS - ss)) + (Math.Abs(sprec.CylOS - sc)) +
        					((sprec.Type == SpecType.Multi) ? Math.Abs(sprec.AddOS - sad)/10 : 0) +
        					(((Math.Abs(sprec.AxisOS - sax)) >= 90 ? 180-Math.Abs((float)sprec.AxisOS - sax) : Math.Abs((float)sprec.AxisOS - sax))/axmult);     				
        				
        				if(sprec.SphereOS > 0)  //Positive sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOS - ss) == (sc - sprec.CylOS)/2 && 
        					   sprec.SphereOS > ss && 
        					   Math.Abs(sc - sprec.CylOS) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.55F;
        					}
        					else if(sprec.SphereOS == ss && Math.Abs(sprec.CylOS - sc) <= 0.75F &&
        					        Math.Abs(sprec.CylOS - sc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOS < ss && sprec.CylOS > sc) || (sprec.SphereOS > ss && sprec.CylOS < sc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ss - sprec.SphereOS) == Math.Abs(sprec.CylOS - sc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					if(sprec.SphereOS < ss) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;
        				}
        				else					//Negative sphere
        				{
        					//Cylinder scoring
        					if((sprec.SphereOS - ss) == (sc - sprec.CylOS)/2 && 
        					   sprec.SphereOS > ss && 
        					   Math.Abs(sc - sprec.CylOS) <= 1)   //Sphereical Equivalent
        					{
        						score -= 0.50F;
        					}
        					else if(sprec.SphereOS == ss && Math.Abs(sprec.CylOS - sc) <= 0.75F &&
        					        Math.Abs(sprec.CylOS - sc) != 0)
        					{
        						score -= 0.12F;
        					}
        					else if((sprec.SphereOS > ss && sprec.CylOS < sc) || (sprec.SphereOS < ss && sprec.CylOS > sc))  //Sphere and cylinder in same direction
        					{
        						if(Math.Abs(ss - sprec.SphereOS) == Math.Abs(sprec.CylOS - sc)) //Sphere and cyl difference are equal
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.55F : 0.30F);
        						}
        						else //Not equal but still in same direction
        						{
        							score -= (Math.Abs(sprec.CylOS - sc) >= 0.5F ? 0.5F : 0.25F);
        						}
        					}
        					
        					if(sprec.SphereOS > ss) //Candidate would overcorrect, which is less ideal
        						score += 0.2F;
        				}
        				
        				//Correct overplussing
        				if(sprec.SphereOS > ss && sprec.SphereOS > 0)
        					score += 0.25F;
        				
        				break;
        		}

        		if (score < SCORE_THRESHOLD) {
                    results.Rows[i][1] = score;
                } else {
                    results.Rows[i].Delete();
                }
        	}
            results.AcceptChanges();
        }

        /// <summary>
        /// Converts the axis degree value into the equivalent 1-180 degree range since the axis represents the diameter, not the radius
        /// <example>190 degrees is equivalent to 10 degrees</example>
        /// </summary>
        /// <param name="axis">The axis measurement in degrees to convert</param>
        /// <returns>The converted degree value</returns>
        private int AxisRangeSplit(int axis)
        {
        	int i;
        	if(axis <= 180 && axis > 0)
        		i = axis;
        	else
        	{
        		i = axis % 180;
        		if (i <= 0)
        			i += 180;
        	}
        	return i;
        }
        
        #endregion

        #region Constructors

        public SSDataBase() 
        {
            DBPath = "";
            DBConn = new SQLiteConnection();
            DBcmd = DBConn.CreateCommand();
            DBParams = new SQLiteParameter[19];
            Sparams = new SQLiteParameter[NUM_SEARCH_PARAMS];
            
            DBParams[0] = new SQLiteParameter("@psku");
            DBParams[1] = new SQLiteParameter("@psod");	// OD Sphere (Min for searches, same for next 7 params)
            DBParams[2] = new SQLiteParameter("@pcod"); // OD Cylinder
            DBParams[3] = new SQLiteParameter("@paxod");// OD Axis
            DBParams[4] = new SQLiteParameter("@padod");// OD Add
            DBParams[5] = new SQLiteParameter("@psos"); // OS Sphere
            DBParams[6] = new SQLiteParameter("@pcos"); // OS Cylinder
            DBParams[7] = new SQLiteParameter("@paxos");// OS Axis
            DBParams[8] = new SQLiteParameter("@pados");// OS Add
            DBParams[9] = new SQLiteParameter("@ptype");
            DBParams[10] = new SQLiteParameter("@pgen");
            DBParams[11] = new SQLiteParameter("@psize");
            DBParams[12] = new SQLiteParameter("@ptint");
            DBParams[13] = new SQLiteParameter("@padate", DbType.Date);
            DBParams[14] = new SQLiteParameter("@pddate", DbType.Date);
            DBParams[15] = new SQLiteParameter("@pcom");
            DBParams[16] = new SQLiteParameter("@pdbname");
            DBParams[17] = new SQLiteParameter("@pdbloc");
            DBParams[18] = new SQLiteParameter("@pdbdate");
            
            Sparams[0] = new SQLiteParameter("@smin0", DbType.Decimal);
            Sparams[1] = new SQLiteParameter("@smax0", DbType.Decimal);
            Sparams[2] = new SQLiteParameter("@smin1", DbType.Decimal);
            Sparams[3] = new SQLiteParameter("@smax1", DbType.Decimal);
            Sparams[4] = new SQLiteParameter("@smin2", DbType.Decimal);
            Sparams[5] = new SQLiteParameter("@smax2", DbType.Decimal);
            Sparams[6] = new SQLiteParameter("@smin3", DbType.Decimal);
            Sparams[7] = new SQLiteParameter("@smax3", DbType.Decimal);
            Sparams[8] = new SQLiteParameter("@cmin0", DbType.Decimal);
            Sparams[9] = new SQLiteParameter("@cmax0", DbType.Decimal);
            Sparams[10] = new SQLiteParameter("@cmin1", DbType.Decimal);
            Sparams[11] = new SQLiteParameter("@cmax1", DbType.Decimal);
            Sparams[12] = new SQLiteParameter("@cmin2", DbType.Decimal);
            Sparams[13] = new SQLiteParameter("@cmax2", DbType.Decimal);
            Sparams[14] = new SQLiteParameter("@cmin3", DbType.Decimal);
            Sparams[15] = new SQLiteParameter("@cmax3", DbType.Decimal);
            Sparams[16] = new SQLiteParameter("@axmin1", DbType.Decimal);
            Sparams[17] = new SQLiteParameter("@axmax1", DbType.Decimal);
            Sparams[18] = new SQLiteParameter("@axmin2", DbType.Decimal);
            Sparams[19] = new SQLiteParameter("@axmax2", DbType.Decimal);
            Sparams[20] = new SQLiteParameter("@type");
            
            //Set up the DBCmd member with some blank parameters
            for (int i = 0; i < 19; i++)
                DBcmd.Parameters.Add(DBParams[i]);
            
            //Set up the DBcmd member with blank search parameters
            for (int i = 0; i < NUM_SEARCH_PARAMS; i++)
            	DBcmd.Parameters.Add(Sparams[i]);
            
            //Set up the results data table (need to do it ahead of time mostly for sorting)
            //Column names are used as the DataPropertyName in the data grid view
            //and the SQL database column names
            dbresults = new DataTable();
            dbresults.Columns.Add(new DataColumn("SKU", System.Type.GetType("System.Int16")));
            dbresults.Columns.Add(new DataColumn("Score", System.Type.GetType("System.Single"))); //Does not correspond to field in the sql database
            dbresults.Columns.Add(new DataColumn("SphereOD", System.Type.GetType("System.Single")));
            dbresults.Columns.Add(new DataColumn("CylinderOD", System.Type.GetType("System.Single")));
            dbresults.Columns.Add(new DataColumn("AxisOD", System.Type.GetType("System.Int16")));
            dbresults.Columns.Add(new DataColumn("AddOD", System.Type.GetType("System.Single")));
            dbresults.Columns.Add(new DataColumn("SphereOS", System.Type.GetType("System.Single")));
            dbresults.Columns.Add(new DataColumn("CylinderOS", System.Type.GetType("System.Single")));
            dbresults.Columns.Add(new DataColumn("AxisOS", System.Type.GetType("System.Int16")));
            dbresults.Columns.Add(new DataColumn("AddOS", System.Type.GetType("System.Single")));
            dbresults.Columns.Add(new DataColumn("Type", System.Type.GetType("System.String")));
            dbresults.Columns.Add(new DataColumn("Gender", System.Type.GetType("System.String")));
            dbresults.Columns.Add(new DataColumn("Size", System.Type.GetType("System.String")));
            dbresults.Columns.Add(new DataColumn("Tint", System.Type.GetType("System.String")));
            dbresults.Columns.Add(new DataColumn("DateAdded", System.Type.GetType("System.DateTime")));
            dbresults.Columns.Add(new DataColumn("DateDispensed", System.Type.GetType("System.DateTime")));
            dbresults.Columns.Add(new DataColumn("Comment", System.Type.GetType("System.String")));

            //Initialize the other permanent DataTables with the same schema as dbresults
            tresults = dbresults.Clone();
            invresults = dbresults.Clone();
            invresults.Columns.RemoveAt(1);
        }

        ~SSDataBase()
        {
            if (DBConn.State == System.Data.ConnectionState.Open)
                DBConn.Close();
        }
        #endregion

        #region DEBUG

        //DEBUG FUNCTION
        public void TestDB()
        {
        	dbresults.Clear();
            DBcmd.CommandText = @"SELECT * FROM CurrentInventory";
            SQLiteDataAdapter da = new SQLiteDataAdapter(DBcmd);
            
            da.Fill(dbresults);

            for (int i = 0; i < dbresults.Rows.Count; i++)
                Console.WriteLine("{0}, {1}, {2}", dbresults.Rows[i][0], dbresults.Rows[i][1], dbresults.Rows[i][2]);
        }

        #endregion
    }
}
