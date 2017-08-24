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
using CHARE_REST_API.JSON_Object;
using Newtonsoft.Json;

namespace CHARE_System
{
    [Activity(Label = "CHARE", MainLauncher = true, Icon = "@drawable/icon")]
    //[Activity(Label = "InitialActivity")]

    public class InitialActivity : Activity
    {
        private HttpClient client;
        private ProgressDialog progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            /*
            client = new HttpClient();
            client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading...");
            progress.SetCancelable(false);

            TripPassenger tripDriver = new TripPassenger();
            tripDriver.PassengerID = 6;  
            tripDriver.originLatLng = "3.2717177,101.6489123";
            tripDriver.destinationLatLng = "3.068518,101.7704228";
            tripDriver.origin = "Taman Amansiara";
            tripDriver.destination = "Cheras 9 Miles";
            tripDriver.arriveTime = "11:13:00";
            tripDriver.femaleOnly = "No";
            tripDriver.cost = 33.24;
            tripDriver.distance = 33;
            tripDriver.duration = 2160;
            tripDriver.costStr = "RM33.24";
            tripDriver.distanceStr = "33.2 km";
            tripDriver.durationStr = "36 mins";
            tripDriver.days = "Mon,Tue,Wed,Thu,Fri";

            var json = JsonConvert.SerializeObject(tripDriver);
            Console.WriteLine("================ A ================ ");
            //response.EnsureSuccessStatusCode();
            Console.WriteLine("Json " + json.ToString());
            CreateTripPassAsync(tripDriver);
            //CreateTripDriverAsync(tripDriver);

            */

            ISharedPreferences pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private);
            var member = pref.GetString(GetString(Resource.String.PreferenceSavedMember), "");
            
            if (!member.Equals(""))
            {
                Intent intent = new Intent(this, typeof(MainActivity));                
                intent.PutExtra("Member", member);
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                StartActivity(intent);
                Finish();
            }
            else
            {
                Intent intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            }            
        }
        async void CreateTripPassAsync(TripPassenger trip)
        {
            RunOnUiThread(() =>
            {
                progress.Show();
            });

            HttpResponseMessage response = await client.PostAsJsonAsync("api/TripPassengers", trip);
            Console.WriteLine("================ 1 ================ ");
            //response.EnsureSuccessStatusCode();
            Console.WriteLine("Response code " + response.StatusCode.ToString());
            Console.WriteLine("Response code 2 " + response.RequestMessage.ToString());
            Console.WriteLine("Response code 3 " + response.Content.ToString());
            Console.WriteLine("================ 2 ================ ");

            if (response.IsSuccessStatusCode)
                Toast.MakeText(this, "Trip created.", ToastLength.Short).Show();
            else
                Toast.MakeText(this, "Failed to create trip.", ToastLength.Short).Show();

            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
        }

        async void CreateTripDriverAsync(TripDriver trip)
        {
            RunOnUiThread(() =>
            {
                progress.Show();
            });

            HttpResponseMessage response = await client.PostAsJsonAsync("api/TripDrivers", trip);
            Console.WriteLine("================ 1 ================ ");
            //response.EnsureSuccessStatusCode();
            Console.WriteLine("Response code " + response.StatusCode.ToString());
            Console.WriteLine("Response code 2 " + response.RequestMessage.ToString());
            Console.WriteLine("Response code 3 " + response.Content.ToString());
            Console.WriteLine("================ 2 ================ ");

            if (response.IsSuccessStatusCode)
                Toast.MakeText(this, "Trip created.", ToastLength.Short).Show();
            else
                Toast.MakeText(this, "Failed to create trip.", ToastLength.Short).Show();

            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
        }
    }
}