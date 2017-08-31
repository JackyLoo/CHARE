using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CHARE_System
{    
    [Activity(Label = "Login")]
    public class LoginActivity : Activity
    {
        private ProgressDialog progress;

        private TextView tvLoginBtn;
        private TextView tvSignuplink;
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

            tvLoginBtn = FindViewById<TextView>(Resource.Id.signinbtn);
            tvSignuplink = FindViewById<TextView>(Resource.Id.signuplink);            
            etUsername = FindViewById<EditText>(Resource.Id.et_login_username);
            etPassword = FindViewById<EditText>(Resource.Id.et_login_password);
                        
            tvLoginBtn.Click += BtnLogin_Click;
            
            tvSignuplink.Click += delegate
            {
                var intent = new Intent(this, typeof(SignupActivity));
                StartActivity(intent);
                Finish();
            };

            if (!etUsername.Text.ToString().Equals(""))
            {
                BtnLogin_Click(null,null);
            }
        }       
        
        private async void BtnLogin_Click(object sender, EventArgs e)
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
            else
            {
                string url = GetString(Resource.String.AzureAPI) + "Members?username=" + etUsername.Text.ToString() + "&password=" + etPassword.Text.ToString();

                RunOnUiThread(() =>
                {
                    progress.Show();
                });

                string downloadedString = await MapHelper.DownloadStringAsync(url);

                RunOnUiThread(() =>
                {
                    progress.Dismiss();
                });

                if (downloadedString.Equals("Exception"))
                    Toast.MakeText(this, "Invalid username or password", ToastLength.Long).Show();
                else
                {
                    var member = JsonConvert.DeserializeObject<Member>(downloadedString);

                    GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private)
                        .Edit()
                        .PutString(GetString(Resource.String.PreferenceSavedMember), JsonConvert.SerializeObject(member))
                        .Commit();

                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("Member", JsonConvert.SerializeObject(member));
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                    StartActivity(intent);
                    Finish();
                }
            }
        }
    }
}
