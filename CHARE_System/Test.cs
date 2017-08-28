using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
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
    //[Activity(Label = "Test", MainLauncher = true, Icon = "@drawable/icon")]
    [Activity(Label = "Test")]
    public class Test : ActionBarActivity,
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
            SetContentView(Resource.Layout.test);

            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Submitting...");
            progress.SetCancelable(false);

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
            mapFragment.GetMapAsync(this);

            button = (Button)FindViewById(Resource.Id.button);

            iTripDetail = JsonConvert.DeserializeObject<TripDetails>(Intent.GetStringExtra("Trip"));

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
        }

        protected void BuildGoogleApiClient()
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

        public async void CreateGeofences()
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

        public void PopulateGeofenceList()
        {
            LatLng w;
            CircleOptions circleOptions;
            string[] latlng;
            int i = 0;
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

                // Draw Circle    
                w = new LatLng(double.Parse(latlng[0]), double.Parse(latlng[1]));
                circleOptions = new CircleOptions()
                    .InvokeCenter(w)
                    .InvokeFillColor(Color.Argb(0x30, 0, 0xff, 0))
                    .InvokeRadius(Constants.GEOFENCE_RADIUS_IN_METERS);
                mMap.AddCircle(circleOptions);
                i++;
            }

            latlng = iTripDetail.destinationLatLng.Split(',');
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

            // Draw Circle    
            w = new LatLng(double.Parse(latlng[0]), double.Parse(latlng[1]));
            circleOptions = new CircleOptions()
                .InvokeCenter(w)
                .InvokeFillColor(Color.Argb(0x30, 0, 0xff, 0))
                .InvokeRadius(Constants.GEOFENCE_RADIUS_IN_METERS);
            mMap.AddCircle(circleOptions);
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

            string strGoogleDirection = await MapHelper.DownloadStringAsync(iTripDetail);

            var googleDirectionAPIRoute = JsonConvert.DeserializeObject<GoogleDirectionAPI>(strGoogleDirection);
            string encodedPoints = googleDirectionAPIRoute.routes[0].overview_polyline.points;
            var lstDecodedPoints = MapHelper.DecodePolylinePoint(encodedPoints);
            //convert list of location point to array of latlng type

            var latLngPoints = lstDecodedPoints.ToArray();
            var polylineoption = new PolylineOptions();
            polylineoption.InvokeColor(Android.Graphics.Color.SkyBlue);
            polylineoption.Geodesic(true);
            polylineoption.Add(latLngPoints);
            mMap.AddPolyline(polylineoption);
            DrawMarkers(iTripDetail);
            PopulateGeofenceList();
            CreateGeofences();
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

            foreach (TripPassenger tp in tripDetail.TripPassengers)
            {
                string[] w = tp.originLatLng.Split(',');
                LatLng waypoint = new LatLng(double.Parse(w[0]), double.Parse(w[1]));
                mMap.AddMarker(new MarkerOptions().SetPosition(waypoint).SetTitle("waypoint"));
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

        public void FinishCarpool(System.Object sender, EventArgs e)
        {
            button.Enabled = false;
            Console.WriteLine("==== Arrive Method Entered");
            int commenter = iTripDetail.DriverID;
            foreach (TripPassenger tp in iTripDetail.TripPassengers)
            {
                Dialog dialog = new Dialog(this);
                dialog.SetContentView(Resource.Layout.Custom_Dialog_Rating);
                dialog.SetTitle("Rating");

                TextView tvTitle = (TextView)dialog.FindViewById(Resource.Id.title);
                RatingBar ratingbar = (RatingBar)dialog.FindViewById(Resource.Id.ratingbar);
                EditText etComment = (EditText)dialog.FindViewById(Resource.Id.comment);
                Button btnSubmit = (Button)dialog.FindViewById(Resource.Id.btn_submit);
                Button btnCancel = (Button)dialog.FindViewById(Resource.Id.btn_cancel);

                tvTitle.Text = "Rate " + tp.Member.username;
                btnSubmit.Click  += async (sender2, e2) =>
                {
                    RunOnUiThread(() =>
                    {
                        progress.Show();
                    });

                    string comment = etComment.Text.ToString();
                    int rating = (int) ratingbar.Rating;
                    int rater = iTripDetail.Member.MemberID;
                    int member = tp.Member.MemberID;
                    Rating rate = new Rating(rater, member, rating, comment);
                    
                    await RESTClient.CreateRatingAsync(this, rate);
                    RunOnUiThread(() =>
                    {
                        progress.Dismiss();
                    });
                    dialog.Dismiss();
                };

                btnCancel.Click += (sender2, e2) =>                                    
                    dialog.Dismiss();              
                
                dialog.Show();
            }
        }

        public void OnTransitionStateChange()
        {
            Console.WriteLine("=== State " + receiver.TransitionState());
            if (receiver.TransitionState().Equals("Entered"))
            {
                button.Enabled = true;
                if (receiver.GeofenceIDs().Equals(iTripDetail.TripPassengers.Count.ToString()))
                {
                    Console.WriteLine("==== Arrive");
                    button.Text = "Arrive";
                    button.Click += FinishCarpool;
                }
                else
                {
                    button.Text = "Pickup Passenger";
                    button.Click += PickupPassenger;
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