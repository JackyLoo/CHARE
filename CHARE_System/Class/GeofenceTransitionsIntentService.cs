using System;
using Android.App;
using Android.Gms.Location;
using Android.Util;
using Android.Content;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Support.V4.Content;

namespace CHARE_System.Class
{
    [Service]
    public class GeofenceTransitionsIntentService : IntentService
    {        
        private string transitionState;
        
        protected const string TAG = "geofence-transitions-service";
        
        public GeofenceTransitionsIntentService() : base(TAG)
        {
        }        

        protected override void OnHandleIntent(Intent intent)
        {
            var geofencingEvent = GeofencingEvent.FromIntent(intent);
            if (geofencingEvent.HasError)
            {
                var errorMessage = GeofenceErrorMessages.GetErrorString(this, geofencingEvent.ErrorCode);
                Log.Error(TAG, errorMessage);
                return;
            }

            int geofenceTransition = geofencingEvent.GeofenceTransition;
            
            if (geofenceTransition == Geofence.GeofenceTransitionEnter ||
                geofenceTransition == Geofence.GeofenceTransitionExit)
            {
                transitionState = GetTransitionString(geofenceTransition);                

                //StartActivity(new Intent(this,typeof(CommentDialogActivity)).SetFlags(ActivityFlags.NewTask));

                
                IList<IGeofence> triggeringGeofences = geofencingEvent.TriggeringGeofences;

                string geofenceTransitionDetails = GetGeofenceTransitionDetails(this, geofenceTransition, triggeringGeofences);                

                SendNotification(geofenceTransitionDetails);                

                Log.Info(TAG, geofenceTransitionDetails);
                transitionState = GetTransitionString(geofenceTransition);
                Intent i = new Intent("transition_change");
                i.PutExtra("Transition", transitionState);
                i.PutExtra("GeofenceId", GetGeofenceString(triggeringGeofences));
                SendBroadcast(i);
            }
            else
            {
                // Log the error.
                Log.Error(TAG, GetString(Resource.String.geofence_transition_invalid_type, new[] { new Java.Lang.Integer(geofenceTransition) }));
            }
        }

        string GetGeofenceTransitionDetails(Context context, int geofenceTransition, IList<IGeofence> triggeringGeofences)
        {
            string geofenceTransitionString = GetTransitionString(geofenceTransition);

            var triggeringGeofencesIdsList = new List<string>();
            foreach (IGeofence geofence in triggeringGeofences)
            {
                triggeringGeofencesIdsList.Add(geofence.RequestId);
            }
            var triggeringGeofencesIdsString = string.Join(", ", triggeringGeofencesIdsList);

            return geofenceTransitionString + ": " + triggeringGeofencesIdsString;
        }

        string GetGeofenceString(IList<IGeofence> triggeringGeofences)
        {            
            var triggeringGeofencesIdsList = new List<string>();
            foreach (IGeofence geofence in triggeringGeofences)
            {
                triggeringGeofencesIdsList.Add(geofence.RequestId);
            }
            var triggeringGeofencesIdsString = string.Join(", ", triggeringGeofencesIdsList);

            return triggeringGeofencesIdsString;
        }

        void SendNotification(string notificationDetails)
        {
            var notificationIntent = new Intent(ApplicationContext, typeof(MainActivity));

            var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(notificationIntent);

            var notificationPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(this);
            builder.SetSmallIcon(Resource.Drawable.Icon)
                .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.Icon))
                .SetColor(Color.Red)
                .SetContentTitle(notificationDetails)
                .SetContentText(GetString(Resource.String.geofence_transition_notification_text))
                .SetContentIntent(notificationPendingIntent);

            builder.SetAutoCancel(true);

            var mNotificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            mNotificationManager.Notify(0, builder.Build());
        }

        public string GetState()
        {
            return transitionState;
        }

        string GetTransitionString(int transitionType)
        {
            switch (transitionType)
            {
                case Geofence.GeofenceTransitionEnter:
                    return GetString(Resource.String.geofence_transition_entered);
                case Geofence.GeofenceTransitionExit:
                    return GetString(Resource.String.geofence_transition_exited);
                default:
                    return GetString(Resource.String.unknown_geofence_transition);
            }
        }
    }
}

