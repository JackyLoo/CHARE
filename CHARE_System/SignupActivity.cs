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
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CHARE_System
{
    //[Activity(Label = "SignupActivity", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "SignupActivity")]
    public class SignupActivity : Activity
    {
        private ProgressDialog progress;

        private HttpClient client;

        private Spinner spnGender;
        private Spinner spnType;
        private EditText etUsername;
        private EditText etPassword;
        private EditText etConPassword;
        private EditText etPhone;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Signup);

            client = new HttpClient();
            client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Registering Account.....");
            progress.SetCancelable(false);
          
            etUsername = (EditText)FindViewById(Resource.Id.signup_username);
            etPassword = (EditText)FindViewById(Resource.Id.signup_password);
            etConPassword = (EditText)FindViewById(Resource.Id.signup_con_pswrd);
            etPhone = (EditText)FindViewById(Resource.Id.signup_phone);
            spnGender = (Spinner)FindViewById(Resource.Id.signup_gender);
            spnType = (Spinner)FindViewById(Resource.Id.signup_account_type);

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.signup_gender, Resource.Layout.Custom_Spinner_Signup);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnGender.Adapter = adapter;

            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.signup_account_type, Resource.Layout.Custom_Spinner_Signup);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnType.Adapter = adapter;


            etUsername.AfterTextChanged += (sender, e) =>
            {
                if (etUsername.Text.ToString().Trim().Equals(""))
                    etUsername.SetError("Username is required!", null);
                else if (etUsername.Text.ToString().Trim().Contains(" "))
                    etUsername.SetError("Username cannot contain space!", null);
            };
            etPassword.AfterTextChanged += (sender, e) =>
            {
                if (etPassword.Text.ToString().Trim().Equals(""))
                    etPassword.SetError("Password is required!", null);
            };
            etConPassword.AfterTextChanged += (sender, e) =>
            {
                if (etConPassword.Text.ToString().Trim().Equals(""))
                    etConPassword.SetError("Confirm password is required!", null);
                else if (!etPassword.Text.ToString().Equals(etConPassword.Text.ToString()))
                    etConPassword.SetError("Password is not same!", null);
            };
            etPhone.AfterTextChanged += (sender, e) =>
            {
                if (etPhone.Text.ToString().Trim().Equals(""))
                    etPhone.SetError("Phone is required!", null);
            };

            TextView signBtn = FindViewById<TextView>(Resource.Id.signupbtn);
            signBtn.Click += async delegate
            {
                Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
                if (cm.ActiveNetworkInfo == null)
                    Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
                else if (etUsername.Text.ToString().Trim().Equals(""))
                {
                    etUsername.SetError("Username is required!", null);
                    etUsername.RequestFocus();
                }
                else if (etPassword.Text.ToString().Trim().Equals(""))
                {
                    etPassword.SetError("Password is required!", null);
                    etPassword.RequestFocus();
                }
                else if (etConPassword.Text.ToString().Trim().Equals(""))
                {
                    etConPassword.SetError("Confirm password is required!", null);
                    etConPassword.RequestFocus();
                }
                else if (!etPassword.Text.ToString().Equals(etConPassword.Text.ToString()))
                {
                    etConPassword.SetError("Password is not same!", null);
                    etConPassword.RequestFocus();
                }
                else if (etPhone.Text.ToString().Trim().Equals(""))
                {
                    etPhone.SetError("Phone is required!", null);
                    etPhone.RequestFocus();
                }
                else if (spnGender.SelectedItem.ToString().Equals("--Select Gender--"))
                    Toast.MakeText(this, "Select your gender.", ToastLength.Long).Show();
                else if (spnType.SelectedItem.ToString().Equals("--Select Account Type--"))
                    Toast.MakeText(this, "Select your account type.", ToastLength.Long).Show();
                else if (etUsername.Text.ToString().Trim().Contains(" "))
                {
                    etUsername.SetError("Username contain space!", null);
                    etUsername.RequestFocus();
                }
                else
                {
                    Member member = new Member();
                    member.username = etUsername.Text.ToString().Trim();
                    member.password = etPassword.Text.ToString().Trim();
                    member.phoneno = etPhone.Text.ToString().Trim();
                    member.gender = spnGender.SelectedItem.ToString();
                    member.type = spnType.SelectedItem.ToString();
                    member.rate = 5;

                    if (spnType.SelectedItem.ToString().Equals("Driver"))
                    {
                        progress.SetMessage("Validating data...");
                        RunOnUiThread(() =>
                        {
                            progress.Show();
                        });
                        if (!await GetMemberAsync(GetString(Resource.String.RestAPIBaseAddress) + "api/Members?username=" + member.username))
                        {
                            RunOnUiThread(() =>
                            {
                                progress.Dismiss();
                            });
                            var intent = new Intent(this, typeof(SignupCarActivity));
                            intent.PutExtra("Member", JsonConvert.SerializeObject(member));
                            StartActivity(intent);
                        }                        
                        RunOnUiThread(() =>
                        {
                            progress.Dismiss();
                        });

                        // intent.PutExtra("Trip", JsonConvert.SerializeObject(iTrip));
                    }
                    else
                    {                        
                        progress.SetMessage("Registering Account.....");
                        RunOnUiThread(() =>
                        {
                            progress.Show();
                        });
                        if (await CreateMemberAsync(member))
                        {
                            RunOnUiThread(() =>
                            {
                                progress.Dismiss();
                            });
                            var intent = new Intent(this, typeof(LoginActivity));
                            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                            StartActivity(intent);
                        }
                        RunOnUiThread(() =>
                        {
                            progress.Dismiss();
                        });
                    }
                }
            };

            TextView signupET = FindViewById<TextView>(Resource.Id.loginlink);
            signupET.Click += delegate
            {
                var intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            };
        }

        async Task<bool> CreateMemberAsync(Member member)
        {            
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Members", member);            
            if (response.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Account registered successfully.", ToastLength.Short).Show();
                return true;
            }            
            else
                Toast.MakeText(this, "Failed to registered account.", ToastLength.Short).Show();
            return false;
        }

        async Task<bool> GetMemberAsync(string path)
        {            
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Username already exist..", ToastLength.Short).Show();
                etUsername.SetError("Username already exist!", null);
                etUsername.RequestFocus();
                return true;
            }                
            return false;
        }
    }
}