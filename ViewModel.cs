//using crudDATA;
using Controller;

namespace viewModel
{

    class View
    {   
       
        public void displaySessionStartText()
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
            //Basic Program Instructions   
            Console.WriteLine(starttext);
            Console.WriteLine("[1] Add Item to Inventory \t [2] Read Inventory \t [3]Update item in inventory \t[4] Delete items from inventory \n[5] View Ledger \t [6] Exit Program ");
        }

        }
    }