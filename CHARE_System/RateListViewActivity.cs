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
using System.Net.Http.Headers;
using CHARE_System.Class;
using Newtonsoft.Json;
using CHARE_System.JSON_Object;

namespace CHARE_System
{
    [Activity(Label = "RateListViewActivity")]
    public class RateListViewActivity : Activity
    {
        private ProgressDialog progress;
        private HttpClient client;
        private ListView listView;
        private List<RatingDetails> listRatings;
        private string memberID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RateListView);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
            else
            {
                memberID = Intent.GetStringExtra("MemberID");
                Console.WriteLine("=== Check id again " + memberID);

                listView = FindViewById<ListView>(Resource.Id.listview);

                client = new HttpClient();
                client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("Loading request...");
                progress.SetCancelable(false);

                RunOnUiThread(() =>
                {
                    progress.Show();
                });

                LoadRateDetails(memberID);
            }
        }

        async void LoadRateDetails(string id)
        {
            var models = await RESTClient.GetRateListAsync(this, id);
            Console.WriteLine("===== Models");
            Console.WriteLine(": " + models);
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
        
                listRatings = JsonConvert.DeserializeObject<List<RatingDetails>>(models);
                listView.Adapter = new RateListViewAdapter(this, listRatings);        
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}