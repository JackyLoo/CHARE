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

namespace CHARE_REST_API.JSON_Object
{
    class TripDetails
    {
        public List<Request> Requests { get; set; }

        public Member Member { get; set; }
        public TripDriver TripDriver { get; set; }
        public int TripPassengerID { get; set; }
        public int PassengerID { get; set; }
        public int? TripDriverID { get; set; }
        
        public int DriverID { get; set; }
        public string PassengerIDs { get; set; }
        public int availableSeat { get; set; }

        public string origin { get; set; }
        public string destination { get; set; }
        public string originLatLng { get; set; }
        public string destinationLatLng { get; set; }
        public string arriveTime { get; set; }
        public string femaleOnly { get; set; }
        public double cost { get; set; }
        public double distance { get; set; }
        public string days { get; set; }
        public int duration { get; set; }
        public string costStr { get; set; }
        public string durationStr { get; set; }
        public string distanceStr { get; set; }
  
    }
}