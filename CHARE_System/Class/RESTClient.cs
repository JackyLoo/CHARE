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
using Newtonsoft.Json;

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

        // Carmodel
        public static async Task<string[]> GetCarmodelAsync(Context c)
        {
            var make = "";
            string[] list = null;

            HttpResponseMessage response = await client.GetAsync("api/CarModels");
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
                list = (make.Substring(1, make.Length - 2)).Replace('\"', ' ').Split(',');
            }
            else
                Toast.MakeText(c, "Failed to load carmodel make data.", ToastLength.Short).Show();
            
            return list;
        }

        public static async Task<string[]> GetCarmodelMakeAsync(Context c, string model)
        {
            var make = "";
            string[] list = null;
            HttpResponseMessage response = await client.GetAsync("api/CarModels?make=" + model); 
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
                list = (make.Substring(1, make.Length - 2)).Replace('\"', ' ').Split(',');
            }
            else
                Toast.MakeText(c, "Failed to load carmodel make data.", ToastLength.Short).Show();

            
            return list;
        }


        // Member
        public static async Task UpdateMemberAsync(Context c, Member member)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("api/Members?id=" +
                member.MemberID, member);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Details updated.", ToastLength.Short).Show();            
            else
                Toast.MakeText(c, "Error occur when updating member details.", ToastLength.Short).Show();            
        }

        public static async Task UpdateMemberVehicleAsync(Context c, Member member)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("api/Members?id=" +
                member.MemberID+"&vehicle=true", member);
            if (response.IsSuccessStatusCode)
            {
                await UpdateVehicleAsync(c, member.Vehicles[0]);
                Toast.MakeText(c, "Details updated.", ToastLength.Short).Show();
            }
            else
                Toast.MakeText(c, "Error occur when updating member details.", ToastLength.Short).Show();
        }

        public static async Task CreateMemberAsync(Context c, Member member)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Members", member);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Member registered successfully.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to register account.", ToastLength.Short).Show();
        }

        public static async Task CreateMemberVehicleAsync(Context c, Member member, Vehicle vehicle)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Members", member);            
            if (response.IsSuccessStatusCode)
            {                
                Toast.MakeText(c, "Member registered successfully.", ToastLength.Short).Show();
                var url = response.Headers.Location;
                Member m = null;
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    m = await response.Content.ReadAsAsync<Member>();
                }
                vehicle.MemberID = m.MemberID;
                await CreateVehicleAsync(c, vehicle);
            }
            else
                Toast.MakeText(c, "Failed to register account.", ToastLength.Short).Show();            
        }

        // Vehicle
        public static async Task UpdateVehicleAsync(Context c, Vehicle vehicle)
        {
            Console.WriteLine("===== Vehicle S");
            Console.WriteLine("1 " + vehicle.MemberID);
            Console.WriteLine("2 " + vehicle.plateNo);
            Console.WriteLine("3 " + vehicle.model);
            Console.WriteLine("4 " + vehicle.make);
            Console.WriteLine("5 " + vehicle.color);            
            

            HttpResponseMessage response = await client.PutAsJsonAsync("api/Vehicles?id=" +
                vehicle.VehicleID, vehicle);
            Console.WriteLine("===== Code " + response.StatusCode);
            Console.WriteLine("===== Vehicle E");
            if (!response.IsSuccessStatusCode)
                Toast.MakeText(c, "Failed to update vehicle.", ToastLength.Short).Show();
        }

        public static async Task CreateVehicleAsync(Context c, Vehicle vehicle)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Vehicles", vehicle);
            if (!response.IsSuccessStatusCode)                           
                Toast.MakeText(c, "Failed to register vehicle.", ToastLength.Short).Show();
        }

        // Rating
        public static async Task<string> GetRateListAsync(Context c, string id)
        {            
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/Ratings?id=" + id + "&type=List");
            Console.WriteLine("===== Rate Test Load error1 : " + response.Content);
            Console.WriteLine("===== Rate Test Load error2 : " + response.StatusCode);
            Console.WriteLine("===== Rate Test Load error3 : " + response.RequestMessage);
            
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
                if (make == null)
                    Toast.MakeText(c, "There is no rating data", ToastLength.Short).Show();
            }
            else
                Toast.MakeText(c, "Failed to load rating data.", ToastLength.Short).Show();

            Console.WriteLine("===== Rate Test Load : " + make.ToString());
            Console.WriteLine("===== Rate GetTripsAsync End");
            return make;
        }

        public static async Task CreateRatingAsync(Context c, Rating rating)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Ratings", rating);
            Console.WriteLine("==== Rating Error :" + response.StatusCode);
            Console.WriteLine("==== Rating Error :" + response.RequestMessage);
            Console.WriteLine("==== Rating Error :" + response.Content);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Rating submitted successfully.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to submit rating.", ToastLength.Short).Show();
        }

        //TripDriver
        public static async Task CreateTripDriverAsync(Context c, TripDriver tripDriver)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/TripDrivers", tripDriver);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Trip successfully created.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to create driver's trip.", ToastLength.Short).Show();
        }

        public static async Task<string> SearchTripDriversAsync(Context c, int tripPassengerID)
        {
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripDrivers?id=" + tripPassengerID + "&type=Search");
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
                if(make==null)
                    Toast.MakeText(c, "No available drivers at the moment. Try again later.", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(c, "Unable to search drivers.", ToastLength.Short).Show();
            }            
            return make;
        }

        public static async Task<string> GetTripDriverAsync(int id)
        {
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripDrivers?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }
            return make;
        }

        public static async Task<string> GetTripDriverListAsync(Context c, int id)
        {
            Console.WriteLine("===== GetTripsAsync Start");
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripDrivers?id=" + id+"&type=List");
            
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
                if(make==null)
                    Toast.MakeText(c, "You haven't created a trip yet. Create one today and get yourself a carpool partner", ToastLength.Short).Show();
            }
            else            
                Toast.MakeText(c, "Failed to load trips.", ToastLength.Short).Show();
            
            Console.WriteLine("===== Test Load : "+ make.ToString());
            Console.WriteLine("===== GetTripsAsync End");
            return make;
        }

        public static async Task UpdateTripDriverAsync(TripDriver tripDriver)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("api/TripDrivers?id=" +
                tripDriver.TripDriverID, tripDriver);
            if (!response.IsSuccessStatusCode)
                Console.WriteLine("Error occur when updating trip driver");
        }

        public static async Task UpdateTripDriverAsync(Context c, TripDriver tripDriver)
        {            
            HttpResponseMessage response = await client.PutAsJsonAsync("api/TripDrivers?id=" +
                tripDriver.TripDriverID, tripDriver);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Updated trip details.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to update.", ToastLength.Short).Show();
        }

        public static async Task DisjoinAllPassengerAsync(Context c, TripDriver tripDriver)
        {
            string passengerIDs = tripDriver.PassengerIDs;
            tripDriver.PassengerIDs = string.Empty;
            HttpResponseMessage response = await client.PutAsJsonAsync("api/TripDrivers?id=" +
                tripDriver.TripDriverID+ "&tripPassengerID=" + passengerIDs, tripDriver);

            Console.WriteLine("==== Code " + response.Content);
            Console.WriteLine("==== Code2 " + response.RequestMessage);
            Console.WriteLine("==== Code3 " + response.StatusCode);
            Console.WriteLine("==== Code4 " + JsonConvert.SerializeObject(tripDriver).ToString());
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Disjoin all passengers.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to disjoin.", ToastLength.Short).Show();
        }

        public static async Task DeleteTripDriverAsync(Context c, int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("api/TripDrivers?id=" + id);
            Console.WriteLine("==== Code " + response.Content);
            Console.WriteLine("==== Code2 " + response.RequestMessage);
            Console.WriteLine("==== Code3 " + response.StatusCode);

            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Deleted trip.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to delete trip.", ToastLength.Short).Show();
        }

        //TripPassenger 
        public static async Task CreateTripPassengerAsync(Context c, TripPassenger tripPassenger)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/TripPassengers", tripPassenger);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Trip successfully created.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to create passenger's trip.", ToastLength.Short).Show();
        }

        public static async Task<string> GetTripPassengerAsync(int id)
        {            
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripPassengers?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
            }            
            return make;
        }
        
        public static async Task QuitCarpoolPassengerAsync(Context c, TripPassenger tripPassenger)
        {
            string tripDriverID = tripPassenger.TripDriverID.ToString();
            tripPassenger.TripDriverID = null;
            HttpResponseMessage response = await client.PutAsJsonAsync("api/TripPassengers?id=" +
                tripPassenger.TripPassengerID+ "&tripDriverID=" + tripDriverID, tripPassenger);
            Console.WriteLine("==== Code " + response.Content);
            Console.WriteLine("==== Code2 " + response.RequestMessage);
            Console.WriteLine("==== Code3 " + response.StatusCode);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Quit from the carpool.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to quit.", ToastLength.Short).Show();
        }

        public static async Task UpdateTripPassengerAsync(TripPassenger tripPassenger)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("api/TripPassengers?id=" +
                tripPassenger.TripPassengerID, tripPassenger);
            if (!response.IsSuccessStatusCode)
                Console.WriteLine("Error occur when updating trip driver");
        }

        public static async Task UpdateTripPassengerAsync(Context c, TripPassenger tripPassenger)
        {            
            HttpResponseMessage response = await client.PutAsJsonAsync("api/TripPassengers?id=" + 
                tripPassenger.TripPassengerID, tripPassenger);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Updated trip details.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to update.", ToastLength.Short).Show();            
        }

        public static async Task<string> GetTripPassengerListAsync(Context c, int id)
        {            
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/TripPassengers?id=" + id + "&type=List");

            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
                if(make==null)
                    Toast.MakeText(c, "You haven't created a trip yet. Create one today and get yourself a carpool partner", ToastLength.Short).Show();
            }
            else            
                Toast.MakeText(c, "Failed to load trips.", ToastLength.Short).Show();                                    
            return make;            
        }

        public static async Task DeleteTripPassengerAsync(Context c, int id)
        {            
            HttpResponseMessage response = await client.DeleteAsync("api/TripPassengers?id=" + id);
            Console.WriteLine("==== Code " + response.Content);
            Console.WriteLine("==== Code2 " + response.RequestMessage);
            Console.WriteLine("==== Code3 " + response.StatusCode);

            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Deleted trip.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to delete trip.", ToastLength.Short).Show();
        }

        // Request
        public static async Task<string> GetTripRequestListAsync(Context c, int id)
        {
            var make = "";
            HttpResponseMessage response = await client.GetAsync("api/Requests?id=" + id + "&type=List");

            if (response.IsSuccessStatusCode)
            {
                make = await response.Content.ReadAsStringAsync();
                if (make == null)
                    Toast.MakeText(c, "There is no request yet. Try again later.", ToastLength.Short).Show();
            }
            else
                Toast.MakeText(c, "Failed to load requests.", ToastLength.Short).Show();            
            return make;
        }        
       
        public static async Task AcceptRequestAsync(Context c, Request request)
        {
            if(request.TripDriver.PassengerIDs != null)
                request.TripDriver.PassengerIDs = request.TripDriver.PassengerIDs +","+request.TripPassenger.TripPassengerID.ToString();            
            else
                request.TripDriver.PassengerIDs = request.TripPassenger.TripPassengerID.ToString();
            request.TripPassenger.TripDriverID = request.TripDriver.TripDriverID;
            await UpdateTripDriverAsync(request.TripDriver);
            await UpdateTripPassengerAsync(request.TripPassenger);

            HttpResponseMessage response = await client.PutAsJsonAsync("api/Requests?id=" +
                request.RequestID, request);
            if (response.IsSuccessStatusCode)                          
                Toast.MakeText(c, "Request accepted.", ToastLength.Short).Show();            
            else
                Toast.MakeText(c, "Failed to update request.", ToastLength.Short).Show();
        }

        public static async Task RejectRequestAsync(Context c, Request request)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("api/Requests?id=" +
                request.RequestID, request);
            if (response.IsSuccessStatusCode)                   
               Toast.MakeText(c, "Request rejected.", ToastLength.Short).Show();                    
            else
                Toast.MakeText(c, "Failed to update request.", ToastLength.Short).Show();
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

        public static async Task DeleteRequestAsync(Context c, Request request)
        {
            HttpResponseMessage response = await client.DeleteAsync("api/Requests?id=" + request.RequestID);
            if (response.IsSuccessStatusCode)
                Toast.MakeText(c, "Cancelled request.", ToastLength.Short).Show();
            else
                Toast.MakeText(c, "Failed to cancel request.", ToastLength.Short).Show();
        }
    }
}