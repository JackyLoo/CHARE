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
using CHARE_System.Class;

namespace CHARE_System
{
    //[Activity(Label = "TripListViewActivity", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "TripPassListViewActivity")]
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
                Console.WriteLine("===== End");
            }
        }

        async void LoadTripDetails(int id)
        {
            Console.WriteLine("===== Loading");
            var models = await RESTClient.GetTripPassengerListAsync(this, id);
            Console.WriteLine("Models " +models);
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });            
            listTrips = JsonConvert.DeserializeObject<List<TripDetails>>(models);                                               
            listView.Adapter = new TripPassListViewAdapter(this, listTrips);
            Console.WriteLine("===== Finish Loading");
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Console.WriteLine("===== RequestCode");
            Console.WriteLine("Code " + requestCode);
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