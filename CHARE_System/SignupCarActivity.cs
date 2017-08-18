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
using static Android.Widget.AdapterView;
using CHARE_REST_API.JSON_Object;
using Newtonsoft.Json;

namespace CHARE_System
{
    //[Activity(Label = "SignupCarActivity", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "SignupCarActivity")]
    public class SignupCarActivity : Activity
    {
        private ProgressDialog progress;

        private Member iMember;

        private Spinner spnMake;
        private Spinner spnModel;
        private Spinner spnColor;
        private EditText etCarplate;
        private TextView signupBtn;

        private HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Signup_Car);

            client = new HttpClient();
            client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Registering Account.....");
            progress.SetCancelable(false);

            iMember = JsonConvert.DeserializeObject<Member>(Intent.GetStringExtra("Member"));

            spnMake = (Spinner)FindViewById(Resource.Id.carmake);
            spnModel = (Spinner)FindViewById(Resource.Id.signup_carmodel);
            spnColor = (Spinner)FindViewById(Resource.Id.signup_carcolor);
            etCarplate = (EditText)FindViewById(Resource.Id.carplate);

            signupBtn = (TextView)FindViewById(Resource.Id.signupcarbtn);
            signupBtn.Click += async (sender, e) =>
            {
                if (etCarplate.Text.ToString().Trim().Equals(""))
                    etCarplate.SetError("Car plate no is required!", null);
                else
                {
                    var url = await CreateAsync(iMember);
                    iMember = await GetMemberAsync(url.PathAndQuery);

                    Vehicle vehicle = new Vehicle();
                    vehicle.model = spnModel.SelectedItem.ToString();
                    vehicle.make = spnMake.SelectedItem.ToString();
                    vehicle.color = spnColor.SelectedItem.ToString();
                    vehicle.plateNo = etCarplate.Text.ToString().Trim();
                    vehicle.MemberID = iMember.MemberID;
                   
                    if (await CreateVehicleAsync(vehicle))
                    {
                        RunOnUiThread(() =>
                        {
                            progress.Dismiss();
                        });
                        var intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                    }
                    RunOnUiThread(() =>
                    {
                        progress.Dismiss();
                    });
                }

            };

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.car_color_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnColor.Adapter = adapter;

            spnMake.ItemSelected += async (sender, e) =>
            {                                                
                var make = await GetCarmodelAsync("api/CarModels?make=" + spnMake.SelectedItem.ToString().Trim());
                var list = (make.Substring(1, make.Length - 2)).Replace('\"', ' ').Split(',');
                Array.Sort(list);
                ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, list);
                dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spnModel.Adapter = dataAdapter;
            };            
            
            InitializeMakeSpinner();
            
            var models = GetCarmodelAsync("api/CarModels");            
        }

        async void InitializeMakeSpinner()
        {
            var make = await GetCarmodelAsync("api/CarModels");
            var list = (make.Substring(1, make.Length - 2)).Replace('\"', ' ').Split(',');
            Array.Sort(list);                                    
            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, list);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnMake.Adapter = dataAdapter;
        }


        async Task<string> GetCarmodelAsync(string path)
        {
            var make = "";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }
            return make;
        }

        async Task<Uri> CreateAsync(Member member)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Members", member);
            if (!response.IsSuccessStatusCode)                            
                Toast.MakeText(this, "Failed to registered account.", ToastLength.Short).Show();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        async Task<bool> CreateVehicleAsync(Vehicle vehicle)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Vehicles", vehicle);
            if (response.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Account registered successfully.", ToastLength.Short).Show();
                return true;
            }
            else
                Toast.MakeText(this, "Failed to registered account.", ToastLength.Short).Show();
            return false;
        }

        async Task<Member> GetMemberAsync(string path)
        {
            Member member= null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                member = await response.Content.ReadAsAsync<Member>();
            }
            return member;
        }
    }
}