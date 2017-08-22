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
using CHARE_REST_API.JSON_Object;
using System.Net.Http;
using CHARE_System.Class;
using System.Globalization;
using Newtonsoft.Json;

namespace CHARE_System
{
    class TripDriverListViewAdapter : BaseAdapter<TripDetails>
    {
        private static Context context;
        private List<TripDetails> trips;        
        private HttpClient client;

        public TripDriverListViewAdapter(Context c, List<TripDetails> trips)
        {
            this.trips = trips;
            context = c;
            client = RESTClient.GetClient();
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
            Console.WriteLine("===== GetView Start");
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TripDriverDetailsRow, parent, false);

                var femaleOnly = view.FindViewById<TextView>(Resource.Id.trip_driver_row_female);
                var origin = view.FindViewById<TextView>(Resource.Id.trip_driver_row_origin);
                var dest = view.FindViewById<TextView>(Resource.Id.trip_driver_row_destination);
                var day = view.FindViewById<TextView>(Resource.Id.trip_driver_row_day);
                var arriveTime = view.FindViewById<TextView>(Resource.Id.trip_driver_row_time);
                var distance = view.FindViewById<TextView>(Resource.Id.trip_driver_row_distance);
                var duration = view.FindViewById<TextView>(Resource.Id.trip_driver_row_duration);
                var cost = view.FindViewById<TextView>(Resource.Id.trip_driver_row_cost);
                var button = view.FindViewById<TextView>(Resource.Id.trip_driver_row_button);

                view.Tag = new TripDriverViewHolder()
                {
                    Female = femaleOnly,
                    Origin = origin,
                    Dest = dest,
                    Day = day,
                    ArriveTime = arriveTime,
                    Distance = distance,
                    Duration = duration,
                    Cost = cost,
                    mButton = button
                };
            }

            var holder = (TripDriverViewHolder)view.Tag;

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
                holder.mButton.Text = "No Request";
            else
            {
                bool hasPending = false;
                foreach (Request r in trips[position].Requests)
                {
                    if (r.status.Equals("Pending"))
                    {
                        Console.WriteLine("===== Has pending");
                        Console.WriteLine("Pending with "+r.RequestID);
                        hasPending = true;
                    }
                }
                if(hasPending)
                    holder.mButton.Text = "View Request";
                else
                    holder.mButton.Text = "No Request";
            }

            holder.mButton.Click += (sender, e) =>
            {
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(context, "Network error. Try again later.", ToastLength.Long).Show();
                else if (holder.mButton.Text.Equals("No Request"))
                {
                    Toast.MakeText(context, "There is no request yet. Try again later.", ToastLength.Long).Show();                    
                }
                else if (holder.mButton.Text.Equals("View Request"))
                {
                    Intent intent = new Intent(context, typeof(RequestListViewActivity));
                    intent.AddFlags(ActivityFlags.ReorderToFront);
                    intent.PutExtra("Trip", JsonConvert.SerializeObject(trips[position]));
                    context.StartActivity(intent);
                }                
            };

            view.Click += (sender, e) =>
            {
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(context, "Network error. Try again later.", ToastLength.Long).Show();
                else
                {
                    Intent intent = new Intent(context, typeof(TripDriverDetailsRow_Edit));
                    intent.PutExtra("Trip", JsonConvert.SerializeObject(trips[position]));
                    ((Activity)context).StartActivityForResult(intent, 0);
                }
            };

            Console.WriteLine("===== GetView End");
            return view;
        }
    }

    public class TripDriverViewHolder : Java.Lang.Object
    {
        public TextView Female { get; set; }
        public TextView Origin { get; set; }
        public TextView Dest { get; set; }
        public TextView Day { get; set; }
        public TextView ArriveTime { get; set; }
        public TextView Distance { get; set; }
        public TextView Duration { get; set; }
        public TextView Cost { get; set; }
        public TextView Seat { get; set; }
        public TextView mButton { get; set; }
    }
}