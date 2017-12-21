using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Realms;

namespace EmployeeManager.Model
{
    class Employee : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string HireDate { get; set; }
        public string Occupation { get; set; }
        public string Salary { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Color { get; set; }

        public Employee() { }

        public Employee(int id, string name, string birthday, string hireDate, string occupation, string salary, string phone, string email, string color)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
            HireDate = hireDate;
            Occupation = occupation;
            Salary = salary;
            Phone = phone;
            Email = email;
            Color = color;
        }
    }
}