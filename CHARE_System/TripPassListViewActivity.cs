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
    [Activity(Label = "Trips")]
    public class TripPassListViewActivity : Activity
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
            SetContentView(Resource.Layout.TripPassListView);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
            else
            {
                listView = FindViewById<ListView>(Resource.Id.trip_pass_listview);

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

                LoadTripDetails(user.MemberID);                
            }
        }

        async void LoadTripDetails(int id)
        {            
            var models = await RESTClient.GetTripPassengerListAsync(this, id);            
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });            
            listTrips = JsonConvert.DeserializeObject<List<TripDetails>>(models);                                               
            listView.Adapter = new TripPassListViewAdapter(this, listTrips);            
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);            
            if (resultCode == 0)
            {
                this.Recreate();                
            }
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}