﻿using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace CHARE_System
{
    class TripPassListViewAdapter : BaseAdapter<TripDetails>
    {
        private List<TripDetails> trips;
        private static Context context;        
        private ProgressDialog progress;

        public TripPassListViewAdapter(Context c, List<TripDetails> trips)
        {
            this.trips = trips;
            context = c;            
            progress = new Android.App.ProgressDialog(c);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
        }

        public override TripDetails this[int position]
        {
            get
            {
                return trips[position];
            }
        }

        public override int Count
        {
            get
            {
                return trips.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {            
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TripPassDetailsRow, parent, false);
                var femaleOnly = view.FindViewById<TextView>(Resource.Id.trip_row_female);
                var origin = view.FindViewById<TextView>(Resource.Id.trip_row_origin);
                var dest = view.FindViewById<TextView>(Resource.Id.trip_row_destination);
                var day = view.FindViewById<TextView>(Resource.Id.trip_row_day);
                var arriveTime = view.FindViewById<TextView>(Resource.Id.trip_row_time);
                var distance = view.FindViewById<TextView>(Resource.Id.trip_row_distance);
                var duration = view.FindViewById<TextView>(Resource.Id.trip_row_duration);
                var cost = view.FindViewById<TextView>(Resource.Id.trip_row_cost);
                var button = view.FindViewById<TextView>(Resource.Id.trip_row_button);                               

                view.Tag = new TripViewHolder() { Female = femaleOnly, Origin = origin, Dest = dest, Day = day,
                    ArriveTime = arriveTime, Distance = distance, Duration = duration, Cost = cost, mButton = button};
            }
                        
            var holder = (TripViewHolder)view.Tag;
            
            // Convert time to hh:mm tt format
            var time = trips[position].arriveTime.Split(':');
            int totalInSecond = (int.Parse(time[0]) * 3600) + (int.Parse(time[1]) * 60);
            TimeSpan onTimeSet = TimeSpan.FromSeconds(totalInSecond);
            string strTime = DateTime.ParseExact(onTimeSet.ToString(@"hh\:mm"), "HH:mm", null).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US"));            

            // Convert "Mon,Tue,Wed..." to "Mon • Tue • Wed..." format
            string strDay = trips[position].days.Replace(",", " • ");

            // Check if is female only
            if (trips[position].femaleOnly.Equals("No"))
            {
                holder.Female.SetHeight(0);
                holder.Female.Visibility = ViewStates.Invisible;
            }
                
            holder.Origin.Text = trips[position].origin;
            holder.Dest.Text = trips[position].destination;
            holder.Day.Text = strDay;
            holder.ArriveTime.Text = strTime;
            holder.Distance.Text = trips[position].distanceStr;
            holder.Duration.Text = " • Approx " + trips[position].durationStr;
            holder.Cost.Text = trips[position].costStr;

            if (trips[position].Requests.Count <= 0)
            {
                if (trips[position].TripDriverID.Equals(null))
                    holder.mButton.Text = "Search Driver";
            }                 
            else if (trips[position].Requests[0].status.Equals("Pending"))
                holder.mButton.Text = "Cancel Request";
            else if (trips[position].Requests[0].status.Equals("Accepted"))
                holder.mButton.Text = "View Driver";
            else if (trips[position].Requests[0].status.Equals("Rejected"))
                holder.mButton.Text = "Search Driver";
            
            holder.mButton.Click += async (sender, e) =>
            {
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(context, "Network error. Try again later.", ToastLength.Long).Show();
                if (holder.mButton.Text.Equals("Search Driver"))
                {
                    //Intent intent = new Intent(context, typeof(DriverListViewActivity)).SetFlags(ActivityFlags.NewTask);
                    Intent intent = new Intent(context, typeof(SearchDriverListViewActivity));
                    intent.AddFlags(ActivityFlags.ReorderToFront);
                    intent.PutExtra("Trip", JsonConvert.SerializeObject(trips[position]));
                    context.StartActivity(intent);
                }
                else if (holder.mButton.Text.Equals("Cancel Request"))
                {                        
                    trips[position].Requests[0].status = "Cancelled";
                    await RESTClient.DeleteRequestAsync(context,trips[position].Requests[0]);
                    holder.mButton.Text = "Search Driver";                        
                }
                else if (trips[position].Requests[0].status.Equals("Accepted"))
                    holder.mButton.Text = "View Driver";
                else if (trips[position].Requests[0].status.Equals("Rejected"))
                    holder.mButton.Text = "Search Driver";
            };

            view.Click += (sender, e) =>
            {
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(context, "Network error. Try again later.", ToastLength.Long).Show();
                else
                {
                    Intent intent = new Intent(context, typeof(TripPassDetailsRow_Edit));
                    // Send intent extra to notify the trip is editable
                    // Condition = no passenger in the trip yet AND no passenger send request for this trip
                    if (holder.mButton.Text.Equals("View Driver"))
                        intent.PutExtra("Status", "Has Driver");                    
                    intent.PutExtra("Trip", JsonConvert.SerializeObject(trips[position]));
                    ((Activity)context).StartActivityForResult(intent, 0);
                }                
            };

            view.LongClick += (sender, e) =>
            {                
                Dialog dialog = new Dialog(context);
                dialog.SetContentView(Resource.Layout.Custom_Dialog_Action);
                TextView quitCarpool = (TextView)dialog.FindViewById(Resource.Id.quit);
                TextView deleteTrip = (TextView)dialog.FindViewById(Resource.Id.delete);                
                if (trips[position].TripDriverID.Equals(null))                
                    quitCarpool.Visibility = ViewStates.Gone;                                    

                quitCarpool.Click += async (sender2, e2) =>
                {
                    progress.SetMessage("Disjoining.....");
                    progress.Show();
                    TripPassenger tp = new TripPassenger(trips[position]);                    
                    await RESTClient.QuitCarpoolPassengerAsync(context, tp);
                    Intent intent = new Intent(context, typeof(TripPassListViewActivity));
                    ((Activity)context).Finish();
                    ((Activity)context).StartActivity(intent); 
                    dialog.Dismiss();
                };

                deleteTrip.Click += async (sender2, e2) =>
                {
                    progress.SetMessage("Deleting.....");
                    progress.Show();
                    await RESTClient.DeleteTripPassengerAsync(context, trips[position].TripPassengerID);
                    Intent intent = new Intent(context, typeof(TripPassListViewActivity));
                    ((Activity)context).Finish();
                    ((Activity)context).StartActivity(intent);
                    dialog.Dismiss();
                };
                dialog.Show();
            };
            
            return view;
        }
    }

    public class TripViewHolder : Java.Lang.Object
    {
        public TextView Female { get; set; }
        public TextView Origin { get; set; }
        public TextView Dest { get; set; }
        public TextView Day { get; set; }
        public TextView ArriveTime { get; set; }
        public TextView Distance { get; set; }
        public TextView Duration { get; set; }
        public TextView Cost { get; set; }
        public TextView mButton { get; set; }
    }
}