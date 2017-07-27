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
    [Activity(Label = "LoginActivity")]

    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            TextView signBtn = FindViewById<TextView>(Resource.Id.signinbtn);
            signBtn.Click += delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

            TextView signupET = FindViewById<TextView>(Resource.Id.signuplink);
            signupET.Click += delegate
            {
               var intent = new Intent(this, typeof(SignupActivity));
               StartActivity(intent);
            };
        }
    }
}