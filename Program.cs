using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using crudDATA;

namespace pSuiteTest
{

    class PROGRAM
    {
        //Converts strings to integer, interger cannot be 0 
        static int ToInterger(string? x)
        {
            int intergerOut=1;

            // Attempts conversion; returns false instead of crashing if it fails
            if (int.TryParse(x, out int result))
            {
              intergerOut = result;
            }
            else
            {
                intergerOut = 0;
            }

            return intergerOut;
        }

        /// <summary>
        /// Gets input necesarry to create an instance of an SKU and returns that instance.
        /// </summary>
        /// <returns></returns>
        static SKU GetSKUData()
        {
            //Collect item name
            Console.WriteLine("Enter item ID");
            int skuID = ToInterger(Console.ReadLine());
            if(skuID == 0)
            {
                Console.WriteLine("input Must be INTERGER > 0!");
                skuID = ToInterger(Console.ReadLine());
            }

            Console.WriteLine("Enter item Name");
            string? skuName = Console.ReadLine();
            if(skuName == null)
            {
                skuName = "unamed";
            }

            //Console.WriteLine("Enter Exp Date");
            DateTime skuEXP = DateTime.Today;

            SKU item = new SKU(skuName,skuID,skuEXP);
            return item;  
        }

        //Update Databse Basic
        static SKU UpdateSKUData()
        {       
                //SKU stored with option 0 but is not written to DB check DB management logic
                SKU failCase = new SKU("BIN",6968,DateTime.Today,0,0);


                //Put options into console
                Console.WriteLine("[1]Update Name \t [2]Update ID \t [3]Update Expiry \t[4]Return");
                int input = ToInterger(Console.ReadLine());

                //Update Name
                if(input == 1)
                {
                    //Collect item name
                    Console.WriteLine("Enter item ID");
                    int skuID = ToInterger(Console.ReadLine());
                    if(skuID == 0)
                    {
                        Console.WriteLine("input Must be INTERGER > 0!");
                        skuID = ToInterger(Console.ReadLine());
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
                }

                //Update ID
                if(input == 2)
                {

                    Console.WriteLine("Enter current item ID");
                    int skuID = ToInterger(Console.ReadLine());
                    if(skuID == 0)
                    {
                        Console.WriteLine("input Must be INTERGER > 0!");
                        skuID = ToInterger(Console.ReadLine());
                    }

                    //Enter new id
                    Console.WriteLine("Enter New item ID");
                    int nextID = ToInterger(Console.ReadLine());
                    if(nextID == 0)
                    {
                        Console.WriteLine("input Must be INTERGER > 0!");
                        nextID = ToInterger(Console.ReadLine());
                    }

                    SKU item = new SKU("tempy",696969,DateTime.Today,0,2);
                    return item;
                }

                if(input== 4)
                {
                    Console.Clear();
                    return failCase; 
                }
                
               
                return failCase;
        }

        static void Main()
        {
            string starttext = @"
                                                    $$$$$$$\         $$$$$$\  $$\   $$\ $$$$$$\ $$$$$$$$\ $$$$$$$$\ 
                                                    $$  __$$\       $$  __$$\ $$ |  $$ |\_$$  _|\__$$  __|$$  _____|
                                                    $$$ |  $$| ____ $$ /  \__|$$ |  $$ |  $$ |     $$ |   $$ |      
                                                    $$$$$$$$$|      \$$$$$$\  $$ |  $$ |  $$ |     $$ |   $$$$$\    
                                                    $$  ____/        \____$$\ $$ |  $$ |  $$ |     $$ |   $$  __|   
                                                    $$ |            $$\   $$ |$$ |  $$ |  $$ |     $$ |   $$ |      
                                                    $$ |            \$$$$$$  |\$$$$$$  |$$$$$$\    $$ |   $$$$$$$$\ 
                                                    \__|             \______/  \______/ \______|   \__|   \________|
            ";
            //Create DB
            Data.initialiseDB();

            // Display Message
            Console.Clear();
        
            bool runProgram = true;
            while (runProgram)
            {
                //Basic Program Instructions
                
                Console.WriteLine(starttext);
                Console.WriteLine("[1] View inventory \t [2] Add item to invetory \t [3]Update item in inventory \t[4] Delete items from inventory \t [5] Exit Program");
                string? input = Console.ReadLine();
                if(input == "1")
                {
                    Console.Clear();
                    Console.WriteLine("###############################################################################################################");
                    Data.readDB();
                    Console.WriteLine("###############################################################################################################");
                    
                }
                else if (input == "2")
                {
                    //Write changes to DB  
                    Data.writetToDB(GetSKUData());
                }
                else if (input == "4")
                {
                        
                }
                else if (input == "5")
                {
                    Console.Clear();
                    Console.WriteLine("##P -Suite closed##");
                    runProgram = false;
                }



        
                while( input=="3")
                {
                    Data.updateDB(UpdateSKUData());
                    input ="0";
                   
                }
                        
            }
        }
    }
}