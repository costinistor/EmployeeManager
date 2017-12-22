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
using Android.Views.Animations;
using EmployeeManager.Model;

namespace EmployeeManager
{
    [Activity(Label = "EmployeeInfo")]
    public class EmployeeInfo : Activity
    {
        ImageButton btnDeleteEmployee, btnEditEmployee;
        TextView employeeInfoName;
        LinearLayout InfoEmployeeContainer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EmployeeInfo);
            Views();
            PrintEmployeeInfo();

            btnDeleteEmployee.Click += BtnDeleteEmployee_Click;
            btnEditEmployee.Click += delegate { SendEmployeeInfoToEdit(); };
        }

        private void BtnDeleteEmployee_Click(object sender, EventArgs e)
        {
            var employeeId = GetRealm.realm().All<Employee>().First(i => i.Id == MainActivity.idEmployeeSelected);
            GetRealm.realm().Write(() => { GetRealm.realm().Remove(employeeId); });
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
            Finish();
        }

        void SendEmployeeInfoToEdit()
        {
            Intent intent = new Intent(this, typeof(AddEmployeeForm));
            intent.PutExtra("convertToEdit", true);
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_up, Resource.Animation.slide_out_up);
        }

        void PrintEmployeeInfo()
        {
            var employeeInfo = GetRealm.realm().All<Employee>().Where(i => i.Id == MainActivity.idEmployeeSelected);
            foreach (var data in employeeInfo)
            {
                employeeInfoName.Text = data.Name;
                CreateLayoutRow("Birthday:", data.Birthday, 2, 0);
                CreateLayoutRow("Hire date:", data.HireDate, 3, 500);
                CreateLayoutRow("Occupation:", data.Occupation, 4, 1000);
                CreateLayoutRow("Salary:", data.Salary, 5, 1500);
                CreateLayoutRow("Phone:", data.Phone, 6, 2000);
                CreateLayoutRow("Email:", data.Email, 7, 2500);
            }
        }

        void CreateLayoutRow(string title, string data, int index, long timeToStart)
        {
            LinearLayout lvContainer = new LinearLayout(this);
            lvContainer.Orientation = Orientation.Vertical;
            lvContainer.SetPadding(0, 30, 0, 0);
            InfoEmployeeContainer.AddView(lvContainer, index);

            TextView titleInfoRow = new TextView(this);
            titleInfoRow.Text = title;
            titleInfoRow.SetTextColor(Android.Graphics.Color.ParseColor("#38538A"));
            titleInfoRow.SetTextSize(Android.Util.ComplexUnitType.Sp, 16);
            lvContainer.AddView(titleInfoRow);

            TextView getDataInfoRow = new TextView(this);
            getDataInfoRow.Text = data;
            getDataInfoRow.SetTextColor(Android.Graphics.Color.ParseColor("#FF8800"));
            getDataInfoRow.SetTextSize(Android.Util.ComplexUnitType.Sp, 18);
            getDataInfoRow.SetPadding(0, 0, 0, 20);
            lvContainer.AddView(getDataInfoRow);
            getDataInfoRow.StartAnimation(animation(timeToStart));

            TextView lineDivider = new TextView(this);
            lineDivider.SetHeight(2);
            lineDivider.SetBackgroundColor(Android.Graphics.Color.ParseColor("#B2CBE6"));
            lvContainer.AddView(lineDivider);
        }

        Animation animation(long timeToStart)
        {
            Animation anim = AnimationUtils.LoadAnimation(this, Resource.Animation.obj_slide_right);
            anim.StartOffset = timeToStart;
            return anim;
        }

        void Views()
        {
            btnDeleteEmployee = FindViewById<ImageButton>(Resource.Id.btnDeleteEmployee);
            btnEditEmployee = FindViewById<ImageButton>(Resource.Id.btnEditEmployee);
            employeeInfoName = FindViewById<TextView>(Resource.Id.employeeInfoName);
            InfoEmployeeContainer = FindViewById<LinearLayout>(Resource.Id.InfoEmployeeContainer);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StartActivity(new Intent(this, typeof(MainActivity)));
            OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
            Finish();
        }
    }
}