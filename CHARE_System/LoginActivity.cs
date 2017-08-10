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
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using CHARE_System.JSON_Object;
using Newtonsoft.Json;
using CHARE_REST_API.Models;
using System.Net;
using System.IO;
using System.Runtime.Serialization;

namespace CHARE_System
{
    //[Activity(Label = "LoginActivity", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "LoginActivity")]

    public class LoginActivity : Activity
    {
        private ProgressDialog progress;

        private EditText etUsername;
        private EditText etPassword;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Login Account...");
            progress.SetCancelable(false);

            TextView signBtn = FindViewById<TextView>(Resource.Id.signinbtn);
            etUsername = FindViewById<EditText>(Resource.Id.et_login_username);
            etPassword = FindViewById<EditText>(Resource.Id.et_login_password);

            signBtn.Click += BtnLogin_Click;

            TextView signupET = FindViewById<TextView>(Resource.Id.signuplink);
            signupET.Click += delegate
            {
                var intent = new Intent(this, typeof(SignupActivity));
                StartActivity(intent);
            };
        }       
        
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            /*
            Member test = new Member();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responses = await client.GetAsync("http://charerestapi.azurewebsites.net/api/Members");
            
            if (responses.IsSuccessStatusCode)
            {
                test = await responses.Content.ReadAsAsync<Member>();
                Console.WriteLine("======================== A ========================");
                Console.WriteLine("Member iD " + test.MemberID);
            }
            else
            {
                Console.WriteLine("======================== Errir ========================");
                Console.WriteLine("Responses" + responses.ToString());
            }
            */

            if (etUsername.Text.ToString().Trim().Equals(""))
                etUsername.SetError("Username is required!", null);
            else if (etPassword.Text.ToString().Trim().Equals(""))
                etPassword.SetError("Password is required!", null);
            else
            {
                string url = GetString(Resource.String.AzureAPI) + "Members?username=" + etUsername.Text.ToString() + "&password=" + etPassword.Text.ToString();

                RunOnUiThread(() =>
                {
                    progress.Show();
                });

                string downloadedString = await fnDownloadString(url);

                RunOnUiThread(() =>
                {
                    progress.Dismiss();
                });

                if (downloadedString.Equals("Exception"))
                    Toast.MakeText(this, "Invalid username or password", ToastLength.Long).Show();
                else
                {
                    var member = JsonConvert.DeserializeObject<Member>(downloadedString);
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("Member", JsonConvert.SerializeObject(member));
                    StartActivity(intent);
                    /*
                    if (etUsername.Text.ToString().Trim().Equals(""))            
                        etUsername.SetError("Username is required!", null);                
                    else if (etPassword.Text.ToString().Trim().Equals(""))            
                        etPassword.SetError("Password is required!", null);                           
                    else
                    {
                        string url = GetString(Resource.String.AzureAPI) + "Members?username=" + etUsername.Text.ToString() + "&password=" + etPassword.Text.ToString();
                        string downloadedString = await fnDownloadString(url);

                        if (downloadedString.Equals("Exception"))
                            Toast.MakeText(this, "Invalid username or password", ToastLength.Long).Show();
                        else
                        {
                            var member = JsonConvert.DeserializeObject<Member>(downloadedString);                    
                        }                
                    }
                    */
                }               

                async Task<string> fnDownloadString(string strUri)
                {
                    WebClient webclient = new WebClient();
                    string strResultData;
                    try
                    {
                        strResultData = await webclient.DownloadStringTaskAsync(new Uri(strUri));
                        Console.WriteLine(strResultData);
                    }
                    catch
                    {
                        strResultData = "Exception";                        
                    }
                    finally
                    {
                        webclient.Dispose();
                        webclient = null;
                    }

                    return strResultData;
                }
            }
        }
    }
}