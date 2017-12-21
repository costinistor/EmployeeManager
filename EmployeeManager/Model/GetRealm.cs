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
    class GetRealm
    {
        public static Realm realm()
        {
            var config = new RealmConfiguration() { SchemaVersion = 0 };
            return Realm.GetInstance(config);
        }
    }
}