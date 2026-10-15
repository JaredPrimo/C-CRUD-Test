using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using viewModel; 
using System;
using Controller;
using DBManagement;
using System.Data.Common;


namespace Session
{
    class CurrentSession
    {   
        //Creates new user View and adds user
        static View display = new View();
        static User admin = new User();
        static Controller.Action userAction = new Controller.Action();

        //Program related Variables
        static bool runProgram = true;
 
        static void Main()
        {
            Console.Clear();
            admin.startSession();
            admin.WriteSessionToLedger();
            userAction.db.initialiseDB();
            
            while(runProgram)
            {
                //Start text
                display.displaySessionStartText(); 
 
                //Handles User input -> Checks permissions and Writes changes to DB
                int userInput = Support.ToInterger(Console.ReadLine());
                runProgram =  userAction.interactDB(userInput,admin);;
                
            }
        }
    }
    
}