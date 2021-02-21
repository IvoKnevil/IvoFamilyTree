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
                 
                case 1: //Show all names starting with certain letter.
                    Console.Write("Please enter the first letter in first name: ");
                    string letter = Console.ReadLine();
                    letter = letter + "%";
                    ShowAllNames(letter);
                    ClearScreen();
                    break;

                
                case 2: //the user choses to add a person.
                    AddPerson();
                    ClearScreen();
                    break;

                
                case 3: //the user choses to delete a person
                    ListAllPeople();
                    Console.Write("Please enter the Id of the person you want to remove: ");
                    int personId = Convert.ToInt32(Console.ReadLine());
                    RemovePerson(personId);
                    ClearScreen();
                    break;

                
                case 4:

                    ChangePerson(); //the user choses to change a person
                    ClearScreen();
                    break;

                case 5:
                    ShowGrandParents();  //Show grandparents
                    break;

                case 6:
                    keepPlaying = false;  //End program
                    break;

                case 7:
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
            Console.WriteLine($"\nYou just added {firstName} {lastName}\n");

            Console.Write("Mother is: \n");
            GetPersonInfo(mother);
            Console.Write("\nFather is: \n");
            GetPersonInfo(father);

        }

        private void ListAllPeople()
        {

            var sql = $"select id, firstName, lastName, birthYear from {databaseName}.[dbo].[{tableName}];";
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

            foreach (DataRow dataRow in dt.Rows)
            {

                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine($"{item}");

                }
                Console.WriteLine("\n");
            }

        }

        private void ChangePerson()
        {
            ListAllPeople();
            Console.Write("Please enter the Id of the person you want to change: ");
            int personId = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("\nWhat would you like to change:\n");
            Console.WriteLine("1. First name");
            Console.WriteLine("2. Last name");
            Console.WriteLine("3. Abort change");
            Console.Write("\nPlease enter the menu number: ");
            int changeId = Convert.ToInt32(Console.ReadLine());


            Console.Clear();
            if (changeId == 1)
            {
                Console.Write("Please enter new first name: ");
                string newName = Console.ReadLine();
                Console.Clear();
                ChangeFirstName(personId, newName);

            }
            else if (changeId == 2)
            {
                Console.Write("Please enter new last name: ");
                string newName = Console.ReadLine();
                Console.Clear();
                ChangeLastName(personId, newName);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Change aborted");
            }
        }



        private void ChangeFirstName(int personId, string newName)
        {

            var sql = @$"UPDATE {databaseName}.[dbo].[{tableName}] 
                          set FirstName = @newName
                          WHERE id =@personId;";


            var dt = new DataTable();
            var connString = string.Format(connectionString, databaseName);
            Console.Clear();
            Console.WriteLine("You are about to change first name of the person below\n");
            GetPersonInfo(personId);
            Console.Write("\nDo you want to proceed? (Y/N)");
            string answer = Console.ReadLine();

            if (answer == "Y" || answer == "y")
            {

                using (var conn = new SqlConnection(connString))

                {
                    conn.Open();
                    using (var command = new SqlCommand(sql, conn))
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            command.Parameters.AddWithValue("@personId", personId);
                            command.Parameters.AddWithValue("@newName", newName);
                            adapter.Fill(dt);
                        }
                    }
                    conn.Close();
                }
                Console.Clear();
                Console.WriteLine("Name changed");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Name change aborted");

            }

        }

        private void ChangeLastName(int personId, string newName)
        {

            var sql = @$"UPDATE {databaseName}.[dbo].[{tableName}] 
                          set Lastname = @newName
                          WHERE id =@personId;";


            var dt = new DataTable();
            var connString = string.Format(connectionString, databaseName);
            Console.Clear();
            Console.WriteLine("You are about to change last name of the person below\n");
            GetPersonInfo(personId);
            Console.Write("\nDo you want to proceed? (Y/N)");
            string answer = Console.ReadLine();

            if (answer == "Y" || answer == "y")
            {

                using (var conn = new SqlConnection(connString))

                {
                    conn.Open();
                    using (var command = new SqlCommand(sql, conn))
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            command.Parameters.AddWithValue("@personId", personId);
                            command.Parameters.AddWithValue("@newName", newName);
                            adapter.Fill(dt);
                        }
                    }
                    conn.Close();
                }
                Console.Clear();
                Console.WriteLine("Name changed");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Name change aborted");

            }

        }


        private void RemovePerson(int personId)
        {

            var sql = $"DELETE FROM {databaseName}.[dbo].[{tableName}] WHERE id = @personId;";
            var dt = new DataTable();
            var connString = string.Format(connectionString, databaseName);
            Console.Clear();
            Console.WriteLine("The person below will be removed from the table: \n");
            GetPersonInfo(personId);
            Console.Write("\nDo you want to proceed? (Y/N)");
            string answer = Console.ReadLine();

            if (answer == "Y" || answer == "y")
            {

                using (var conn = new SqlConnection(connString))

                {
                    conn.Open();
                    using (var command = new SqlCommand(sql, conn))
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            command.Parameters.AddWithValue("@personId", personId);
                            adapter.Fill(dt);
                        }
                    }
                    conn.Close();
                }
                Console.Clear();
                Console.WriteLine("Person removed");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Removal aborted");

            }

        }

        private void GetPersonInfo(int personId)
        {

            var sql = $"select firstName, lastName, birthYear from {databaseName}.[dbo].[{tableName}] where ID = @personId;";

            var dt = new DataTable();
            var connString = string.Format(connectionString, databaseName);

            using (var conn = new SqlConnection(connString))

            {
                conn.Open();
                using (var command = new SqlCommand(sql, conn))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        command.Parameters.AddWithValue("@personId", personId);
                        adapter.Fill(dt);
                    }
                }
                conn.Close();
            }


            foreach (DataRow dataRow in dt.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

        }


        private void ShowAllNames(string letter)
        {

            var sql = $"select firstName, lastName, birthYear from {databaseName}.[dbo].[{tableName}] where firstName like @letter;";
            var dt = new DataTable();
            var connString = string.Format(connectionString, databaseName);

            using (var conn = new SqlConnection(connString))

            {
                conn.Open();
                using (var command = new SqlCommand(sql, conn))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        command.Parameters.AddWithValue("@letter", letter);
                        adapter.Fill(dt);
                    }
                }
                conn.Close();
            }

            foreach (DataRow dataRow in dt.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

        }

        private void ShowGrandParents()
        {
            var sql = $"select Id, firstName, lastName, birthYear from {databaseName}.[dbo].[{tableName}] where mother = 0;";
            var list = GetDataTable(sql);

            foreach (DataRow row in list.Rows)
            {
                Console.WriteLine($"{row["Id"]}. {row["firstName"].ToString().Trim()}, {row["lastName"].ToString().Trim()}, birth year: {row["birthYear"]}");
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
