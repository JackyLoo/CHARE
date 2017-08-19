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
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CHARE_REST_API.JSON_Object;

namespace CHARE_System.Class
{
    class RESTClient
    {
        private static HttpClient client;        
        public RESTClient()
        {
            
        }
        static RESTClient()
        {                        
            client = new HttpClient();
            client.BaseAddress = new Uri("http://charerestapi.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public static HttpClient GetClient()
        {
            return client;
        }
        //TripDriver
        public static async Task<string> SearchTripDriversAsync(Context c, int tripPassengerID)
        {
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripDrivers?id=" + tripPassengerID + "&passengerTrip=null");
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }
            else
            {
                Toast.MakeText(c, "Unable to search drivers.", ToastLength.Short).Show();
            }
            return make;
        }

        public static async Task<string> GetTripDriverAsync(Context c, int id)
        {
            Console.WriteLine("===== GetTripsAsync Start");
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripDrivers?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }
            else
            {
                Toast.MakeText(c, "Failed to load trips.", ToastLength.Short).Show();
            }
            Console.WriteLine("===== Test Load : "+ make.ToString());
            Console.WriteLine("===== GetTripsAsync End");
            return make;
        }

        //TripPassenger 
        public static async Task<string> GetTripPassengerAsync(int id)
        {
            Console.WriteLine("===== GetTripsAsync Start");
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripPassengers?id="+id);
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }
            Console.WriteLine("===== GetTripsAsync End");
            return make;
        }
        // Request
        public static async Task DeleteRequestAsync(Context c, Request request)
        {
            HttpResponseMessage response = await client.DeleteAsync("api/Requests?id=" + request.RequestID);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Cancelled request.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to cancel request.", ToastLength.Short).Show();
        }

        public static async Task CancelRequestAsync(Context c, Request request)
        {         
            HttpResponseMessage response = await client.PutAsJsonAsync("api/Requests?id="+request.RequestID, request);                     
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Cancelled request.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to cancel request.", ToastLength.Short).Show();
        }

        public static async Task CreateRequestAsync(Context c, Request request)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Requests", request);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Request sent.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to send request.", ToastLength.Short).Show();
        }
    }
}