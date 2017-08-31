using Android.Content;
using Android.Net;
using System;

namespace CHARE_System
{
    public class NetworkBroadcastReceiver : BroadcastReceiver
    {        
        private bool isConnected;
        private NetworkListener listener;

        public interface NetworkListener
        {
            void OnNetworkChange();
        }

        public void SetListener(NetworkListener listener)
        {
            this.listener = listener;
        }

        public NetworkBroadcastReceiver(Context context)
        {            
            isConnected = true;
            ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
            {                
                isConnected = false;                                
            }
            if (listener != null)
            {
                listener.OnNetworkChange();
            }
        }
        
        public override void OnReceive(Context context, Intent intent)
        {            
            isConnected = true;
            ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
            {                
                isConnected = false;                
            }
            if (listener != null)
            {
                listener.OnNetworkChange();
            }            
        }
        public bool GetStatus()
        {
            return isConnected;
        }
    }
}