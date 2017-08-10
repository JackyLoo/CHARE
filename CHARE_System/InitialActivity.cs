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
    [Activity(Label = "CHARE", MainLauncher = true, Icon = "@drawable/icon")]
    //[Activity(Label = "InitialActivity")]
    public class InitialActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ISharedPreferences pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private);
            var member = pref.GetString(GetString(Resource.String.PreferenceSavedMember), "");

            if (!member.Equals(""))
            {
                Intent intent = new Intent(this, typeof(MainActivity));                
                intent.PutExtra("Member", member);
                StartActivity(intent);
                Finish();
            }
            else
            {
                Intent intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            }
        }
    }
}