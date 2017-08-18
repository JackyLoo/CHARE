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
using CHARE_System.JSON_Object;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CHARE_System
{
    [Activity(Label = "DriverListViewActivity")]
    public class DriverListViewActivity : Activity
    {
        private Member user;
        private ProgressDialog progress;
        private HttpClient client;
        private ListView listView;
        private List<TripDetails> listTrips;
        private TripDetails passTrip;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DriverListView);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

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
            
            LoadTripDetails("api/TripDrivers?id="+passTrip.TripPassengerID+"&passengerTrip=null");                        
        }

        async void LoadTripDetails(string path)
        {            
            var models = await GetTripsAsync(path);           
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
            listTrips = JsonConvert.DeserializeObject<List<TripDetails>>(models);
            listView.Adapter = new DriverListViewAdapter(this, listTrips, passTrip);
        }

        async Task<string> GetTripsAsync(string path)
        {            
            var make = "";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }            
            return make;
        }
        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}