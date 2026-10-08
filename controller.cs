using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using crudDATA;
using System.IO;


namespace Ledger
{
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
                int userInput = pSuiteTest.PROGRAM.ToInterger(Console.ReadLine());
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
            Console.WriteLine("###############################################################################################################");
            Console.WriteLine("LEDGER VIEW");
            string readText = File.ReadAllText("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt");
            Console.WriteLine(readText);
            Console.WriteLine("###############################################################################################################");
        }
        
    }
    public class Action
    {   
        public string userAction;
        public SKU oldInfo;
        public SKU newInfo;
        List<Action> actionList= new List<Action>();

        public Action(SKU oldInfo,SKU newInfo, string action="Empty")
        {
            this.userAction = action;
            this.oldInfo = oldInfo;
            this.newInfo = newInfo;
        }

         public void  UpdateAction()
         {
            //pull query
            this.oldInfo = Query(this.newInfo.giveID());
            Action updatedAction = new Action(oldInfo, newInfo, userAction);
            actionList.Add(updatedAction);
            //Console.WriteLine("Elements in the list:");
            if (File.Exists("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt"))
            {
                foreach (Action action in actionList){
                    File.AppendAllText("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt", action.userAction + Environment.NewLine);
                    }
            }
            else
            {
                File.Create("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt");
                foreach (Action action in actionList){
                    File.WriteAllText("/Users/jbox/Documents/doNetApps/c#/pSuiteTestApp/ledger.txt", action.userAction + Environment.NewLine);
                    }
            }
            
            
        }
       
        private SKU Query(int skuID)
        {
            SKU queriedItem = Data.readItemFromDB(skuID);
           return queriedItem;
        }
        // public string giveINfo()
        // {
        //     string actions = $"{this.userAction} + {oldInfo}";
        //     return actions;
        // }


    }


}