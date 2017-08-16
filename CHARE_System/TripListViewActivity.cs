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
using CHARE_REST_API.Models;

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
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TripListView);

            listView = FindViewById<ListView>(Resource.Id.trip_listview);

            ISharedPreferences pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private);
            var member = pref.GetString(GetString(Resource.String.PreferenceSavedMember), "");
            user = JsonConvert.DeserializeObject<Member>(member);

            client = new HttpClient();
            client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading Trips.....");
            progress.SetCancelable(false);

            RunOnUiThread(() =>
            {
                progress.Show();
            });

            LoadTripDetails();                                                
        }
        async void LoadTripDetails()
        {
            var models = await GetTripsAsync("api/Trips?id="+user.MemberID);
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });            
            listTrips = JsonConvert.DeserializeObject<List<TripDetails>>(models);                                               
            listView.Adapter = new TripListViewAdapter(listTrips);                        
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
    }
}