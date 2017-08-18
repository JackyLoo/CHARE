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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CHARE_System.JSON_Object;
using CHARE_REST_API.JSON_Object;

namespace CHARE_System
{
    //[Activity(Label = "TripListViewActivity", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "TripListViewActivity")]
    public class TripListViewActivity : Activity
    {
        private Member user;
        private ProgressDialog progress;
        private HttpClient client;
        private ListView listView;
        private List<TripDetails> listTrips;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);                        
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TripListView);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            listView = FindViewById<ListView>(Resource.Id.trip_listview);

            client = new HttpClient();
            client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading Trips.....");
            progress.SetCancelable(false);
            
            ISharedPreferences pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private);
            var member = pref.GetString(GetString(Resource.String.PreferenceSavedMember), "");
            user = JsonConvert.DeserializeObject<Member>(member);

            RunOnUiThread(() =>
            {
                progress.Show();
            });
            Console.WriteLine("===== Start");            
            if (user.type.Equals("Driver"))
                LoadTripDetails("api/TripDrivers?id=");
            else
                LoadTripDetails("api/TripPassengers?id=");
            Console.WriteLine("===== End");
        }
        async void LoadTripDetails(string path)
        {
            Console.WriteLine("===== Loading");
            var models = await GetTripsAsync(path+user.MemberID);
            Console.WriteLine("Models " +models);
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });            
            listTrips = JsonConvert.DeserializeObject<List<TripDetails>>(models);                                               
            listView.Adapter = new TripListViewAdapter(this, listTrips);
            Console.WriteLine("===== Finish Loading");
        }

        async Task<string> GetTripsAsync(string path)
        {
            Console.WriteLine("===== GetTripsAsync Start");
            var make = "";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }
            Console.WriteLine("===== GetTripsAsync End");
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