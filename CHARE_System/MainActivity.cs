using Android.App;
using Android.Gms.Location.Places.UI;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location.Places;
using System.Threading.Tasks;
using System.Net;
using Android.Widget;
using Newtonsoft.Json;
using System.IO;

// ## Check before final deployment


namespace CHARE_System
{
    [Activity(Label = "CHARE_App", MainLauncher = true, Icon = "@drawable/icon")]

    // implement ILocationListener for 
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {

        private GoogleMap mMap;
        private LatLng originLatLng;
        private LatLng destLatLng;

        // ILocationListener : Variables for auto change camera to user location
        private LocationManager locationManager;
        private const long MIN_TIME = 400;
        private const float MIN_DISTANCE = 1000;

        // Variables for Google Direction API
        // Sample htt://maps.googleapis.com/maps/api/directions/json?origin=Disneyland&destination=Universal+Studios+Hollywood4&key=YOUR_API_KEY

        private const string strGoogleDirectionAPIOri = "https://maps.googleapis.com/maps/api/directions/json?origin=";
        private const string strGoogleDirectionAPIDest = "&destination=";
        // Google API Key allow HTTP Referrers
        private const string strGoogleApiKey = "&key=AIzaSyBxXCmp-C6i5LwwRSTuvzIjD9_roPjJ4EI";

        PlaceAutocompleteFragment originAutocompleteFragment;
        PlaceAutocompleteFragment destAutocompleteFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            originAutocompleteFragment = (PlaceAutocompleteFragment)
                FragmentManager.FindFragmentById(Resource.Id.place_autocomplete_origin_fragment);
            originAutocompleteFragment.SetHint("Enter the origin");            
            originAutocompleteFragment.PlaceSelected += OnOriginSelected;

            destAutocompleteFragment = (PlaceAutocompleteFragment)
                FragmentManager.FindFragmentById(Resource.Id.place_autocomplete_destination_fragment);
            destAutocompleteFragment.SetHint("Enter the destination");
            destAutocompleteFragment.PlaceSelected += OnDestinationSelected;                        

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
            mapFragment.GetMapAsync(this);

            locationManager = (LocationManager)GetSystemService(Context.LocationService);
            locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, MIN_TIME, MIN_DISTANCE, this); //You can also use LocationManager.GPS_PROVIDER and LocationManager.PASSIVE_PROVIDER     
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, MIN_TIME, MIN_DISTANCE, this); //You can also use LocationManager.GPS_PROVIDER and LocationManager.PASSIVE_PROVIDER     
        }

        private async void OnDestinationSelected(object sender, PlaceSelectedEventArgs e)
        {            
            // Set destination latlng to destLatLng.
            destLatLng = e.Place.LatLng;
            // Add destination marker to Google Map
            mMap.AddMarker(new MarkerOptions().SetPosition(destLatLng).SetTitle("Destination"));

            // Combine Google Direction API string 
            string url = strGoogleDirectionAPIOri + originLatLng.Latitude.ToString() + "," + originLatLng.Longitude.ToString() +
                strGoogleDirectionAPIDest + destLatLng.Latitude.ToString() + "," + destLatLng.Longitude.ToString() + strGoogleApiKey;

            string strGoogleDirection = await fnDownloadString(url);

            var googleDirectionAPIRoute = JsonConvert.DeserializeObject<RootObject>(strGoogleDirection);
            string encodedPoints = googleDirectionAPIRoute.routes[0].overview_polyline.points;
            var lstDecodedPoints = FnDecodePolylinePoints(encodedPoints);
            //convert list of location point to array of latlng type
            var latLngPoints = lstDecodedPoints.ToArray();
            var polylineoption = new PolylineOptions();
            polylineoption.InvokeColor(Android.Graphics.Color.SkyBlue);
            polylineoption.Geodesic(true);
            polylineoption.Add(latLngPoints);
            mMap.AddPolyline(polylineoption);
        }

        private void OnOriginSelected(object sender, PlaceSelectedEventArgs e)
        {            
            mMap.Clear();
            try
            {                                
                originLatLng = e.Place.LatLng;
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(originLatLng, 15);
                mMap.AddMarker(new MarkerOptions().SetPosition(originLatLng).SetTitle("Origin"));
                mMap.AnimateCamera(cameraUpdate);                               
            }
            catch (IOException ex)
            {
                // TODO Auto-generated catch block
                Console.WriteLine("======================= error =============================");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("====================================================");
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
            mMap.MyLocationEnabled = true;
        }

        public void OnLocationChanged(Location location)
        {
            LatLng latLng = new LatLng(location.Latitude, location.Longitude);
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(latLng, 15);
            Geocoder geocoder = new Geocoder(this);
            try
            {                
                // Get current location address and set to origin textfield               
                IList<Address> addresses = geocoder.GetFromLocation(location.Latitude, location.Longitude, 1);
                Address obj = addresses[0];
                String address = obj.GetAddressLine(0);
                originAutocompleteFragment.SetText(address);
                
                // ## Set user current latlng to Origin 
                originLatLng = latLng;
                // Move camera to user current location
                mMap.AddMarker(new MarkerOptions().SetPosition(originLatLng).SetTitle("Origin"));
                mMap.AnimateCamera(cameraUpdate);
                                                          
                locationManager.RemoveUpdates(this);
            }
            catch (IOException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine("======================= error =============================");
                Console.WriteLine(e.ToString());
                Console.WriteLine("====================================================");                               
            }
           
        }

        public void OnProviderDisabled(string provider){}

        public void OnProviderEnabled(string provider){}

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras){}

        List<LatLng> FnDecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return null;
            var poly = new List<LatLng>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylinechars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                LatLng p = new LatLng(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(p);
            }

            return poly;
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
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Unable to connect to server!!!", ToastLength.Short).Show();
                });
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


