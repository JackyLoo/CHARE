using Android.Content;
using System;

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