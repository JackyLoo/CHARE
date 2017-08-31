using Android.App;
using Android.Content;
using Android.OS;

namespace CHARE_System
{
    [Activity(Label = "CHARE", MainLauncher = true, Icon = "@drawable/carpool_logo")]    
    public class InitialActivity : Activity
    {        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);     

            ISharedPreferences pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private);
            var member = pref.GetString(GetString(Resource.String.PreferenceSavedMember), "");
            
            if (!member.Equals(""))
            {
                pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileNameActivity), FileCreationMode.Private);
                var trip = pref.GetString(GetString(Resource.String.PreferenceSavedTrip), "");
                if (trip.Equals(""))
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                    StartActivity(intent);
                    Finish();
                }
                else
                {                    
                    Intent intent = new Intent(this, typeof(StartRouteActivity));
                    intent.PutExtra("Trip", trip);
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                    StartActivity(intent);
                    Finish();
                }                
            }
            else
            {
                Intent intent = new Intent(this, typeof(LoginActivity));
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                StartActivity(intent);
                Finish();
            }            
        }
    }
}