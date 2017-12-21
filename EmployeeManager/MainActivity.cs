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
        ImageButton btnAddEmployee;
        TextView noEmployeesInList;

        List<EmployeeList> mEmployees = new List<EmployeeList>();
        ListView mEmployeesList;

        public static int id;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Views();

            btnAddEmployee.Click += BtnAddEmployee_Click;

            CreateListOfEmployees();

        }

        private void BtnAddEmployee_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddEmployeeForm));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_up, Resource.Animation.slide_out_up);
        }

        void CreateListOfEmployees()
        {
            var employees = GetRealm.realm().All<Employee>().OrderBy(n => n.Name);
            foreach (var employee in employees)
            {
                mEmployees.Add(new EmployeeList(employee.Id, employee.Name.Substring(0,1), employee.Name, employee.Occupation, typeof(EmployeeInfo), employee.Color));
            }

            AdapterListEmployees adapter = new AdapterListEmployees(this, mEmployees);
            mEmployeesList.Adapter = adapter;

            if (mEmployees.Count > 0)
                noEmployeesInList.Visibility = ViewStates.Gone;

            mEmployeesList.ItemClick += MEmployeesList_ItemClick;
        }

        private void MEmployeesList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            id = mEmployees[e.Position].Id;
            Intent intent = new Intent(this, mEmployees[e.Position].EmployeeInfoView);
            StartActivity(intent);
        }

        void Views()
        {
            btnAddEmployee = FindViewById<ImageButton>(Resource.Id.btnAddEmployee);
            noEmployeesInList = FindViewById<TextView>(Resource.Id.noEmployeeInList);
            mEmployeesList = FindViewById<ListView>(Resource.Id.listEmployees);
        }
    }
}

