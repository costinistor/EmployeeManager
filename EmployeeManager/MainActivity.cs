using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using EmployeeManager.Model;
using System.Linq;

namespace EmployeeManager
{
    [Activity(Label = "EmployeeManager", MainLauncher = true)]
    public class MainActivity : Activity
    {
        TextView textEmployees;
        SearchView searchEmployee;
        ImageButton btnAddEmployee;
        TextView noEmployeesInList;

        List<EmployeeList> mEmployees = new List<EmployeeList>();
        ListView mEmployeesList;

        public static int idEmployeeSelected;
        string searchForEmployees = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Views();

            //Buttons events for search control
            searchEmployee.SearchClick += SearchEmployee_SearchClick;
            searchEmployee.Close += SearchEmployee_Close;
            searchEmployee.QueryTextChange += SearchEmployee_QueryTextChange;
            //end search

            btnAddEmployee.Click += BtnAddEmployee_Click;

            WriteTheListWithEmployees(); // List with employees from database

        }

        // Search control for employees
        private void SearchEmployee_SearchClick(object sender, System.EventArgs e)
        {
            textEmployees.Visibility = ViewStates.Gone;
        }

        private void SearchEmployee_Close(object sender, SearchView.CloseEventArgs e)
        {
            e.Handled = false;
            textEmployees.Visibility = ViewStates.Visible;
        }

        private void SearchEmployee_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            searchForEmployees = e.NewText;
            WriteTheListWithEmployees();
        }
        // end search control

        // Button to add employee form
        private void BtnAddEmployee_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddEmployeeForm));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_up, Resource.Animation.slide_out_up);
        }

        // Display the list with all employees from database
        void WriteTheListWithEmployees()
        {
            mEmployees.Clear();
            var employees = GetRealm.realm().All<Employee>().OrderBy(n => n.Name);
            foreach (var employee in employees)
            {
                if(employee.Name.ToLower().Contains(searchForEmployees) || employee.Occupation.ToLower().Contains(searchForEmployees))
                {
                    mEmployees.Add(new EmployeeList(employee.Id, employee.Name.Substring(0,1), employee.Name, employee.Occupation, typeof(EmployeeInfo), employee.Color));
                }
            }

            AdapterListEmployees adapter = new AdapterListEmployees(this, mEmployees);
            mEmployeesList.Adapter = adapter;

            // If there are data in database alert will be hidden
            if (mEmployees.Count > 0)
                noEmployeesInList.Visibility = ViewStates.Gone;

            mEmployeesList.ItemClick += EmployeesList_ItemClick;
        }
        // Employee clicked from the list to open the employee information
        private void EmployeesList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            idEmployeeSelected = mEmployees[e.Position].Id;
            Intent intent = new Intent(this, mEmployees[e.Position].EmployeeInfoView);
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_left, Resource.Animation.slide_out_left);
        }

        // 
        void Views()
        {
            textEmployees = FindViewById<TextView>(Resource.Id.textEmployees);
            searchEmployee = FindViewById<SearchView>(Resource.Id.searchEmployee);
            btnAddEmployee = FindViewById<ImageButton>(Resource.Id.btnAddEmployee);
            noEmployeesInList = FindViewById<TextView>(Resource.Id.noEmployeeInList);
            mEmployeesList = FindViewById<ListView>(Resource.Id.listEmployees);
        }
    }
}

