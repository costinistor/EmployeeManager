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
using Android.Graphics.Drawables;

namespace EmployeeManager
{
    class AdapterListEmployees : BaseAdapter<EmployeeList>
    {
        List<EmployeeList> mEmployee;
        Context mContext;

        public AdapterListEmployees(Context context, List<EmployeeList> employee)
        {
            mEmployee = employee;
            mContext = context;
        }

        public override int Count
        {
            get { return mEmployee.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override EmployeeList this[int position]
        {
            get { return mEmployee[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.rowEmployee, null, false);
            }

            TextView logoName = row.FindViewById<TextView>(Resource.Id.logoName);
            logoName.Text = mEmployee[position].LogoName;
            string bgColor = (!String.IsNullOrWhiteSpace(mEmployee[position].Color)) ? mEmployee[position].Color : "#C4EBE5";

            try
            {
                GradientDrawable bg = new GradientDrawable();
                bg.SetColor(Android.Graphics.Color.ParseColor(bgColor));
                bg.SetShape(ShapeType.Oval);
                logoName.Background = bg;
            }
            catch (Exception)
            {

                logoName.SetBackgroundColor(Android.Graphics.Color.ParseColor(bgColor));
            }


            TextView employeeName = row.FindViewById<TextView>(Resource.Id.employeeName);
            employeeName.Text = mEmployee[position].Name;

            TextView employeeOccupation = row.FindViewById<TextView>(Resource.Id.employeeOccupation);
            employeeOccupation.Text = mEmployee[position].Occupation;

            return row;
        }
    }

    class EmployeeList
    {
        public int Id { get; set; }
        public string LogoName { get; set; }
        public string Name { get; set; }
        public string Occupation { get; set; }
        public Type EmployeeInfoView { get; set; }
        public string Color { get; set; }

        public EmployeeList(int id, string logoName, string name, string occupation, Type employeeInfo, string color)
        {
            Id = id;
            LogoName = logoName;
            Name = name;
            Occupation = occupation;
            EmployeeInfoView = employeeInfo;
            Color = color;
        }
    }
}