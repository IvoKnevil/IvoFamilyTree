using System;
using System.Collections.Generic;
using System.Text;

namespace IvoFamilyTree
{
    class Person
    {

        private string firstName;
        private string lastName;
        private int mother;
        private int father;
        private int birthYear;

        public string FirstName { get => firstName; set => firstName = value; }

        public string LastName { get => lastName; set => lastName = value; }

        public int Mother { get => mother; set => mother = value; }

        public int Father { get => father; set => father = value; }

        public int BirthYear { get => birthYear; set => birthYear = value; }

        /*
        public Person(string firstName, string lastName, int birthYear)
        {

            this.firstName = firstName;
            this.lastName = lastName;
            this.birthYear = birthYear;
        }
        */

        public Person(string firstName, string lastName, int mother, int father, int birthYear)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.mother = mother;
            this.father = father;
            this.birthYear = birthYear;

        }


    }
}
