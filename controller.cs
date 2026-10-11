using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using DBManagement; 


namespace Controller
{
    public class Action
    {
        //Initailise DB
        public Data db = new Data();

        //Keeps reference to the current user 
        private User? user;

        //Keep track of user actions
        Movement movement = new Movement();
        public bool runProgram;


         public bool interactDB(int userInput, User user)
        {
            this.user = user;
            
            switch (userInput)
            {
                //Create
                case 1:
                    //CheckPermissions(User){} if user is allowed make change. 
                    Console.WriteLine("Please Enter ITEM ID");
                    int SKUid = Support.ToInterger(Console.ReadLine());

                    Console.WriteLine("Please Enter ITEM NAME");
                    string? SKUName = Console.ReadLine();
                    if(SKUName == null)
                    {
                        SKUName = "unnamed";
                    }

                    SKU sku = new SKU(SKUName,SKUid,DateTime.Today);
                    //Writes changes to db
                    db.WritetToDB(sku,this.user);

                    //Write Changes to ledger
                    movement.TakeMovement(sku,$"{user.userName} Added to db -> \n {sku.GiveInfo()}");
                    movement.UpdateAction(userInput, movement);
                return true;
            
                //Read
                case 2:
                // Display the elements in the list
                    Console.Clear();
                    Console.WriteLine("###############################################################################################################");
                    Console.WriteLine("Inventory View");
                    db.readDB();
                    Console.WriteLine("###############################################################################################################");
                    
                    movement.TakeMovement(SKU.readItem,$"{user.userName} Read DB");
                    movement.UpdateAction(userInput,movement);
                return true;
                
                //Update
                case 3:
                    sku = db.GetQueryforUpdateDB();
                    movement.TakeMovement(sku,$"{user.userName} Udated DB:");
                    movement.UpdateAction(userInput, movement);
                    db.updateDB(sku);
                
                    Console.Clear();

                return true;

                //Delete
                case 4:
                    Console.WriteLine("Please enter the ID of the item to be deleted");
                    userInput = Support.ToInterger(Console.ReadLine());
                    sku = new SKU("Deleted",userInput,DateTime.Today);
                    movement.TakeMovement(sku, $"{user.userName} Deleted");
                    movement.UpdateAction(4,movement);
                    db.DeleteSKUFromDB(userInput);
                    
                //Codeblock 
                return true;

                //View ledger
                case 5:
                movement.viewLedger();
                return true;

                //Exit Program
                case 6:
                    Console.WriteLine("Program exitied ");
                return false;
                
            }
            return true;
        }

    }
    public class User
    {
        public string? userName{ get; private set;}
        public int sessionCount= 0;
        private int password=1111; 
        public void startSession()
        {
            bool notLoggedIn = true;
            while(notLoggedIn)
            {
                Console.Write("Enter User Name: "); 
                this.userName = Console.ReadLine();

                Console.Write("PLease enter password: ");
                
                int userInput = Support.ToInterger(Console.ReadLine());
                if(password != userInput)
                {
                    Console.WriteLine("Wrong password try again");
                }
                else if(userInput == password)
                {
                    notLoggedIn = false;
                    Console.WriteLine("Session Started");
                }
                else
                {
                    notLoggedIn = false;
                    Console.WriteLine("Session Started");
                }
            }
        

        }
         public void viewLedger()
        {
            // Display the elements in the list
            Console.Clear();
            Console.WriteLine("###############################################################################################################");
            Console.WriteLine("LEDGER VIEW");
            string readText = File.ReadAllText("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt");
            Console.WriteLine(readText);
            Console.WriteLine("###############################################################################################################");
        }
        
    }

    public class Movement{   
        public string? userAction;
        public SKU? oldInfo;
        public SKU? newInfo;
        List<Movement> ledger= new List<Movement>();

        public void TakeMovement(SKU newInfo, string userAction="Empty")
        {
            this.userAction = userAction;
            this.newInfo = newInfo;
        }

         public void  UpdateAction(int actionType, Movement currentMovement)
         {
            if(newInfo != null && userAction != null)
            {
                switch (actionType)
                {
                    case 1:
                        //ADDED ITEM TO DB No NEED TO record oldSKU info
                        ledger.Add(currentMovement);
                    break;

                    case 2:
                        //Read item from DB no need to record current oldSKU info
                        ledger.Add(currentMovement);

                    break;

                    case 3:
                        //Updated DB
                        //pull query
                        oldInfo = Query(newInfo.giveID());

                        currentMovement.userAction = $"{currentMovement.userAction}\n\t{oldInfo.GiveInfo()}\n\tto\n\t{newInfo.GiveInfo()}";
                        
                        // Ledger.l updatedAction = new Ledger.ledgerEntry(oldInfo, newInfo, userAction);
                        ledger.Add(currentMovement);
                    break;

                    case 4:
                        //Delete Item from DB
                        //pull query
                        oldInfo = Query(newInfo.giveID());

                        currentMovement.userAction = $"{currentMovement.userAction} {oldInfo.GiveInfo()}";
                        ledger.Add(currentMovement);
                    break;
                }
                    //Write ledger changes to file.
                    if (File.Exists("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt"))
                    {
                        File.AppendAllText("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt", userAction + Environment.NewLine);
                    }
                    else
                    {
                        File.Create("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt");
                        File.WriteAllText("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt", userAction + Environment.NewLine);    
                    }       
            }
            else
            {
                newInfo = SKU.failItem;
                userAction ="Empty";
                Console.WriteLine("Handling error failed to gather SKU and USER action");
            }
             
            
        }
      //  private void WriteTOLedger()
    
       
        private SKU Query(int skuID)
        {
            SKU queriedItem = Data.readSKUFromDB(skuID);
            return queriedItem;
        }

         public void viewLedger()
        {
            // Display the elements in the list
            Console.Clear();
            Console.WriteLine("###############################################################################################################");
            Console.WriteLine("LEDGER VIEW");
            string readText = File.ReadAllText("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt");
            Console.WriteLine(readText);
            Console.WriteLine("###############################################################################################################");
        }

    }
    // public class sqlQueries
     
    public class Support()
    {
        
     //Converts strings to integer, interger cannot be 0 
        public static int ToInterger(string? x)
        {
            int integerOut =0 ;
            bool checking = true;
            
            {
                // Attempts conversion; returns false instead of crashing if it fails
                if (int.TryParse(x, out int result))
                {
                    int intergerOut = result;
                    return intergerOut;
                }
                else
                {
                    while(checking)
                    {
                        Console.WriteLine("Internal Error: Input must be integer");
                        string? newInput= Console.ReadLine();
                        if (int.TryParse(newInput, out int resultx))
                        {
                            int intergerOut = resultx;
                            return intergerOut;
                        }
                    }
                    
                }
            }
            
            return integerOut;
        }
    }
        
    
}