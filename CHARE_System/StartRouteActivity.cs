using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using Java.Lang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CHARE_System
{    
    [Activity(Label = "Start Route")]
    public class StartRouteActivity : Activity,
        GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener,
        IOnMapReadyCallback, GeofenBroadcastReceiver.GeofenceListener
    {
        private TripDetails iTripDetail;
        private Button button;

        GoogleMap mMap;
        protected const string TAG = "creating-and-monitoring-geofences";
        protected GoogleApiClient mGoogleApiClient;
        protected IList<IGeofence> mGeofenceList;
        bool mGeofencesAdded;
        PendingIntent mGeofencePendingIntent;
        ISharedPreferences mSharedPreferences;

        GeofenBroadcastReceiver receiver;
        IntentFilter intentFilter;

        private ProgressDialog progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StartRoute);

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading...");
            progress.SetCancelable(false);
            RunOnUiThread(() =>            
            {
                progress.Show(); 
            });        

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
            mapFragment.GetMapAsync(this);

            button = (Button)FindViewById(Resource.Id.button);

            iTripDetail = JsonConvert.DeserializeObject<TripDetails>(Intent.GetStringExtra("Trip"));

            for (int i = 0; i < iTripDetail.TripPassengers.Count; i++)
            {
                Console.WriteLine("=== passenger 2" + iTripDetail.TripPassengers[i].origin);
            }

            receiver = new GeofenBroadcastReceiver();
            receiver.SetListener(this);
            intentFilter = new IntentFilter("transition_change");
            RegisterReceiver(receiver, intentFilter);

            mGeofenceList = new List<IGeofence>();
            mGeofencePendingIntent = null;

            mSharedPreferences = GetSharedPreferences(Constants.SHARED_PREFERENCES_NAME,
                FileCreationMode.Private);

            mGeofencesAdded = mSharedPreferences.GetBoolean(Constants.GEOFENCES_ADDED_KEY, false);

            BuildGoogleApiClient();
            SavePreference();
        }

        private void ClearPreference()
        {
            GetSharedPreferences(GetString(Resource.String.PreferenceFileNameActivity), FileCreationMode.Private)
                      .Edit()
                      .Remove(GetString(Resource.String.PreferenceFileNameActivity))
                      .Remove(GetString(Resource.String.PreferenceSavedTrip))
                      .Commit();
        }

        private void SavePreference()
        {
            GetSharedPreferences(GetString(Resource.String.PreferenceFileNameActivity), FileCreationMode.Private)
                .Edit()
                .PutString(GetString(Resource.String.PreferenceSavedActivity), "StartRouteActivity")
                .PutString(GetString(Resource.String.PreferenceSavedTrip), Intent.GetStringExtra("Trip"))
                .Commit();
        }

        private void BuildGoogleApiClient()
        {
            mGoogleApiClient = new GoogleApiClient.Builder(this)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(LocationServices.API)
                .Build();
        }

        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mGoogleApiClient.Disconnect();
        }

        public void OnConnected(Bundle connectionHint)
        {
            Log.Info(TAG, "Connected to GoogleApiClient");
        }

        public void OnConnectionSuspended(int cause)
        {
            Log.Info(TAG, "Connection suspended");
        }

        public void OnConnectionFailed(Android.Gms.Common.ConnectionResult result)
        {
            Log.Info(TAG, "Connection failed: ConnectionResult.getErrorCode() = " + result.ErrorCode);
        }

        private async void CreateGeofences()
        {
            if (!mGoogleApiClient.IsConnected)
            {
                Toast.MakeText(this, GetString(Resource.String.not_connected), ToastLength.Short).Show();
            }

            try
            {
                var status = await LocationServices.GeofencingApi.AddGeofencesAsync(mGoogleApiClient, GetGeofencingRequest(),
                    GetGeofencePendingIntent());

            }
            catch (SecurityException securityException)
            {
                LogSecurityException(securityException);
            }
        }

        public async void RemoveGeofencesButtonHandler(object sender, EventArgs e)
        {
            if (!mGoogleApiClient.IsConnected)
            {
                Toast.MakeText(this, GetString(Resource.String.not_connected), ToastLength.Short).Show();
                return;
            }
            try
            {
                var status = await LocationServices.GeofencingApi.RemoveGeofencesAsync(mGoogleApiClient,
                    GetGeofencePendingIntent()); ;
            }
            catch (SecurityException securityException)
            {
                LogSecurityException(securityException);
            }
        }

        void LogSecurityException(SecurityException securityException)
        {
            Log.Error(TAG, "Invalid location permission. " +
                "You need to use ACCESS_FINE_LOCATION with geofences", securityException);
        }

        public void HandleResult(Statuses status)
        {
            if (status.IsSuccess)
            {
                mGeofencesAdded = !mGeofencesAdded;
                var editor = mSharedPreferences.Edit();
                editor.PutBoolean(Constants.GEOFENCES_ADDED_KEY, mGeofencesAdded);
                editor.Commit();

                Toast.MakeText(
                    this,
                    GetString(mGeofencesAdded ? Resource.String.geofences_added :
                        Resource.String.geofences_removed),
                    ToastLength.Short
                ).Show();
            }
            else
            {
                var errorMessage = GeofenceErrorMessages.GetErrorString(this,
                    status.StatusCode);
                Log.Error(TAG, errorMessage);
            }
        }

        GeofencingRequest GetGeofencingRequest()
        {
            var builder = new GeofencingRequest.Builder();
            builder.SetInitialTrigger(GeofencingRequest.InitialTriggerEnter);
            builder.AddGeofences(mGeofenceList);

            return builder.Build();
        }

        PendingIntent GetGeofencePendingIntent()
        {
            if (mGeofencePendingIntent != null)
            {
                return mGeofencePendingIntent;
            }

            var intent = new Intent(this, typeof(GeofenceTransitionsIntentService));
            return PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        private void PopulateGeofenceList()
        {
            LatLng w;
            CircleOptions circleOptions;
            string[] latlng;
            int i = 0;
            if (iTripDetail.TripPassengers != null)
            {
                foreach (TripPassenger tp in iTripDetail.TripPassengers)
                {
                    latlng = tp.originLatLng.Split(',');
                    mGeofenceList.Add(new GeofenceBuilder()
                        .SetRequestId(i.ToString())
                        .SetCircularRegion(
                            double.Parse(latlng[0]),
                            double.Parse(latlng[1]),
                            Constants.GEOFENCE_RADIUS_IN_METERS
                        )
                        .SetExpirationDuration(Constants.GEOFENCE_EXPIRATION_IN_MILLISECONDS)
                        .SetTransitionTypes(Geofence.GeofenceTransitionEnter |
                            Geofence.GeofenceTransitionExit)
                        .Build());

                    /*
                    // Draw Circle    
                    w = new LatLng(double.Parse(latlng[0]), double.Parse(latlng[1]));
                    circleOptions = new CircleOptions()
                        .InvokeCenter(w)
                        .InvokeFillColor(Color.Argb(0x30, 0, 0xff, 0))
                        .InvokeRadius(Constants.GEOFENCE_RADIUS_IN_METERS);
                        
                    mMap.AddCircle(circleOptions);
                    */
                    i++;
                }
            }
            if(iTripDetail.Member.type.Equals("Driver"))            
                latlng = iTripDetail.destinationLatLng.Split(',');
            else
                latlng = iTripDetail.TripDriver.destinationLatLng.Split(',');

            mGeofenceList.Add(new GeofenceBuilder()
                .SetRequestId(i.ToString())
                .SetCircularRegion(
                    double.Parse(latlng[0]),
                    double.Parse(latlng[1]),
                    Constants.GEOFENCE_RADIUS_IN_METERS
                )
                .SetExpirationDuration(Constants.GEOFENCE_EXPIRATION_IN_MILLISECONDS)
                .SetTransitionTypes(Geofence.GeofenceTransitionEnter |
                    Geofence.GeofenceTransitionExit)
                .Build());

            /*
            // Draw Circle    
            w = new LatLng(double.Parse(latlng[0]), double.Parse(latlng[1]));
            circleOptions = new CircleOptions()
                .InvokeCenter(w)
                .InvokeFillColor(Color.Argb(0x30, 0, 0xff, 0))
                .InvokeRadius(Constants.GEOFENCE_RADIUS_IN_METERS);
            mMap.AddCircle(circleOptions);
            */
        }

        private async void AddGeofence()
        {
            if (!mGoogleApiClient.IsConnected)
            {
                Toast.MakeText(this, GetString(Resource.String.not_connected), ToastLength.Short).Show();
                return;
            }

            try
            {
                var status = await LocationServices.GeofencingApi.AddGeofencesAsync(mGoogleApiClient, GetGeofencingRequest(),
                    GetGeofencePendingIntent());
            }
            catch (SecurityException securityException)
            {
                LogSecurityException(securityException);
            }
        }

        public async void OnMapReady(GoogleMap googleMap)
        {            
            mMap = googleMap;
            mMap.MyLocationEnabled = true;
            string strGoogleDirection;
            
            if (iTripDetail.Member.type.Equals("Driver"))
            {                
                strGoogleDirection = await MapHelper.DownloadStringAsync(iTripDetail);
            }
            else
            {                
                strGoogleDirection = await MapHelper.DownloadStringAsync(iTripDetail, "Passenger");
                button.Text = "Driver Arrive";
                button.Enabled = true;                                    
                button.Click += PickupByDriver;                
            }            

            var googleDirectionAPIRoute = JsonConvert.DeserializeObject<GoogleDirectionAPI>(strGoogleDirection);
            string ePoints = googleDirectionAPIRoute.routes[0].overview_polyline.points;
            var dPoints = MapHelper.DecodePolylinePoint(ePoints);
            var latLngPoints = dPoints.ToArray();
            var polylineoption = new PolylineOptions();            
            polylineoption.InvokeColor(Android.Graphics.Color.SkyBlue);
            polylineoption.Geodesic(true);
            polylineoption.Add(latLngPoints);
            mMap.AddPolyline(polylineoption);
            DrawMarkers(iTripDetail);
            PopulateGeofenceList();
            CreateGeofences();
            RunOnUiThread(() =>
            {            
               progress.Dismiss();
            });        
        }

        private void DrawMarkers(TripDetails tripDetail)
        {
            // Draw Markers            
            string[] origin = tripDetail.originLatLng.Split(',');
            string[] destination = tripDetail.destinationLatLng.Split(',');
            LatLng o = new LatLng(double.Parse(origin[0]), double.Parse(origin[1]));
            LatLng d = new LatLng(double.Parse(destination[0]), double.Parse(destination[1]));
            mMap.AddMarker(new MarkerOptions().SetPosition(o).SetTitle("Origin"));
            mMap.AddMarker(new MarkerOptions().SetPosition(d).SetTitle("Destination"));            
            if (tripDetail.TripPassengers != null)
            {
                foreach (TripPassenger tp in tripDetail.TripPassengers)
                {
                    string[] w = tp.originLatLng.Split(',');
                    LatLng waypoint = new LatLng(double.Parse(w[0]), double.Parse(w[1]));
                    mMap.AddMarker(new MarkerOptions().SetPosition(waypoint).SetTitle("waypoint"));
                }
            }            
            // Zoom map to the set padding
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            builder.Include(o);
            builder.Include(d);
            LatLngBounds bounds = builder.Build();
            int padding = 100;
            CameraUpdate cu = CameraUpdateFactory.NewLatLngBounds(bounds, padding);
            mMap.AnimateCamera(cu);
        }

        public void PickupPassenger(System.Object sender, EventArgs e)
        {
            button.Enabled = false;
            Toast.MakeText(this, "Pickup Passenger", ToastLength.Short).Show();
        }

        public void PickupByDriver(System.Object sender, EventArgs e)
        {
            button.Enabled = false;
            Toast.MakeText(this, "Driver has pick me up.", ToastLength.Short).Show();
        }

        public void FinishCarpool(System.Object sender, EventArgs e)
        {            
            if (iTripDetail.Member.type.Equals("Driver"))
            {
                int i = iTripDetail.TripPassengers.Count;                

                foreach (TripPassenger tp in iTripDetail.TripPassengers)
                {
                    Dialog dialog = new Dialog(this);
                    dialog.SetContentView(Resource.Layout.Custom_Dialog_Rating);

                    TextView tvTitle = (TextView)dialog.FindViewById(Resource.Id.title);
                    RatingBar ratingbar = (RatingBar)dialog.FindViewById(Resource.Id.ratingbar);
                    EditText etComment = (EditText)dialog.FindViewById(Resource.Id.comment);
                    Button btnSubmit = (Button)dialog.FindViewById(Resource.Id.btn_submit);
                    Button btnCancel = (Button)dialog.FindViewById(Resource.Id.btn_cancel);

                    tvTitle.Text = "Rate " + tp.Member.username;

                    btnSubmit.Click += async (sender2, e2) =>
                    {
                        string comment = etComment.Text.ToString();
                        int rating = (int)ratingbar.Rating;
                        int rater = iTripDetail.Member.MemberID;
                        int member = tp.Member.MemberID;

                        if (rating != 0)
                        {
                            progress.SetMessage("Submitting...");
                            RunOnUiThread(() =>
                            {
                                progress.Show();
                            });

                            Rating rate = new Rating(rater, member, rating, comment);
                            await RESTClient.CreateRatingAsync(this, rate);
                            RunOnUiThread(() =>
                            {
                                progress.Dismiss();
                            });
                        }
                        else
                            Toast.MakeText(this, "Set the rating star", ToastLength.Short).Show();

                        if (i == 1)
                        {
                            Intent intent = new Intent(this, typeof(MainActivity));
                            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                            StartActivity(intent);
                            Finish();
                        }
                        i--;
                        ClearPreference();
                        dialog.Dismiss();
                    };

                    btnCancel.Click += (sender2, e2) =>
                    {
                        if (i == 1)
                        {
                            ClearPreference();
                            Intent intent = new Intent(this, typeof(MainActivity));
                            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                            StartActivity(intent);
                            Finish();
                        }
                        i--;
                        dialog.Dismiss();
                    };

                    dialog.Show();
                }
            }
            else
            {
                Dialog dialog = new Dialog(this);
                dialog.SetContentView(Resource.Layout.Custom_Dialog_Rating);

                TextView tvTitle = (TextView)dialog.FindViewById(Resource.Id.title);
                RatingBar ratingbar = (RatingBar)dialog.FindViewById(Resource.Id.ratingbar);
                EditText etComment = (EditText)dialog.FindViewById(Resource.Id.comment);
                Button btnSubmit = (Button)dialog.FindViewById(Resource.Id.btn_submit);
                Button btnCancel = (Button)dialog.FindViewById(Resource.Id.btn_cancel);

                tvTitle.Text = "Rate " + iTripDetail.TripDriver.Member.username;

                btnSubmit.Click += async (sender2, e2) =>
                {
                    string comment = etComment.Text.ToString();
                    int rating = (int)ratingbar.Rating;
                    int rater = iTripDetail.Member.MemberID;
                    int member = iTripDetail.TripDriver.Member.MemberID;

                    if (rating != 0)
                    {
                        progress.SetMessage("Submitting...");
                        RunOnUiThread(() =>
                        {
                            progress.Show();
                        });

                        Rating rate = new Rating(rater, member, rating, comment);
                        await RESTClient.CreateRatingAsync(this, rate);
                        RunOnUiThread(() =>
                        {
                            progress.Dismiss();
                        });
                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                        StartActivity(intent);
                        Finish();
                        ClearPreference();
                        dialog.Dismiss();
                    }
                    else
                        Toast.MakeText(this, "Set the rating star", ToastLength.Short).Show();                                        
                };

                btnCancel.Click += (sender2, e2) =>
                {                    
                    ClearPreference();
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                    StartActivity(intent);
                    Finish();                    
                    dialog.Dismiss();
                };

                dialog.Show();
            }
        }

        public void OnTransitionStateChange()
        {            
            if (receiver.TransitionState().Equals("Entered"))
            {                
                button.Enabled = true;
                if (iTripDetail.Member.type.Equals("Driver"))
                {                    
                    if (receiver.GeofenceIDs().Equals(iTripDetail.TripPassengers.Count.ToString()))
                    {                        
                        button.Text = "Arrive";
                        button.Click += FinishCarpool;
                    }
                    else
                    {
                        button.Text = "Pickup Passenger";
                        button.Click += PickupPassenger;
                    }
                }
                else if(iTripDetail.Member.type.Equals("Passenger"))
                {                                        
                    button.Text = "Arrive";
                    button.Click += FinishCarpool;                    
                }
                else
                {
                    button.Text = "Arrive";
                    button.Click += FinishCarpool;
                }

            }
            else if (receiver.TransitionState().Equals("Exited"))
            {
                button.Enabled = false;
                if (receiver.GeofenceIDs().Equals((iTripDetail.TripPassengers.Count - 1)))
                    button.Text = "Arrive";
            }
        }
    }
}