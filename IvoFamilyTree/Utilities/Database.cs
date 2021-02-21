using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using IvoFamilyTree.Utilities;
using System.Data;
using System.Diagnostics;

namespace IvoFamilyTree.Utilities
{
    class Database
    {
        private bool keepPlaying = true;
        private string connectionString { get; set; } = @"Data Source=.\SQLExpress;Integrated Security=true";

        private string databaseName { get; set; } = "FamilyTree";

        private string tableName { get; set; } = "People";

        public void StartProgram()
        {

            while (keepPlaying)
            {

                AddMockData();
                keepPlaying = false;
                ShowMenu(); //Calls method to show the main menu
                ProgramActions(Menu.UserMenuChoice()); //calls method that takes user menu choice. Sends the info to method PlayerMenuChoice in the class Menu.
            }

        }


        private void ProgramActions(object[] userMenuChoice)
        {


            Console.WriteLine($"You've chosen {userMenuChoice[1]}\n");
            switch (userMenuChoice[0])
            {
                //Character goes on adventure if user chooses the first option in the menu. 
                case 1:
                    GetPersonInfo(2);
                    ClearScreen();
                    break;

                //the user choses to look add person.
                case 2:
                    AddPerson();
                    ClearScreen();
                    break;

                case 3:

                    ClearScreen();
                    break;


                case 4:

                    break;

                case 5:
                    keepPlaying = false;  //End program
                    break;

                default:
                    ClearScreen();
                    break;

            }

        }




        private void AddMockData()
        {
            if (!DoesDbExist())

            {
                CreateDb();
                CreateTable();
                AddTableData();
            }

            else
            {
                Console.WriteLine("Data allready exists");
                ClearScreen();
            }


        }


        private void ExecuteSQL(string sql)
        {

            using (var conn = new SqlConnection(connectionString))

            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

        }


        private bool DoesDbExist()
        {
            var theDB = GetDataTable("SELECT * FROM master.dbo.sysdatabases WHERE name = '" + databaseName + "'");
            return theDB?.Rows.Count > 0;
        }

        private void CreateDb()
        {
            string sql = "CREATE DATABASE " + databaseName;
            ExecuteSQL(sql);
            Console.WriteLine($"Database {databaseName} created");
        }


        private void CreateTable()
        {
            string sql = @$"USE [{databaseName}]
                                CREATE TABLE 
                                [dbo].[{tableName}](
            	                [ID] [int] IDENTITY(1,1) NOT NULL,
	                            [firstName] [nvarchar](50) NULL,
	                            [lastName] [nvarchar](50) NULL,
	                            [mother] [int] NULL,
	                            [father] [int] NULL,
	                            [birthYear] [int] NOT NULL
                                ) ON [PRIMARY]";

            ExecuteSQL(sql);
            Console.WriteLine($"Table {tableName} created");
        }


        private void AddTableData()
        {
            AddData("Clint", "Eastwood", 0, 0, 1925);
            AddData("Anna", "Eastwood", 0, 0, 1935);
            AddData("Arnold", "Schwarzenegger", 0, 0, 1945);
            AddData("Gloria", "Schwarzenegger", 0, 0, 1952);
            AddData("Richard", "Spelling", 0, 0, 1940);
            AddData("Maria", "Spelling", 0, 0, 1945);
            AddData("Bob", "Ferguson", 0, 0, 1943);
            AddData("Ornela", "Ferguson", 0, 0, 1944);
            AddData("Adolfo", "Dos Santos", 0, 0, 1940);
            AddData("Maria", "Dos Santos", 0, 0, 1940);
            AddData("John", "Eastwood", 2, 1, 1962);
            AddData("Laura", "Eastwood", 2, 1, 1962);
            AddData("Anna", "Schwarzenegger", 4, 3, 1972);
            AddData("George", "Spelling", 6, 5, 1973);
            AddData("Lina", "Ferguson", 8, 7, 1974);
            AddData("Patrick", "Dos Santos", 10, 9, 1976);
            AddData("Tim", "Eastwood", 13, 11, 1980);
            AddData("Lucia", "Dos Santos", 12, 16, 1982);
            AddData("Ben", "Spelling", 15, 1, 1982);

            Console.WriteLine("Added data to the table");
        }


        private void AddData(string firstName, string lastName, int mother, int father, int birthYear)
        {

            using (var conn = new SqlConnection(connectionString))
            {

                var sql = $"Insert Into {databaseName}.[dbo].[{tableName}] (firstName, lastName, mother, father, birthYear) VALUES (@firstName, @lastName, @mother, @father, @birthYear);";
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@mother", mother);
                    cmd.Parameters.AddWithValue("@father", father);
                    cmd.Parameters.AddWithValue("@birthYear", birthYear);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

        }


        private DataTable GetDataTable(string sql)
        {
            var dt = new DataTable();
            var connString = string.Format(connectionString, databaseName);

            using (var conn = new SqlConnection(connString))

            {
                conn.Open();
                using (var command = new SqlCommand(sql, conn))
                {


                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }

                }
                conn.Close();

            }

            return dt;
        }


        private void AddPerson()
        {
            Console.Write("Please enter the first name of the person you want to add: ");
            string firstName = Console.ReadLine();
            Console.Write("Please enter the last name of the person you want to add: ");
            string lastName = Console.ReadLine();
            Console.Write("Please enter the ID of the added persons mother: ");
            int mother = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter the ID of the added persons father: ");
            int father = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter the birth year of the added person: ");
            int birthYear = Convert.ToInt32(Console.ReadLine());
            AddData(firstName, lastName, mother, father, birthYear);
            Console.Write("Mother is: ");
            GetPersonInfo(mother);



        }


        private void GetPersonInfo(int mother)
        {

            var list = GetDataTable($"SELECT * FROM {databaseName}.[dbo].[{tableName}] WHERE ID = '{mother}'");
            foreach (item in list)
            {
                Console.WriteLine($"{item}");
            }


        }

        private void ShowMenu()
        {
            Menu.ProgramMenu();
        }


        private void ClearScreen()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();

        }



        public void ExitProgram()
        {
            Console.WriteLine("Thanks for playing. See ya!");
            keepPlaying = false;

        }





    }
}
