using Android.App;
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
    class TripDriverListViewAdapter : BaseAdapter<TripDetails>
    {
        private static Context context;
        private List<TripDetails> trips;        
        private HttpClient client;
        private ProgressDialog progress;

        public TripDriverListViewAdapter(Context c, List<TripDetails> trips)
        {
            this.trips = trips;
            context = c;
            client = RESTClient.GetClient();
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
                var seat = view.FindViewById<TextView>(Resource.Id.trip_driver_row_seat);                

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
                    Seat = seat,
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

            int totalPassenger = 0;
            int totalSeat = trips[position].availableSeat;
            if (trips[position].PassengerIDs != null)
            {
                string[] passengers = trips[position].PassengerIDs.Split(',');
                totalPassenger = passengers.Count();                
            }
            holder.Seat.Text = totalPassenger + "/" + totalSeat;

            if (trips[position].Requests.Count <= 0)
                holder.mButton.Text = "No Request";
            else
            {
                bool hasPending = false;
                foreach (Request r in trips[position].Requests)
                {
                    if (r.status.Equals("Pending"))
                    {         
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
                    ((Activity)context).StartActivityForResult(intent, 0);
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
                    // Send intent extra to notify the trip is editable
                    // Condition = no passenger in the trip yet AND no passenger send request for this trip
                    if (totalPassenger == 0 && holder.mButton.Text.Equals("No Request"))
                        intent.PutExtra("Status", "No Request");
                    else if (totalPassenger != 0 && holder.mButton.Text.Equals("No Request"))
                        intent.PutExtra("Status", "Has Passenger");

                    intent.PutExtra("Trip", JsonConvert.SerializeObject(trips[position]));
                    ((Activity)context).StartActivityForResult(intent, 0);
                }                
            };

            view.LongClick += (sender, e) =>
            {
                Dialog dialog = new Dialog(context);
                dialog.SetContentView(Resource.Layout.Custom_Dialog_Action);
                TextView disjoinPassenger = (TextView)dialog.FindViewById(Resource.Id.quit);
                TextView deleteTrip = (TextView)dialog.FindViewById(Resource.Id.delete);
                disjoinPassenger.Text = "Disjoin Passengers";

                if (string.IsNullOrEmpty(trips[position].PassengerIDs))
                    disjoinPassenger.Visibility = ViewStates.Gone;                
                
                disjoinPassenger.Click += async (sender2, e2) =>
                {
                    progress.SetMessage("Disjoining passengers.....");              
                    progress.Show();                    
                    TripDriver td = new TripDriver(trips[position]);                    
                    await RESTClient.DisjoinAllPassengerAsync(context, td);
                    Intent intent = new Intent(context, typeof(TripDriverListViewActivity));
                    ((Activity)context).Finish();
                    ((Activity)context).StartActivity(intent);
                    progress.Dismiss();
                    dialog.Dismiss();                    
                };

                deleteTrip.Click += async (sender2, e2) =>
                {
                    progress.SetMessage("Deleting trip.....");
                    progress.Show();
                    await RESTClient.DeleteTripDriverAsync(context, (int)trips[position].TripDriverID);
                    Intent intent = new Intent(context, typeof(TripDriverListViewActivity));
                    ((Activity)context).Finish();
                    ((Activity)context).StartActivity(intent);
                    progress.Dismiss();
                    dialog.Dismiss();
                };
                dialog.Show();
            };            
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