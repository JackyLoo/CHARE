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
using CHARE_System.Class;

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
            SetContentView(Resource.Layout.SearchDriverListView);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
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
            listView.Adapter = new DriverListViewAdapter(this, listTrips, passTrip);
        }
       
        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}