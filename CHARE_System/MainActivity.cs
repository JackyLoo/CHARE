﻿using Android.Animation;
using Android.App;
using Android.Content;
using Android.Gms.Location.Places.UI;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using Mikepenz.MaterialDrawer;
using Mikepenz.MaterialDrawer.Models;
using Mikepenz.MaterialDrawer.Models.Interfaces;
using Mikepenz.Typeface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static Android.Animation.Animator;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace CHARE_System
{
    [Activity(Label = "CHARE_App")]
        
    public class MainActivity : ActionBarActivity, IOnMapReadyCallback, ILocationListener, Drawer.IOnDrawerItemClickListener, AccountHeader.IOnAccountHeaderListener,
        IDialogInterfaceOnClickListener, NetworkBroadcastReceiver.NetworkListener, IAnimatorListener
    {
        // Network State Change Broadcast receiver & Listener
        private NetworkBroadcastReceiver receiver;
        private TextView tvNetworkNotification;
        private IntentFilter intentFilter;

        private Member user;    
        private GoogleMap mMap;
        private Geocoder geocoder;
        private LatLng originLatLng;
        private LatLng destLatLng;
        
        // Navigation Bar
        AccountHeader headerResult = null;
                
        // ILocationListener : Variables for auto change camera to user location
        private LocationManager locationManager;
        private const long MIN_TIME = 400;
        private const float MIN_DISTANCE = 1000;

        // Variables for Google Direction API
        // Sample htt://maps.googleapis.com/maps/api/directions/json?origin=Disneyland&destination=Universal+Studios+Hollywood4&key=YOUR_API_KEY       
        PlaceAutocompleteFragment originAutocompleteFragment;
        PlaceAutocompleteFragment destAutocompleteFragment;

        private string originTxt;        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            tvNetworkNotification = (TextView)FindViewById(Resource.Id.network_notification);

            ConnectivityManager cm = (ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
            {
                var ad = new Android.Support.V7.App.AlertDialog.Builder(this);
                ad.SetTitle("No Connection");
                ad.SetMessage("Looks like there's a problem with your network connection. Try again later.");
                ad.SetCancelable(false);
                ad.SetPositiveButton("OK", this);
                Android.Support.V7.App.AlertDialog dialog = ad.Create();
                dialog.Show();
            }
            else
            {
                receiver = new NetworkBroadcastReceiver(this);
                receiver.SetListener(this);
                intentFilter = new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE");
                RegisterReceiver(receiver, intentFilter);

                MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
                mapFragment.GetMapAsync(this);

                var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);

                geocoder = new Geocoder(this);
                locationManager = (LocationManager)GetSystemService(Context.LocationService);
                //You can also use LocationManager.GPS_PROVIDER and LocationManager.PASSIVE_PROVIDER     
                locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, MIN_TIME, MIN_DISTANCE, this);
                //You can also use LocationManager.GPS_PROVIDER and LocationManager.PASSIVE_PROVIDER                 
                locationManager.RequestLocationUpdates(LocationManager.GpsProvider, MIN_TIME, MIN_DISTANCE, this);

                // Deserialize the member object
                ISharedPreferences pref = GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private);
                var member = pref.GetString(GetString(Resource.String.PreferenceSavedMember), "");
                user = JsonConvert.DeserializeObject<Member>(member);

                var profile = new ProfileDrawerItem();
                profile.WithName(user.username);
                //profile.WithSelectable(false);
                //profile.WithEnabled(false);
                
                
                profile.WithIdentifier(100);

                headerResult = new AccountHeaderBuilder()
                    .WithActivity(this)                    
                    .WithHeaderBackground(Resource.Drawable.profilebackground)
                    .WithSelectionListEnabledForSingleProfile(false)
                    .AddProfiles(profile)
                    .WithOnAccountHeaderListener(this)
                    .WithSavedInstance(bundle)
                    .Build();

                var header = new PrimaryDrawerItem();
                header.WithName(Resource.String.Drawer_Item_Trips);
                header.WithIcon(GoogleMaterial.Icon.GmdDirectionsCar);
                header.WithIdentifier(1);

                /*
                var secondaryDrawer = new SecondaryDrawerItem();
                secondaryDrawer.WithName(Resource.String.Drawer_Item_About);
                secondaryDrawer.WithIcon(GoogleMaterial.Icon.GmdInfo);
                secondaryDrawer.WithIdentifier(2);
                */

                var logoutDrawer = new SecondaryDrawerItem();
                logoutDrawer.WithName(Resource.String.Drawer_Item_Logout);
                logoutDrawer.WithIcon(GoogleMaterial.Icon.GmdSettingsPower);
                logoutDrawer.WithIdentifier(4);

                //create the drawer and remember the `Drawer` result object
                Drawer result = new DrawerBuilder()
                    .WithActivity(this)
                    .WithToolbar(toolbar)
                    .WithAccountHeader(headerResult)
                    .AddDrawerItems(
                        header,
                        new DividerDrawerItem(),                
                        logoutDrawer
                    )
                    .WithOnDrawerItemClickListener(this)
                .Build();

                originAutocompleteFragment = (PlaceAutocompleteFragment)
                    FragmentManager.FindFragmentById(Resource.Id.place_autocomplete_origin_fragment);
                originAutocompleteFragment.SetHint("Enter the origin");
                originAutocompleteFragment.PlaceSelected += OnOriginSelectedAsync;

                destAutocompleteFragment = (PlaceAutocompleteFragment)
                    FragmentManager.FindFragmentById(Resource.Id.place_autocomplete_destination_fragment);
                destAutocompleteFragment.SetHint("Enter the destination");
                destAutocompleteFragment.PlaceSelected += OnDestinationSelected;
            }          
        }

        private void OnDestinationSelected(object sender, PlaceSelectedEventArgs e)
        {               
            // Set destination latlng to iDestLatLng.
            destLatLng = e.Place.LatLng;
            
            // Instantiate trip 
            Trip trip = new Trip();
            trip.originLatLng= originLatLng.Latitude.ToString() + "," + originLatLng.Longitude.ToString();
            trip.destinationLatLng = destLatLng.Latitude.ToString() + "," + destLatLng.Longitude.ToString();
            trip.origin = originTxt;
            trip.destination = e.Place.NameFormatted.ToString();

            Intent intent = new Intent(this, typeof(TripConfirmation_1));
            intent.PutExtra("Member", JsonConvert.SerializeObject(user));
            intent.PutExtra("Trip", JsonConvert.SerializeObject(trip));
            StartActivity(intent);           
        }
        
        private void OnOriginSelectedAsync(object sender, PlaceSelectedEventArgs e)
        {
            mMap.Clear();
            originTxt = e.Place.NameFormatted.ToString();
            originLatLng = e.Place.LatLng;

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(originLatLng, 18);
            mMap.AddMarker(new MarkerOptions().SetPosition(originLatLng).SetTitle("Origin"));
            mMap.AnimateCamera(cameraUpdate);            
        }    
            
        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
            mMap.MyLocationEnabled = true;
        }
        
        public void OnLocationChanged(Location location)
        {
            // Zoom camera to the device's location
            LatLng latLng = new LatLng(location.Latitude, location.Longitude);
            originLatLng = latLng;
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(latLng, 18);
            // Move camera to user current location
            mMap.AddMarker(new MarkerOptions().SetPosition(originLatLng).SetTitle("Origin"));
            mMap.MoveCamera(cameraUpdate);

            try
            {                               
                // Get current location address and set to origin textfield                                       
                IList<Address> addresses = geocoder.GetFromLocation(location.Latitude, location.Longitude, 1);
                originTxt = addresses[0].SubLocality;                
                originAutocompleteFragment.SetText(addresses[0].GetAddressLine(0));
                locationManager.RemoveUpdates(this);
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "Error occur when requesting location.", ToastLength.Long).Show();
                Toast.MakeText(this, e.ToString(), ToastLength.Long).Show();                
            }
        }   

        public void OnProviderDisabled(string provider){}

        public void OnProviderEnabled(string provider){}

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras){}
        
        public bool OnItemClick(View view, int position, IDrawerItem drawerItem)
        {
            Console.WriteLine("=== " + position);
            if (drawerItem != null)
            {
                Intent intent;
                switch (position)
                {                    
                    case 1:                        
                        if(user.type.Equals("Passenger"))
                            intent = new Intent(this, typeof(TripPassListViewActivity));
                        else if (user.type.Equals("Driver"))
                            intent = new Intent(this, typeof(TripDriverListViewActivity));
                        else
                            intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        break;
                    case 3:
                        GetSharedPreferences(GetString(Resource.String.PreferenceFileName), FileCreationMode.Private)
                        .Edit()
                        .Clear()
                        .Commit();
                        intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                        Finish();                        
                        break;
                    
                }
            }
            return false;
        }      

        public bool OnProfileChanged(View view, IProfile profile, bool current)
        {
            if (user.type.Equals("Driver"))
            {
                Intent intent = new Intent(this, typeof(EditDriverDetailActivity));
                StartActivityForResult(intent, 0);                     
            }
            else if (user.type.Equals("Passenger"))
            {
                Intent intent = new Intent(this, typeof(EditPassengerDetailActivity));
                StartActivityForResult(intent, 0); 
            }       
            return true;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);            
            if (resultCode == 0)
            {
                this.Recreate();
            }
        }    

        public void OnClick(IDialogInterface dialog, int which)
        {
            Finish();
        }

        public void OnNetworkChange()
        {
            if (receiver.GetStatus())
            {
                tvNetworkNotification.Animate().TranslationY(0).Alpha(0f).SetListener(this);
            }
            else
            {
                tvNetworkNotification.Animate().TranslationY(0).Alpha(1.0f).SetListener(this);
            }
        }

        public void OnAnimationCancel(Animator animation)
        {
        }

        public void OnAnimationEnd(Animator animation)
        {
            if (receiver.GetStatus())
                tvNetworkNotification.Visibility = ViewStates.Gone;
            else
                tvNetworkNotification.Visibility = ViewStates.Visible;
        }

        public void OnAnimationRepeat(Animator animation){}

        public void OnAnimationStart(Animator animation){}

        protected override void OnDestroy()
        {
            UnregisterReceiver(receiver);            
            base.OnDestroy();
        }
    }
}