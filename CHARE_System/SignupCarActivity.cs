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
using CHARE_System.Class;

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
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
                else if (etCarplate.Text.ToString().Trim().Equals(""))
                    etCarplate.SetError("Car plate no is required!", null);
                else
                {                                        
                    Vehicle vehicle = new Vehicle();                    
                    vehicle.model = spnModel.SelectedItem.ToString();
                    vehicle.make = spnMake.SelectedItem.ToString();
                    vehicle.color = spnColor.SelectedItem.ToString();
                    vehicle.plateNo = etCarplate.Text.ToString().Trim();                    

                    await RESTClient.CreateMemberVehicleAsync(this, iMember, vehicle);

                    var intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);

                    RunOnUiThread(() =>
                    {
                        progress.Dismiss();
                    });
                }

            };
  
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.car_color_array, Resource.Layout.Custom_Spinner_Signup);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnColor.Adapter = adapter;

            spnMake.ItemSelected += async (sender, e) =>
            {                                                
                var makeList = await RESTClient.GetCarmodelMakeAsync(this, spnMake.SelectedItem.ToString().Trim());                
                Array.Sort(makeList);
                ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Resource.Layout.Custom_Spinner_Signup, makeList);
                dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spnModel.Adapter = dataAdapter;
            };

            InitializeMakeSpinner();                        
        }

        async void InitializeMakeSpinner()
        {
            var modelList = await RESTClient.GetCarmodelAsync(this);            
            Array.Sort(modelList);                                    
            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Resource.Layout.Custom_Spinner_Signup, modelList);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnMake.Adapter = dataAdapter;
        }


  
    }
}