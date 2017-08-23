using Android.Content;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CHARE_System
{
    class RequestListViewAdapter : BaseAdapter<Request>
    {
        private List<Request> requests;
        private Context context;
        private HttpClient client;
        private TripDetails passTrip;

        public RequestListViewAdapter(Context c, List<Request> requests, TripDetails passTrip)
        {
            this.requests = requests;
            this.passTrip = passTrip;
            context = c;
            client = new HttpClient();
            client.BaseAddress = new Uri(context.GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override Request this[int position]
        {
            get
            {
                return requests[position];
            }
        }

        public override int Count
        {
            get
            {
                return requests.Count();
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
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RequestDetailsRow, parent, false);

                var name = view.FindViewById<TextView>(Resource.Id.name);
                var femaleOnly = view.FindViewById<TextView>(Resource.Id.female);
                var origin = view.FindViewById<TextView>(Resource.Id.origin);
                var dest = view.FindViewById<TextView>(Resource.Id.destination);
                var day = view.FindViewById<TextView>(Resource.Id.day);
                var arriveTime = view.FindViewById<TextView>(Resource.Id.time);
                var duration = view.FindViewById<TextView>(Resource.Id.duration);
                var buttonAccept = view.FindViewById<Button>(Resource.Id.button_accept);
                var buttonReject = view.FindViewById<Button>(Resource.Id.button_reject);

                view.Tag = new RequestViewHolder()
                {
                    Name = name,
                    Female = femaleOnly,
                    Origin = origin,
                    Dest = dest,
                    Day = day,
                    ArriveTime = arriveTime,
                    Duration = duration,
                    ButtonAccept = buttonAccept,
                    ButtonReject = buttonReject
                };
            }

            var holder = (RequestViewHolder)view.Tag;
            // Convert time to hh:mm tt format
            var time =  requests[position].TripPassenger.arriveTime.Split(':');
            int totalInSecond = (int.Parse(time[0]) * 3600) + (int.Parse(time[1]) * 60);
            TimeSpan onTimeSet = TimeSpan.FromSeconds(totalInSecond);
            string strTime = DateTime.ParseExact(onTimeSet.ToString(@"hh\:mm"), "HH:mm", null).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

            // Convert "Mon,Tue,Wed..." to "Mon • Tue • Wed..." format
            string strDay = requests[position].TripPassenger.days.Replace(",", " • ");

            // Check if is female only
            if (requests[position].TripPassenger.femaleOnly.Equals("No"))
                holder.Female.Visibility = ViewStates.Gone;

            holder.Name.Text = requests[position].TripPassenger.Member.username;
            holder.Origin.Text = requests[position].TripPassenger.origin;
            holder.Dest.Text = requests[position].TripPassenger.destination;
            holder.Day.Text = strDay;
            holder.ArriveTime.Text = strTime;
            holder.Duration.Text = " • Approx " + requests[position].TripPassenger.durationStr;
            holder.ButtonAccept.Click += async (sender, e) =>
            {
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(context, "Network error. Try again later.", ToastLength.Long).Show();
                else
                {
                    if (requests[position].TripDriver.PassengerIDs != null)
                    {
                        string[] noOfPassenger = requests[position].TripDriver.PassengerIDs.Split(',');
                        if (noOfPassenger.Count() >= requests[position].TripDriver.availableSeat)
                            Toast.MakeText(context, "The available seat is full. ", ToastLength.Long).Show();
                        else
                        {
                            requests[position].status = "Accepted";
                            await RESTClient.AcceptRequestAsync(context, requests[position]);
                            view.Visibility = ViewStates.Gone;
                        }
                    }
                    else
                    {
                        requests[position].status = "Accepted";
                        await RESTClient.AcceptRequestAsync(context, requests[position]);
                        view.Visibility = ViewStates.Gone;
                    }
                }
            };
            holder.ButtonReject.Click += async (sender, e) =>
            {
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(context, "Network error. Try again later.", ToastLength.Long).Show();
                else
                {
                    requests[position].status = "Rejected";
                    await RESTClient.RejectRequestAsync(context, requests[position]);
                    view.Visibility = ViewStates.Gone;
                }
            };
            return view;
        }
    }

    public class RequestViewHolder : Java.Lang.Object
    {        
        public TextView Name { get; set; }
        public TextView Female { get; set; }
        public TextView Origin { get; set; }
        public TextView Dest { get; set; }
        public TextView Day { get; set; }
        public TextView ArriveTime { get; set; }
        public TextView Duration { get; set; }
        public TextView Cost { get; set; }
        public Button ButtonAccept { get; set; }
        public Button ButtonReject { get; set; }
    }
}