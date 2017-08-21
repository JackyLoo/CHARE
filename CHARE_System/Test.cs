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
    //[Activity(Label = "Test", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "Test")]
    public class Test : Activity, ILocationListener, NetworkBroadcastReceiver.NetworkListener, IAnimatorListener
    {
        private const long MIN_TIME = 400;
        private const float MIN_DISTANCE = 1000;
        TextView t;
        
        NetworkBroadcastReceiver receiver;
        IntentFilter filter;        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.test);
            t = (TextView)FindViewById(Resource.Id.network_notification);
            
            receiver = new NetworkBroadcastReceiver(this);
            receiver.SetListener(this);
            filter = new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE");
            RegisterReceiver(receiver, filter);

            Button b = (Button)FindViewById(Resource.Id.testbtn);
            b.Click += (sender, e) =>
            {
                if(receiver.GetStatus())
                    Console.WriteLine("===== Status True");
                else
                    Console.WriteLine("===== Status False");                
            };            
            // Create your application here
        }
     
        public void OnLocationChanged(Location location)
        {
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        public void OnNetworkChange()
        {                       
            if (receiver.GetStatus())
            {                
                t.Animate().TranslationY(0).Alpha(0f).SetListener(this);                
            }
            else
            {
                t.Animate().TranslationY(0).Alpha(1.0f).SetListener(this);
            }
        }

        public void OnAnimationCancel(Animator animation)
        {            
        }

        public void OnAnimationEnd(Animator animation)
        {
            if(receiver.GetStatus())
                t.Visibility = ViewStates.Gone;
            else
                t.Visibility = ViewStates.Visible;
        }

        public void OnAnimationRepeat(Animator animation)
        {            
        }

        public void OnAnimationStart(Animator animation)
        {            
        }
    }
}