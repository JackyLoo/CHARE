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
    [Activity(Label = "Found Drivers")]
    public class SearchDriverListViewActivity : Activity
    {        
        private ProgressDialog progress;
        private HttpClient client;
        private ListView listView;
        private List<TripDetails> listTrips;
        private TripDetails passTrip;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SearchDriverListView);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
            else
            {
                passTrip = JsonConvert.DeserializeObject<TripDetails>(Intent.GetStringExtra("Trip"));
                listView = FindViewById<ListView>(Resource.Id.driver_listview);

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

                LoadTripDetails(passTrip.TripPassengerID);
            }
        }

        async void LoadTripDetails(int id)
        {            
            var models = await RESTClient.SearchTripDriversAsync(this, id);           
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
            listTrips = JsonConvert.DeserializeObject<List<TripDetails>>(models);
            listView.Adapter = new SearchDriverListViewAdapter(this, listTrips, passTrip);
        }
       
        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}