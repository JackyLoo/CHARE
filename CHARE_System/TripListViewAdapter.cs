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
using CHARE_REST_API.Models;
using Android.Gms.Common.Images;
using CHARE_System.JSON_Object;
using System.Globalization;

namespace CHARE_System
{
    class TripListViewAdapter : BaseAdapter<TripDetails>
    {
        List<TripDetails> trips;

        public TripListViewAdapter(List<TripDetails> trips)
        {
            this.trips = trips;
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
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TripDetailsRow, parent, false);

                var femaleOnly = view.FindViewById<TextView>(Resource.Id.trip_row_female);
                var origin = view.FindViewById<TextView>(Resource.Id.trip_row_origin);
                var dest = view.FindViewById<TextView>(Resource.Id.trip_row_destination);

                var day = view.FindViewById<TextView>(Resource.Id.trip_row_day);
                var arriveTime = view.FindViewById<TextView>(Resource.Id.trip_row_time);
                var duration = view.FindViewById<TextView>(Resource.Id.trip_row_duration);
                var cost = view.FindViewById<TextView>(Resource.Id.trip_row_cost);
                var button = view.FindViewById<TextView>(Resource.Id.trip_row_button);
                
                //var vehicle = view.FindViewById<TextView>(Resource.Id.trip_row_vehicle);

                view.Tag = new TripViewHolder() { Female = femaleOnly, Origin = origin, Dest = dest, Day = day,
                    ArriveTime = arriveTime, Duration = duration, Cost = cost, mButton = button};
            }

            var userType = trips[position].TripPassengerID;
            // userType is Driver if equal to 0
            if (userType == 0)
            {
            }
            else
            {
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
                    holder.Female.Visibility = ViewStates.Invisible;
                
                holder.Origin.Text = trips[position].origin;
                holder.Dest.Text = trips[position].destination;
                holder.Day.Text = strDay;
                holder.ArriveTime.Text = strTime;
                holder.Duration.Text = " • Approx " + trips[position].durationStr;
                holder.Cost.Text = trips[position].costStr;

                if (trips[position].TripDriverID.Equals(null))
                    holder.mButton.Text = "Search Driver";
                else
                    holder.mButton.Text = "View Driver";

                holder.mButton.Click += (sender, e) =>
                {


                };
            }

            Console.WriteLine("===== GetView End");
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
        public TextView Duration { get; set; }
        public TextView Cost { get; set; }
        public TextView mButton { get; set; }
    }
}