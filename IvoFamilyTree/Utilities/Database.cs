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
                ExitProgram();
                //ShowGameMenu(player); //Calls method to show the main menu
                //GameActions(Menu.PlayerMenuChoice(player), player); //calls method that takes user menu choice. Sends the info to method PlayerMenuChoice in the class Menu.
            }

        }

        private void AddMockData()
        {
            if (!DoesDbExist())

            {
                

                CreateDb();
                CreateTable();
                //AddTableData();
            }

            else
            {
                AddTableData();
                //Console.WriteLine("Data allready exists");
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


            CreateData("Clint", "Eastwood", 0, 0, 1925);
            CreateData("Anna", "Eastwood", 0, 0, 1935);
            CreateData("Arnold", "Schwarzenegger", 0, 0, 1945);
            CreateData("Gloria", "Schwarzenegger", 0, 0, 1952);
            CreateData("Richard", "Spelling", 0, 0, 1940);
            CreateData("Maria", "Spelling", 0, 0, 1945);
            CreateData("Bob", "Ferguson", 0, 0, 1943);
            CreateData("Ornela", "Ferguson", 0, 0, 1944);
            CreateData("Adolfo", "Dos Santos", 0, 0, 1940);
            CreateData("Maria", "Dos Santos", 0, 0, 1940);
            CreateData("John", "Eastwood", 2, 1, 1962);
            CreateData("Laura", "Eastwood", 2, 1, 1962);
            CreateData("Anna", "Schwarzenegger", 4, 3, 1972);
            CreateData("George", "Spelling", 6, 5, 1973);
            CreateData("Lina", "Ferguson", 8, 7, 1974);
            CreateData("Patrick", "Dos Santos", 10, 9, 1976);
            CreateData("Tim", "Eastwood", 13, 11, 1980);
            CreateData("Lucia", "Dos Santos", 12, 16, 1982);
            CreateData("Ben", "Spelling", 15, 1, 1982);




            /*
                Console.WriteLine($"Table {tableName} created");
            */
        }


        private void CreateData(string firstName, string lastName, int mother, int father, int birthYear)
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



        public void ExitProgram()
        {
            Console.WriteLine("Thanks for playing. See ya!");
            keepPlaying = false;

        }





    }
}
