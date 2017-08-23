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
using Android.Locations;
using Android.Net;
using static Android.Animation.Animator;
using Android.Animation;

namespace CHARE_System
{
    [Activity(Label = "Test", MainLauncher = true, Icon = "@drawable/icon")]
    //[Activity(Label = "Test")]
    public class Test : Activity
    {
        private const long MIN_TIME = 400;
        private const float MIN_DISTANCE = 1000;
        TextView t;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.test);

            Dialog dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.Custom_Dialog_Rating);
            dialog.SetTitle("Rating");
            dialog.Show();
        }
    }
}