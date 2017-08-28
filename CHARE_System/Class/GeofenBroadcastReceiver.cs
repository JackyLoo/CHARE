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
using Android.Net;

namespace CHARE_System.Class
{
    class GeofenBroadcastReceiver : BroadcastReceiver
    {
        private string transitionState;        
        private GeofenceListener listener;
        private string geofenceIds;

        public interface GeofenceListener
        {
            void OnTransitionStateChange();
        }

        public void SetListener(GeofenceListener listener)
        {
            this.listener = listener;
        }

        public GeofenBroadcastReceiver()
        {            
        }

        public override void OnReceive(Context context, Intent intent)
        {            
            transitionState = intent.GetStringExtra("Transition");
            geofenceIds = intent.GetStringExtra("GeofenceId");
            Console.WriteLine("===== Receiver receive message " + transitionState);
                                    
            if (listener != null)
            {                                
                listener.OnTransitionStateChange();
            }            
        }

        public string GeofenceIDs()
        {
            return geofenceIds;
        }

        public string TransitionState()
        {
            return transitionState;
        }
    }
}