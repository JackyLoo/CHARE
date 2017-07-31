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
using Android.Gms.Maps.Model;

namespace CHARE_System
{
    [Activity(Label = "TripConfirmation_2")]
    public class TripConfirmation_2 : Activity
    {
        private LatLng originLatLng;
        private LatLng destLatLng;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TripConfirmation_2);

            double originLat = Convert.ToDouble(Intent.GetStringExtra("originLat") ?? "Data not available");
            double originLng = Convert.ToDouble(Intent.GetStringExtra("originLng") ?? "Data not available");
            double destLat = Convert.ToDouble(Intent.GetStringExtra("destLat") ?? "Data not available");
            double destLng = Convert.ToDouble(Intent.GetStringExtra("destLng") ?? "Data not available");

            originLatLng = new LatLng(originLat, originLng);
            destLatLng = new LatLng(destLat, destLng);
        }
    }
}