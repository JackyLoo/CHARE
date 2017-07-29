﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace CHARE_System
{
    [Activity(Label = "TripConfirmation_1")]
    public class TripConfirmation_1 : Activity, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private LatLng originLatLng;
        private LatLng destLatLng;

        private const string strGoogleDirectionAPIOri = "https://maps.googleapis.com/maps/api/directions/json?origin=";
        private const string strGoogleDirectionAPIDest = "&destination=";
        // Google API Key allow HTTP Referrers
        private const string strGoogleApiKey = "&key=AIzaSyBxXCmp-C6i5LwwRSTuvzIjD9_roPjJ4EI";

        public async void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            builder.Include(originLatLng);
            builder.Include(destLatLng);
            LatLngBounds bounds = builder.Build();

            int padding = 100; // offset from edges of the map in pixels
            CameraUpdate cu = CameraUpdateFactory.NewLatLngBounds(bounds, padding);

            // Add markers to oriign and destination
            mMap.AddMarker(new MarkerOptions().SetPosition(originLatLng).SetTitle("Origin"));
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
            mMap.AnimateCamera(cu);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TripConfirmation_1);
            
            double originLat = Convert.ToDouble(Intent.GetStringExtra("originLat") ?? "Data not available");
            double originLng = Convert.ToDouble(Intent.GetStringExtra("originLng") ?? "Data not available");
            double destLat = Convert.ToDouble(Intent.GetStringExtra("destLat") ?? "Data not available");
            double destLng = Convert.ToDouble(Intent.GetStringExtra("destLng") ?? "Data not available");

            originLatLng = new LatLng(originLat, originLng);
            destLatLng = new LatLng(destLat, destLng);

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
            mapFragment.GetMapAsync(this);            
        }

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