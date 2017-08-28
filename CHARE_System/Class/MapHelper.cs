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
using System.Threading.Tasks;
using System.Net;
using Android.Gms.Maps.Model;
using CHARE_REST_API.JSON_Object;

namespace CHARE_System.Class
{
    class MapHelper
    {
        private const string _GoogleDistanceMatrixAPIAddress = "https://maps.googleapis.com/maps/api/distancematrix/json";                
        private const string _GoogleDirectionAPIAddress = "https://maps.googleapis.com/maps/api/directions/json";
        private const string _GoogleAPIKey = "AIzaSyBxXCmp-C6i5LwwRSTuvzIjD9_roPjJ4EI";

            //?origin=3.2718236,101.6489234&destination=3.1161034,101.6392469&waypoints=optimize:true|3.209876,101.659176|3.302183,101.598181&key=AIzaSyBxXCmp-C6i5LwwRSTuvzIjD9_roPjJ4EI";";
        public static List<LatLng> DecodePolylinePoint(string encodedPoints)
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

        public static async Task<string>  DownloadStringAsync(string strUri)
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
            }
            finally
            {
                webclient.Dispose();
                webclient = null;
            }

            return strResultData;
        }

        public static async Task<string> DownloadStringAsync(TripDetails trips)
        {
            string waypoints = "";

            foreach (TripPassenger tp in trips.TripPassengers)
            {
                waypoints += "|"+tp.originLatLng;
            }
            
            string strUri = _GoogleDirectionAPIAddress + "?origin=" + trips.originLatLng + "&destination=" + trips.destinationLatLng + 
                "&waypoints=optimize:true" + waypoints + "&key=" + _GoogleAPIKey;
            Console.WriteLine("===== strUri " + strUri);
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
            }
            finally
            {
                webclient.Dispose();
                webclient = null;
            }

            return strResultData;
        }

        public static string GoogleDirectionAPIAddress(string originLatLng, string destinationLatLng)
        {
            return _GoogleDirectionAPIAddress + "?origin=" + originLatLng + "&destination=" 
                 + destinationLatLng + "&key=" + _GoogleAPIKey;
        }

        public static string GoogleDistanceMatrixAPIAddress(string originLatLng, string destinationLatLng)
        {       
            return _GoogleDistanceMatrixAPIAddress + "?origins=" + originLatLng + "&destinations="
                 + destinationLatLng + "&key=" + _GoogleAPIKey;
        }
    }
}