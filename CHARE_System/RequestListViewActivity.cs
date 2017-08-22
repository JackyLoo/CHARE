using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CHARE_System
{
    [Activity(Label = "RequestListViewActivity")]
    public class RequestListViewActivity : Activity
    {
        private ProgressDialog progress;
        private HttpClient client;
        private ListView listView;
        private List<Request> listRequests;
        private TripDetails passTrip;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RequestListView);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
            {
                passTrip = JsonConvert.DeserializeObject<TripDetails>(Intent.GetStringExtra("Trip"));

                listView = FindViewById<ListView>(Resource.Id.listview);

                client = new HttpClient();
                client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("Looking for drivers.....");
                progress.SetCancelable(false);

                RunOnUiThread(() =>
                {
                    progress.Show();
                });

                LoadRequestDetails((int)passTrip.TripDriverID);
            }
        }

        async void LoadRequestDetails(int id)
        {
            var models = await RESTClient.GetTripRequestListAsync(this, id);
            Console.WriteLine("===== Models");
            Console.WriteLine(": "+models);
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
            listRequests = JsonConvert.DeserializeObject<List<Request>>(models);
            listView.Adapter = new RequestListViewAdapter(this, listRequests, passTrip);
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent = new Intent(this, typeof(TripDriverListViewActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            StartActivity(intent);            
            return true;
        }
    }
}