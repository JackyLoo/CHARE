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
using Android.Gms.Maps;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using CHARE_System.JSON_Object;
using static CHARE_System.JSON_Object.GoogleDistanceMatrix;
using CHARE_REST_API.JSON_Object;

namespace CHARE_System
{
    [Activity(Label = "Route Details")]
    public class TripConfirmation_1 : Activity, IOnMapReadyCallback
    {
        private ProgressDialog progress;

        private Member iMember;

        // Intent Putextra Data
        private Trip iTrip;

        private LatLng originLatLng;
        private LatLng destLatLng;

        private GoogleMap mMap;        

        private const string strGoogleMatrixAPIOri = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=";
        private const string strGoogleMatrixAPIDest = "&destinations=";
        private const string strGoogleDirectionAPIOri = "https://maps.googleapis.com/maps/api/directions/json?origin=";
        private const string strGoogleDirectionAPIDest = "&destination=";        
        private const string strGoogleApiKey = "&key=AIzaSyBxXCmp-C6i5LwwRSTuvzIjD9_roPjJ4EI";

        private TextView txtviewDistance;
        private TextView txtviewDuration;
        private TextView txtviewCost;
        private Button btnContinue;
        
        private const double dblPassengerCostKM = 0.0003;
        private const double dblDriverCostKM = 0.0010;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefault);
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.TripConfirmation_1);

            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading...");
            progress.SetCancelable(false);

            RunOnUiThread(() =>
            {
                progress.Show();
            });

            iMember = JsonConvert.DeserializeObject<Member>(Intent.GetStringExtra("Member"));

            iTrip = JsonConvert.DeserializeObject<Trip>(Intent.GetStringExtra("Trip"));
            var o = iTrip.originLatLng.Split(',');
            var d = iTrip.destinationLatLng.Split(',');
           
            originLatLng =  new LatLng(Double.Parse(o[0]), Double.Parse(o[1]));
            destLatLng = new LatLng(Double.Parse(d[0]), Double.Parse(d[1]));

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
            mapFragment.GetMapAsync(this);

            txtviewDistance = (TextView)FindViewById(Resource.Id.textview_distance);
            txtviewDuration = (TextView)FindViewById(Resource.Id.textview_time);
            txtviewCost = (TextView)FindViewById(Resource.Id.textview_cost);

            btnContinue = (Button)FindViewById(Resource.Id.btn_tripcon_continue);
            btnContinue.Click += (sender, e) =>
            {
                // D = Digit
                // Parse string "DD.D km" to int "DD"                
                iTrip.distance = (int)Math.Round(Double.Parse(txtviewDistance.Text.ToString().
                    Substring(0, txtviewDistance.Text.ToString().Length - 3), System.Globalization.CultureInfo.InvariantCulture), 0);
                
                // Convert text duration to total in second
                string duration = txtviewDuration.Text.ToString();
                int totalInSecond;
                // If duration is more than 6
                if (duration.Length > 8)
                {
                    int hour = int.Parse(duration.Substring(0, duration.Length - 14));
                    int min = int.Parse(duration.Substring(duration.Length - 7, 2).Trim());
                    totalInSecond = (hour * 3600) + (min * 60);                    
                }
                else
                {
                    int min = int.Parse(duration.Substring(0, duration.Length - 5).Trim());
                    totalInSecond = min * 60;
                }

                iTrip.duration = totalInSecond;                
                iTrip.cost = Double.Parse(txtviewCost.Text.ToString().Substring(2, txtviewCost.Text.ToString().Length - 2),
                System.Globalization.CultureInfo.InvariantCulture);                

                iTrip.distanceStr = txtviewDistance.Text.ToString();
                iTrip.durationStr = txtviewDuration.Text.ToString();
                iTrip.costStr = txtviewCost.Text.ToString();                

                Intent intent = new Intent(this, typeof(TripConfirmation_2));
                intent.PutExtra("Member", Intent.GetStringExtra("Member"));
                intent.PutExtra("Trip", JsonConvert.SerializeObject(iTrip));                
                StartActivity(intent);
            };
        }

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
            string urlGoogleDirection = strGoogleDirectionAPIOri + iTrip.origin +
                strGoogleDirectionAPIDest + iTrip.destination + strGoogleApiKey;

            string strGoogleDirection = await fnDownloadString(urlGoogleDirection);

            var googleDirectionAPIRoute = JsonConvert.DeserializeObject<GoogleDirectionAPI>(strGoogleDirection);
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

            string urlGoogleMatrix = strGoogleMatrixAPIOri + iTrip.origin +
                                        strGoogleMatrixAPIDest + iTrip.destination + strGoogleApiKey;
            
            string strGoogleMatrix = await fnDownloadString(urlGoogleMatrix);
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });

            var googleDirectionMatrix = JsonConvert.DeserializeObject<GoogleDistanceMatrixAPI>(strGoogleMatrix);

            txtviewDistance.Text = googleDirectionMatrix.rows[0].elements[0].distance.text.ToString();
            txtviewDuration.Text = googleDirectionMatrix.rows[0].elements[0].duration.text.ToString();

            double cost;
            if (iMember.type.Equals("Driver"))
                cost = Math.Round(dblDriverCostKM * googleDirectionMatrix.rows[0].elements[0].distance.value, 2);
            else
                cost = Math.Round(dblPassengerCostKM * googleDirectionMatrix.rows[0].elements[0].distance.value, 2);

            txtviewCost.Text = string.Format("RM{0:0.00}", cost);
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
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