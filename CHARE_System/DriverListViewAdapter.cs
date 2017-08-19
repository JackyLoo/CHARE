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
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CHARE_System.Class;

namespace CHARE_System
{
    class DriverListViewAdapter : BaseAdapter<TripDetails>
    {
        private List<TripDetails> trips;
        private Context context;
        private HttpClient client;
        private TripDetails passTrip;        

        public DriverListViewAdapter(Context c, List<TripDetails> trips, TripDetails passTrip)
        {
            this.trips = trips;            
            this.passTrip = passTrip;
            context = c;
            client = new HttpClient();
            client.BaseAddress = new Uri(context.GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            Console.WriteLine("===== Driver GetView Start");
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SearchDriverDetailsRow, parent, false);

                var name = view.FindViewById<TextView>(Resource.Id.driver_row_name);
                var femaleOnly = view.FindViewById<TextView>(Resource.Id.driver_row_female);
                var origin = view.FindViewById<TextView>(Resource.Id.driver_row_origin);
                var dest = view.FindViewById<TextView>(Resource.Id.driver_row_destination);
                var day = view.FindViewById<TextView>(Resource.Id.driver_row_day);
                var arriveTime = view.FindViewById<TextView>(Resource.Id.driver_row_time);
                var duration = view.FindViewById<TextView>(Resource.Id.driver_row_duration);                
                var button = view.FindViewById<TextView>(Resource.Id.driver_row_button);                

                view.Tag = new DriverViewHolder()
                {
                    Name = name,
                    Female = femaleOnly,
                    Origin = origin,
                    Dest = dest,
                    Day = day,
                    ArriveTime = arriveTime,
                    Duration = duration,             
                    mButton = button
                };
            }
            
            var holder = (DriverViewHolder)view.Tag;
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

            holder.Name.Text = trips[position].Member.username;
            holder.Origin.Text = trips[position].origin;
            holder.Dest.Text = trips[position].destination;
            holder.Day.Text = strDay;
            holder.ArriveTime.Text = strTime;
            holder.Duration.Text = " • Approx " + trips[position].durationStr;            
            holder.mButton.Click += async (sender, e) =>
            {
                Request request = new Request();
                request.SenderID = passTrip.TripPassengerID;
                request.DriverID = (int)trips[position].TripDriverID;
                await RESTClient.CreateRequestAsync(context, request);
                Intent intent = new Intent(context, typeof(TripPassListViewActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                context.StartActivity(intent);                
            };                        
            return view;
        }        
    }

    public class DriverViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
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