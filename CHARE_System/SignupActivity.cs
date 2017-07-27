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

namespace CHARE_System
{
    [Activity(Label = "SignupActivity")]
    public class SignupActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Signup);

            TextView signBtn = FindViewById<TextView>(Resource.Id.signupbtn);
            signBtn.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

            TextView signupET = FindViewById<TextView>(Resource.Id.loginlink);
            signupET.Click += delegate
            {
                var intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            };
        }
    }
}