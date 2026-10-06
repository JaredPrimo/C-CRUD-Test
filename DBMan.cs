using Microsoft.Data.Sqlite;

namespace crudDATA
{

    // Basic Data type for info
    public class SKU
    {
        public int skuID;
        public int nextID;
        public string name;
        public DateTime expDate;
        public int editOption;
      
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="skuID"></param>
    /// <param name="expDate"></param>
    /// <param name="editOption"> 1: Name 2:ID 3:Expiry 4:Delete</param>
    public SKU(string name, int skuID,  DateTime expDate, int nextID=0, int editOption = 0)
        {
   
        this.skuID = skuID; 
        this.name  = name;
        this.expDate = expDate;
        this.editOption = editOption;
        this.nextID = nextID;

        }
        public int giveID()
        {
        return skuID;
        }

        public string giveName()
        {
        return name;
        }
        
        public DateTime giveExpDate()
        {
        return expDate;
        }
        public int giveOption()
        {
            return editOption;
        }
        public int getNextID()
        {
            return nextID;
        }
       
    }

    //Acesss SQL Data Base
    public static class Data
    {   
        //Creates reference to local DB file location
        private const string ConnectionString = "Data Source=/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/DBCRUD.db";

        public static void initialiseDB()
        {
           //Connect to DB and initalise table
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                const string createSKUTableQuery = """
                CREATE TABLE IF NOT EXISTS Inventory(
                    IDnum INTEGER PRIMARY KEY UNIQUE,
                    Name TEXT NOT NULL, 
                    Expiry TEXT NOT NULL);
                """;

                //execute table creation command.
                using (var command = new SqliteCommand(createSKUTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }      
                
                //simple confirmation messaage
                // Console.Clear();
                // Console.WriteLine("DB Created and SKU data inserted");
                connection.Close();  
            
            //catch and share error message on failure
            } catch (SqliteException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
            }
        }
        public static void writetToDB(SKU item)
        {
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                const string insertSKUTableData = 
                """
                INSERT OR IGNORE INTO Inventory (IDnum,Name,Expiry)
                VALUES (@ID,@name,@expdate);
                """;

                using(var command = new SqliteCommand(insertSKUTableData,connection))
                {
                    command.Parameters.AddWithValue("@ID",item.giveID());
                    command.Parameters.AddWithValue("@name",item.giveName());
                    command.Parameters.AddWithValue("@expdate",item.giveExpDate());
                    command.ExecuteNonQuery();
                }     
                
            }catch (SqliteException ex )
            {
               // Console.Clear();
                Console.WriteLine(ex.Message);
            }
           
        }
        public static void readDB()
        { 
            try
            {
                using(var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();

                    const string readInvetory = 
                    """
                    SELECT * FROM Inventory;
                    """;

        
                    using(var command = new SqliteCommand(readInvetory,connection))
                    {
                        var reader = command.ExecuteReader();
                        while(reader.Read())
                        {
                           var id = reader.GetString(0);
                           var name = reader.GetString(1);
                           var exp  = reader.GetString(2);
                           
                           Console.WriteLine($"{id} | {name} | {exp}");
                         }
                    }
                }     
                
            }catch (SqliteException ex )
            {
               // Console.Clear();
                Console.WriteLine(ex.Message);
            }
         //  return output;
        }
        public static void updateDB(SKU item)
        { 
            try
            {
                using(var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();

                    const string updateName = 
                    """
                    UPDATE Inventory
                    SET Name = @name
                    WHERE IDnum = @ID; 
                    """;

                    const string updateID = 
                    """
                    UPDATE Inventory
                    SET IDnum = @nextID 
                    WHERE IDnum = @ID; 
                    """;

                    //update Name with option 1
                    if(item.giveOption() == 1)
                    {
                     using(var command = new SqliteCommand(updateName,connection))
                        {
                            command.Parameters.AddWithValue("@ID",item.giveID());
                            command.Parameters.AddWithValue("@name",item.giveName());
                            command.ExecuteNonQuery();
                        }
                    }
                    //update ID 
                    if(item.giveOption() == 2)
                    {
                     using(var command = new SqliteCommand(updateID,connection))
                        {
                            command.Parameters.AddWithValue("@ID",item.giveID());
                            command.Parameters.AddWithValue("@name",item.giveName());
                            command.Parameters.AddWithValue("@nextID",item.getNextID());
                            command.ExecuteNonQuery();
                        }
                    }
                    // //update ID 
                    // if(item.giveOption() == 0)
                    // {
                    //  Console.WriteLine("Bad Input");
                    // }
                    
                }     
                
            }catch (SqliteException ex )
            {
               // Console.Clear();
                Console.WriteLine(ex.Message);
            }
         //  return output;
        }
 
    }
}