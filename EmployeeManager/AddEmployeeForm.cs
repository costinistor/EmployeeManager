﻿using System;
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
using EmployeeManager.Model;
using Android.Animation;
using Android.Views.Animations;
using System.Threading.Tasks;
using System.Threading;

namespace EmployeeManager
{
    [Activity(Label = "AddEmployeeForm")]
    public class AddEmployeeForm : Activity
    {
        TextView createOrEditEmployee;
        ImageButton btnSaveEmployee, btnSelectColor;
        EditText inputEmployeeName, inputEmployeeBirthday, inputEmployeeHireDate, inputEmployeeOccupation, inputEmployeeSalary,
                 inputEmployeePhone, inputEmployeeEmail;

        bool editEmployee;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddEmployeeForm);
            Views();
            editEmployee = Intent.GetBooleanExtra("convertToEdit", false); // return true if button edit was clicked
            GetEmployeeInfoToEdit();

            btnSaveEmployee.Click += delegate { AddOrEditEmployee(); };

            //Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
            //CreateAnim(anim1, 0, createOrEditEmployee);
            //CreateAnim(anim2, 2000, btnSaveEmployee);

            createOrEditEmployee.StartAnimation(CreateAnim(0));
            btnSaveEmployee.StartAnimation(CreateAnim(4000));
        }

        Animation CreateAnim(long time)
        {
            Animation anim = AnimationUtils.LoadAnimation(this, Resource.Animation.rotate_right);
            anim.StartOffset = time;
            return anim;
        }

        void GetEmployeeInfoToEdit()
        {
            if (editEmployee)
            {
                var employeeInfo = GetRealm.realm().All<Employee>().Where(i => i.Id == MainActivity.id);
                foreach (var data in employeeInfo)
                {
                    inputEmployeeName.Text = data.Name;
                    inputEmployeeBirthday.Text = data.Birthday;
                    inputEmployeeHireDate.Text = data.HireDate;
                    inputEmployeeOccupation.Text = data.Occupation;
                    inputEmployeeSalary.Text = data.Salary;
                    inputEmployeePhone.Text = data.Phone;
                    inputEmployeeEmail.Text = data.Email;
                }
                createOrEditEmployee.Text = GetString(Resource.String.edit_employee);
            }
            else
            {
                createOrEditEmployee.Text = GetString(Resource.String.create_employee);
            }
        }

        void AddOrEditEmployee()
        {
            string name = inputEmployeeName.Text;
            string birthday = inputEmployeeBirthday.Text;
            string hireDate = inputEmployeeHireDate.Text;
            string occupation = inputEmployeeOccupation.Text;
            string salary = inputEmployeeSalary.Text;
            string phone = inputEmployeePhone.Text;
            string email = inputEmployeeEmail.Text;

            if(!String.IsNullOrWhiteSpace(name) || !String.IsNullOrWhiteSpace(occupation))
            {
                if (!editEmployee)
                {
                    // Add new employee here
                    var count = GetRealm.realm().All<Employee>().Count();
                    int nextId = 0;
                    if (count > 0)
                    {
                        var lastId = GetRealm.realm().All<Employee>().Last();
                        nextId = lastId.Id + 1;
                    }

                    GetRealm.realm().Write(() =>
                    {
                        GetRealm.realm().Add(new Employee(nextId, name, birthday, hireDate, occupation, salary, phone, email, "#C4EBE5"));
                    });
                    StartActivity(new Intent(this, typeof(MainActivity)));
                    OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
                }
                else
                {
                    // Edit the employee here
                    GetRealm.realm().Write(() =>
                    {
                        GetRealm.realm().Add(new Employee(MainActivity.id, name, birthday, hireDate, occupation, salary, phone, email, "#C4EBE5"), update: true);      
                    });
                    StartActivity(new Intent(this, typeof(EmployeeInfo)));
                }
                Finish();
            }
            else
            {
                // error here
            }

        }

        void Views()
        {
            createOrEditEmployee = FindViewById<TextView>(Resource.Id.createOrEditEmployee);
            btnSaveEmployee = FindViewById<ImageButton>(Resource.Id.btnSaveEmployee);
            btnSelectColor = FindViewById<ImageButton>(Resource.Id.btnSelectColor);

            inputEmployeeName = FindViewById<EditText>(Resource.Id.inputEmployeeName);
            inputEmployeeBirthday = FindViewById<EditText>(Resource.Id.inputEmployeeBirthday);
            inputEmployeeHireDate = FindViewById<EditText>(Resource.Id.inputEmployeeHireDate);
            inputEmployeeOccupation = FindViewById<EditText>(Resource.Id.inputEmployeeOccupation);
            inputEmployeeSalary = FindViewById<EditText>(Resource.Id.inputEmployeeSalary);
            inputEmployeePhone = FindViewById<EditText>(Resource.Id.inputEmployeePhone);
            inputEmployeeEmail = FindViewById<EditText>(Resource.Id.inputEmployeeEmail);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.slide_in_down, Resource.Animation.slide_out_down);
            Finish();
        }
    }
}