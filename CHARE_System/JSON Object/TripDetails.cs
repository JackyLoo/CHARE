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
using CHARE_REST_API.Models;

namespace CHARE_System.JSON_Object
{
    class TripDetails
    {               
        public Member Member { get; set; }
        public int TripID { get; set; }
        public int MemberID { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string originLatLng { get; set; }
        public string destinationLatLng { get; set; }
        public string arriveTime { get; set; }
        public string femaleOnly { get; set; }
        public double cost { get; set; }
        public int distance { get; set; }
        public string days { get; set; }
        public int duration { get; set; }
        public string costStr { get; set; }
        public string durationStr { get; set; }
        public string distanceStr { get; set; }
    }
}