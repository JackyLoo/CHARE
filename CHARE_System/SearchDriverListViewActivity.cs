using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CHARE_System
{
    [Activity(Label = "Found Drivers")]
    public class SearchDriverListViewActivity : Activity
    {        
        private ProgressDialog progress;        
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

                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("Looking for drivers.....");
                progress.SetCancelable(false);
               
                LoadDriverDetails(passTrip.TripPassengerID);
            }
        }

        async void LoadDriverDetails(int id)
        {
            RunOnUiThread(() =>
            {
                progress.Show();
            });

            var models = await RESTClient.SearchTripDriversAsync(this, id);                       
            listTrips = JsonConvert.DeserializeObject<List<TripDetails>>(models);
            listView.Adapter = new SearchDriverListViewAdapter(this, listTrips, passTrip);

            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
        }
       
        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}