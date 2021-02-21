using System;
using System.Data.SqlClient;
using IvoFamilyTree.Utilities;


namespace IvoFamilyTree
{
    class Program
    {
        static void Main(string[] args)
        {

            Database database = new Database();
            database.StartProgram();
            database.ExitProgram();



        }
    }
}
