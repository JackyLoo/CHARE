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
using CHARE_REST_API.JSON_Object;

namespace CHARE_System.JSON_Object
{
    class RatingDetails
    {   
        public Member Member1 { get; set; }   
        public Member Member { get; set; }        
        public int RateID { get; set; }
        public int MemberID { get; set; }
        public int RaterID { get; set; }
        public string comment { get; set; }
        public int rate { get; set; }
    }
}