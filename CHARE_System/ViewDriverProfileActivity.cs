using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CHARE_System
{
    [Activity(Label = "Profile")]
    public class ViewDriverProfileActivity : Activity
    {
        private Member member;
        private ProgressDialog progress;
        private TripDetails tripDetail;
        // Member
        private TextView tvUsername;
        private TextView tvPhone;
        // Vehicle
        private TextView tvGender;
        private TextView tvMake;
        private TextView tvModel;
        private TextView tvColor;
        private TextView tvCarplate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DriverProfileDetail);
            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading driver profile...");
            progress.SetCancelable(false);

            tripDetail = JsonConvert.DeserializeObject<TripDetails>(Intent.GetStringExtra("Trip"));

            // Member variable views 
            tvUsername = (TextView)FindViewById(Resource.Id.username);
            tvPhone = (TextView)FindViewById(Resource.Id.phone);
            tvGender = (TextView)FindViewById(Resource.Id.gender);
            // Vehicle variable views
            tvMake = (TextView)FindViewById(Resource.Id.carmake);
            tvModel = (TextView)FindViewById(Resource.Id.carmodel);
            tvColor = (TextView)FindViewById(Resource.Id.color);
            tvCarplate = (TextView)FindViewById(Resource.Id.carplate);

            LoadDriverProfile(tripDetail.TripDriver.Member.MemberID);
        }

        async void LoadDriverProfile(int id)
        {
            RunOnUiThread(() =>
            {
                progress.Show();
            });

            member = await RESTClient.GetDriverProfileAsync(this, id);
            tvUsername.Text = member.username;
            tvPhone.Text = member.phoneno;
            tvGender.Text = member.gender;
            tvMake.Text = member.Vehicles[0].make;
            tvModel.Text = member.Vehicles[0].model;
            tvColor.Text = member.Vehicles[0].color;
            tvCarplate.Text = member.Vehicles[0].plateNo;

            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });                        
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}