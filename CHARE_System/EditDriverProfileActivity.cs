using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using Newtonsoft.Json;
using CHARE_System.Class;
using Android.Views;
using System.Text.RegularExpressions;

namespace CHARE_System
{
    [Activity(Label = "Edit Account")]
    public class EditDriverProfileActivity : Activity
    {
        private ProgressDialog progress;

        private Member user;  
        // Member
        private EditText etUsername;
        private EditText etPassword;
        private EditText etConPassword;
        private EditText etPhone;
        // Vehicle
        private Spinner spnGender;
        private Spinner spnMake;
        private Spinner spnModel;
        private Spinner spnColor;
        private EditText etCarplate;
        
        private Button buttonUpdate;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DriverDetails_Edit);            
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
            else
            {
                progress = new Android.App.ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetMessage("Loading ...");
                progress.SetCancelable(false);

                ISharedPreferences pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private);
                var member = pref.GetString(GetString(Resource.String.PreferenceSavedMember), "");
                user = JsonConvert.DeserializeObject<Member>(member);

                // Member variable views 
                etUsername = (EditText)FindViewById(Resource.Id.edittext_username);
                etPassword = (EditText)FindViewById(Resource.Id.edittext_password);
                etConPassword = (EditText)FindViewById(Resource.Id.edittext_confirm_password);
                etPhone = (EditText)FindViewById(Resource.Id.edittext_phone);
                spnGender = (Spinner)FindViewById(Resource.Id.spinner_gender);
                // Vehicle variable views
                spnMake = (Spinner)FindViewById(Resource.Id.carmake);
                spnModel = (Spinner)FindViewById(Resource.Id.carmodel);
                spnColor = (Spinner)FindViewById(Resource.Id.color);
                etCarplate = (EditText)FindViewById(Resource.Id.edittext_carplate);

                buttonUpdate = (Button)FindViewById(Resource.Id.button_update);

                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.signup_gender, Resource.Layout.Custom_Spinner_Edit_Details);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spnGender.Adapter = adapter;

                etUsername.Text = user.username;
                etPassword.Text = user.password;
                etConPassword.Text = user.password;
                etPhone.Text = user.phoneno;
                if (user.gender.Equals("Male"))
                    spnGender.SetSelection(1);
                else
                    spnGender.SetSelection(2);
                etCarplate.Text = user.Vehicles[0].plateNo;

                InitializeVehicleSpinners();
                buttonUpdate.Click += UpdateDetails;

                SetValidation();
            }
        }

        private async void InitializeVehicleSpinners()
        {
            RunOnUiThread(() =>
            {
                progress.Show();
            });
            var modelList = await RESTClient.GetCarmodelAsync(this);
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
            Array.Sort(modelList);
            ArrayAdapter<string> dataAdapter = new ArrayAdapter<string>(this, Resource.Layout.Custom_Spinner_Edit_Details, modelList);
            dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnMake.Adapter = dataAdapter;
            spnMake.SetSelection(dataAdapter.GetPosition(user.Vehicles[0].make));            

            spnMake.ItemSelected += async (sender, e) =>
            {
                var makeList = await RESTClient.GetCarmodelMakeAsync(this, spnMake.SelectedItem.ToString().Trim());
                Array.Sort(makeList);
                dataAdapter = new ArrayAdapter<string>(this, Resource.Layout.Custom_Spinner_Edit_Details, makeList);
                dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spnModel.Adapter = dataAdapter;
                spnModel.SetSelection(dataAdapter.GetPosition(user.Vehicles[0].model));
            };

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.car_color_array, Resource.Layout.Custom_Spinner_Edit_Details);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnColor.Adapter = adapter;
            spnColor.SetSelection(adapter.GetPosition(user.Vehicles[0].color));
        }

        private bool ValidateData()
        {
            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
            {
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
                return false;
            }
            else if (etUsername.Text.ToString().Trim().Equals(""))
            {
                etUsername.SetError("Username is required!", null);
                etUsername.RequestFocus();
                return false;
            }
            else if (etPassword.Text.ToString().Trim().Equals(""))
            {
                etPassword.SetError("Password is required!", null);
                etPassword.RequestFocus();
                return false;
            }
            else if (!ValidatePassword(etPassword.Text.ToString().Trim()))
            {
                etPassword.SetError("Password must be alphanumeric and at least 6 character!", null);
                etPassword.RequestFocus();
                return false;
            }
            else if (etConPassword.Text.ToString().Trim().Equals(""))
            {
                etConPassword.SetError("Confirm password is required!", null);
                etConPassword.RequestFocus();
                return false;
            }
            else if (!etPassword.Text.ToString().Equals(etConPassword.Text.ToString()))
            {
                etConPassword.SetError("Password is not same!", null);
                etConPassword.RequestFocus();
                return false;
            }
            else if (etPhone.Text.ToString().Trim().Equals(""))
            {
                etPhone.SetError("Phone is required!", null);
                etPhone.RequestFocus();
                return false;
            }
            else if (spnGender.SelectedItem.ToString().Equals("--Select Gender--"))
            {
                Toast.MakeText(this, "Select your gender.", ToastLength.Long).Show();
                return false;
            }
            else if (etUsername.Text.ToString().Trim().Contains(" "))
            {
                etUsername.SetError("Username contain space!", null);
                etUsername.RequestFocus();
                return false;
            }
            else if (etCarplate.Text.ToString().Trim().Equals(""))
            {
                etCarplate.SetError("Car plate no is required!", null);
                etCarplate.RequestFocus();
                return false;
            }
            else
                return true;
        }

        private async void UpdateDetails(object sender, EventArgs e)
        {             
            if(ValidateData())
            {                                                
                user.username = etUsername.Text.ToString().Trim();
                user.password = etPassword.Text.ToString().Trim();
                user.phoneno = etPhone.Text.ToString().Trim();
                user.gender = spnGender.SelectedItem.ToString();

                user.Vehicles[0].model = spnModel.SelectedItem.ToString();
                user.Vehicles[0].make = spnMake.SelectedItem.ToString();
                user.Vehicles[0].color = spnColor.SelectedItem.ToString();
                user.Vehicles[0].plateNo = etCarplate.Text.ToString().Trim();

                progress.SetMessage("Updating Account.....");
                RunOnUiThread(() =>
                {
                    progress.Show();
                });
                await RESTClient.UpdateMemberVehicleAsync(this, user);
                GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private)
                        .Edit()
                        .PutString(GetString(Resource.String.PreferenceSavedMember), JsonConvert.SerializeObject(user))
                        .Commit();
                RunOnUiThread(() =>
                {
                    progress.Dismiss();
                });                
            }
        }


        private static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,16}$");
            Match match = regex.Match(password);
            if (match.Success)
                return true;
            return false;
        }

        private void SetValidation()
        {
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
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent returnIntent = new Intent();
            SetResult(Result.Canceled, returnIntent);
            Finish();
            return true;
        }
    }
}