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
using Android.Gms.Maps;
using Android.Gms.Common;

namespace CHARE_System
{
    class MapFragmentActivity : Fragment, IOnMapReadyCallback
    {
        MapView mapView;
        GoogleMap map;

        public View OnCreateView(LayoutInflater inflater, ViewGroup cOntainer, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.MapViewFragment, cOntainer, false);

            // Gets the MapView from the XML layout and creates it
            mapView = (MapView)v.FindViewById(Resource.Id.mapview);
            mapView.OnCreate(savedInstanceState);

            // Gets to GoogleMap from the MapView and does initializatiOn stuff
            mapView.GetMapAsync(this);

            // Needs to call MapsInitializer before doing any CameraUpdateFactory calls
            try
            {
                MapsInitializer.Initialize(Activity);
            }
            catch (GooglePlayServicesNotAvailableException e)
            {
                Console.WriteLine("==== Error in GooglePlayServicesNotAvailableExceptiOn");
                Console.WriteLine("==== Error: " + e.ToString());
            }


            return v;
        }


        public override void OnResume()
        {
            mapView.OnResume();
            base.OnResume();
        }


        public override void OnPause()
        {
            base.OnPause();
            mapView.OnPause();
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            mapView.OnDestroy();
        }


        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView.OnLowMemory();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.MyLocationEnabled = true;
        }
    }
}