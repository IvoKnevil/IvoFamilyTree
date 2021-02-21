using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using IvoFamilyTree.Utilities;
using System.Data;
using System.Diagnostics;

namespace IvoFamilyTree.Utilities
{
    class Menu
    {

        private static List<string> programMenu = new List<string>() { "Show all names starting with chosen letter", "Add person", "Remove person", "Change person", "Exit program" };
        private static int userChoice;
        private static string userInputDescription;
        private static string inputText;
        private static object[] menuChoiceToReturn = new object[2];


        static public void ProgramMenu()  //Prints program menu

        {
            for (int i = 0; i < programMenu.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {programMenu[i]}");
            }
        }


        static public object[] UserMenuChoice() //method reads users choice (choice=int, choice description=string), adds these to the array MenuChoiceToReturn. The array is sent back to the Database class. 
        {

            Console.Write("\nWhat would you like to do? ");
            inputText = Console.ReadLine();

            if (!string.IsNullOrEmpty(inputText)) //Handles exceptions if user misses to make a choice in program menu
            {
                userChoice = Convert.ToInt32(inputText);
            }
            else
            {
                ReturnErrorMsg(0);
            }


            Console.Clear();

            menuChoiceToReturn[0] = userChoice;

            if (userChoice > 0 && userChoice <= programMenu.Count) //if-else condition for error handling (user making choices out of menu range).
            {
                userInputDescription = programMenu[userChoice - 1];
            }
            else
            {
                userInputDescription = ReturnErrorMsg(userChoice);
            }

            menuChoiceToReturn[1] = userInputDescription;

            return menuChoiceToReturn;

        }




        static public string ReturnErrorMsg(int userInput)
        {

            return $"choice nr {userInput} doesn't exist at the moment.\nSo you dont get dissapointed that you choice got you " +
                        $"nowhere, here is a website you should visit.\n\n ivonazlic.com";
        }



    }
}
