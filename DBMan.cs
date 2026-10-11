using Microsoft.Data.Sqlite;
using Controller;


namespace DBManagement
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
    public SKU(string name, int skuID,  DateTime expDate , int nextID=0, int editOption = 0)
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
        public string GiveInfo()
        {
            string info;
            info= $"ID: {skuID} | Name: {name} | Exp: {expDate} | QTY:NA ";
            return info;

        }
        public static SKU failItem = new SKU("rgb(221, 174, 170) BADENTRY",01010101,DateTime.MinValue,0,0);
        public static SKU readItem = new SKU("rgb(170, 221, 178) NOCHANGE",00000000,DateTime.MaxValue,0,0);
    }

    //Acesss SQL Data Base
    public class Data
    {   
        //Creates reference to local DB file location
        private const string ConnectionString = "Data Source=/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/DBCRUD.db";

        public void initialiseDB()
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
    


        public void WritetToDB(SKU item, User user)
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

                // Ledger.Action uAction = new Ledger.Action(item,item,$"{user.userName} added id: {item.giveID()}| ItemName:{item.giveName()} to inventory.");
                // uAction.UpdateAction();
                
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


        public void readDB()
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

        public static SKU readSKUFromDB(int searchID)
        { 
            try
            {
                using(var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();

                    const string readInvetory = 
                    """
                    SELECT * FROM Inventory
                    Where IDnum = @ID;
                    """;

        
                    using(var command = new SqliteCommand(readInvetory,connection))
                    {   command.Parameters.AddWithValue("@ID", searchID);
                        var reader = command.ExecuteReader();
                        while(reader.Read())
                        {
                           int  id = Support.ToInterger(reader["IDnum"].ToString());
                           string? name = reader["Name"].ToString();
                           if (name == null)
                            {
                                name="null";
                            }
                           SKU item = new SKU(name, id, DateTime.Today);
                           return item;
                         }
                    }
                }
            }
            catch (SqliteException ex )
            {
                Console.WriteLine($" Error: {ex}");
            }
            return SKU.failItem;
        }
                    

        public void updateDB(SKU item)
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
                     const string updateExpDate = 
                    """
                    UPDATE Inventory
                    SET Expiry = @exp 
                    WHERE IDnum = @ID; 
                    """;

                    //update Name with option 1
                    if(item.giveOption() == 1)
                    {
                        Console.WriteLine("name changed");
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
                    //update Date 
                    if(item.giveOption() == 3)
                    {
                     using(var command = new SqliteCommand(updateExpDate,connection))
                        {
                            command.Parameters.AddWithValue("@ID",item.giveID());
                            command.Parameters.AddWithValue("@exp",item.giveExpDate().ToString());
                            command.ExecuteNonQuery();
                        }
                    }
                    
                }  
            }catch (SqliteException ex )
                {
               // Console.Clear();
                Console.WriteLine(ex.Message);
                }
        }

        public SKU GetQueryforUpdateDB()
        {       

            //Put options into console
            Console.WriteLine("[1]Update Name \t [2]Update ID \t [3]Update Expiry \t[4]Return");
            int input = Support.ToInterger(Console.ReadLine());

            //Update Name
            switch (input)
            {
                //Updated Name
                case 1:
                    //Collect item name
                    Console.WriteLine("Enter item ID");
                    int skuID = Support.ToInterger(Console.ReadLine());
                    if(skuID == 0)
                    {
                        Console.WriteLine("input Must be INTERGER > 0!");
                        skuID = Support.ToInterger(Console.ReadLine());
                    }

                    //Collect item ID
                    Console.WriteLine("Enter New item Name");
                    string? skuName = Console.ReadLine();
                    if(skuName == null)
                    {
                        skuName = "unamed";
                    }

                    SKU item = new SKU(skuName,skuID,DateTime.Today,0,1);

                return item;

                //Update ID
                case 2:
               
                    Console.WriteLine("Enter current item ID");
                    skuID = Support.ToInterger(Console.ReadLine());

                    //Enter new id
                    Console.WriteLine("Enter New item ID");
                    int nextID = Support.ToInterger(Console.ReadLine());
                    if(nextID == 0)
                    {
                        Console.WriteLine("input Must be INTERGER > 0!");
                        nextID = Support.ToInterger(Console.ReadLine());
                    }

                     item = new SKU("tempy",696969,DateTime.Today,0,2);
                return item;
                    
                case 3:
                    Console.WriteLine("Enter current item ID");
                        skuID = Support.ToInterger(Console.ReadLine());
                        if(skuID == 0)
                        {
                            Console.WriteLine("input Must be INTERGER > 0!");
                            skuID = Support.ToInterger(Console.ReadLine());
                        }
                    item = new SKU("tempy",skuID,DateTime.Today,0,3);
                return item;

                 //if(input== 4)
                
                //     Console.Clear();
                //     return SKU.failItem; 
            }
            
            
            return SKU.failItem;
            
        }
         public void DeleteSKUFromDB(int ID)
        { 
            try
            {
                using(var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();

                    const string deleteSKU = 
                    """
                    DELETE FROM Inventory
                    WHERE IDnum = @ID; 
                    """;

                    //update Name with option 1
                    using(var command = new SqliteCommand(deleteSKU,connection))
                    {
                        command.Parameters.AddWithValue("@ID",ID);
                        command.ExecuteNonQuery();
                    }
                    
                }  
            }catch (SqliteException ex )
                {
               // Console.Clear();
                Console.WriteLine(ex.Message);
                }
        }
    }
}

                
//             }catch (SqliteException ex )
//             {
//                // Console.Clear();
//                 Console.WriteLine(ex.Message);
//             }
//          //  return output;
//         }
 
//     }
// }