using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.JSON_Object;
using CHARE_System.Class;
using CHARE_System.JSON_Object;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Android.App.TimePickerDialog;

namespace CHARE_System
{
    [Activity(Label = "Route Details")]
    public class TripConfirmation_1 : Activity, IOnMapReadyCallback, IOnTimeSetListener
    {
        private ProgressDialog progress;
        private Member iMember;
        // Intent Putextra Data
        private double distanceValue;
        private int durationValue;
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
        private Button btnCreate;        
        private const double dblPassengerCostKM = 0.0003;
        private const double dblDriverCostKM = 0.0010;

        // Lower        
        private TextView tvArriveTime;
        private TextView tvDay;
        private Switch switchFemaleOnly;                
        private ToggleButton tbtnMon;
        private ToggleButton tbtnTue;
        private ToggleButton tbtnWed;
        private ToggleButton tbtnThu;
        private ToggleButton tbtnFri;
        private ToggleButton tbtnSat;
        private ToggleButton tbtnSun;
        private Button btnDayConfirm;
        private bool[] arrCheckedDay;
        private Spinner spinnerSeat;
        private int hour, minute;
        private TimeSpan onTimeSet;
        private string strPickedDays = "";        

        struct CustomPair
        {
            public int key;
            public string value;

            public CustomPair(int key, string value) : this()
            {
                this.key = key;
                this.value = value;
            }
        }

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

            // Lower Part
            arrCheckedDay = new bool[7];
            SetDayArrayBool(false);

            LinearLayout upperContainer = (LinearLayout)FindViewById(Resource.Id.upper_container);
            LinearLayout lowerContainer = (LinearLayout)FindViewById(Resource.Id.lower_container);
            LinearLayout seatLayout = (LinearLayout)FindViewById(Resource.Id.availableSeat_layout);

            tvArriveTime = (TextView)FindViewById(Resource.Id.textview_arrivetime);
            tvDay = (TextView)FindViewById(Resource.Id.textview_day);
            switchFemaleOnly = (Switch)FindViewById(Resource.Id.switch_femaleonly);            
            spinnerSeat = (Spinner)FindViewById(Resource.Id.spinner_seat);            
            tvArriveTime.Click += ShowTimeDialog;
            tvDay.Click += ShowDayDialog;

            if (iMember.type.Equals("Driver"))
            {
                // Upper Part
                // Set available seat string array to adapter
                var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.trip_available_seat,
                    Resource.Layout.Custom_Spinner_Seat);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinnerSeat.Adapter = adapter;
            }
            else
            {
                seatLayout.Visibility = ViewStates.Gone;
                upperContainer.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 0, 6f);
                lowerContainer.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 0, 4f);
            }

            Android.Icu.Util.Calendar mcurrentTime = Android.Icu.Util.Calendar.Instance;
            hour = mcurrentTime.Get(Android.Icu.Util.Calendar.HourOfDay);
            minute = mcurrentTime.Get(Android.Icu.Util.Calendar.Minute);

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.googlemap);
            mapFragment.GetMapAsync(this);

            txtviewDistance = (TextView)FindViewById(Resource.Id.textview_distance);
            txtviewDuration = (TextView)FindViewById(Resource.Id.textview_time);
            txtviewCost = (TextView)FindViewById(Resource.Id.textview_cost);
            btnCreate = (Button)FindViewById(Resource.Id.btn_tripcon_continue);
            btnCreate.Click += CreateClick;
        }

        private bool ValidateData()
        {
            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
            {
                Toast.MakeText(this, "Network error. Try again later.", ToastLength.Long).Show();
                return false;
            }
            else if (tvArriveTime.Text.ToString().Equals("Set time"))
            {
                Toast.MakeText(this, "Set arrive time", ToastLength.Long).Show();
                return false;
            }
            else if (tvDay.Text.ToString().Equals("Set days") || tvDay.Text.ToString().Trim() == "")
            {
                Toast.MakeText(this, "Select day", ToastLength.Long).Show();
                return false;
            }
            else
                return true;            
        }

        private async void CreateClick(Object sender, EventArgs e)
        {            
            if(ValidateData())
            {
                iTrip.distance = distanceValue;
                iTrip.duration = durationValue;
                iTrip.cost = Double.Parse(txtviewCost.Text.ToString().Substring(2, txtviewCost.Text.ToString().Length - 2),
                System.Globalization.CultureInfo.InvariantCulture);
                iTrip.distanceStr = txtviewDistance.Text.ToString();
                iTrip.durationStr = txtviewDuration.Text.ToString();
                iTrip.costStr = txtviewCost.Text.ToString();
                // Assign user input and request POST to REST API            
                iTrip.arriveTime = onTimeSet.ToString();
                iTrip.days = tvDay.Text.ToString();

                if (switchFemaleOnly.Checked)
                    iTrip.femaleOnly = "Yes";
                else
                    iTrip.femaleOnly = "No";

                TripDriver tripDriver;
                TripPassenger tripPassenger;

                progress.SetMessage("Creating trip...");
                RunOnUiThread(() =>
                {
                    progress.Show();
                });
                if (iMember.type.Equals("Driver"))
                {
                    tripDriver = new TripDriver(iTrip);
                    tripDriver.DriverID = iMember.MemberID;
                    tripDriver.availableSeat = int.Parse(spinnerSeat.SelectedItem.ToString());
                    var json = JsonConvert.SerializeObject(tripDriver);                    
                    await RESTClient.CreateTripDriverAsync(this, tripDriver);
                }
                else
                {
                    tripPassenger = new TripPassenger(iTrip);
                    tripPassenger.PassengerID = iMember.MemberID;
                    var json = JsonConvert.SerializeObject(tripPassenger);                   
                    await RESTClient.CreateTripPassengerAsync(this, tripPassenger);
                }
                RunOnUiThread(() =>
                {
                    progress.Dismiss();
                });
                Intent intent = new Intent(this, typeof(MainActivity));                
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                StartActivity(intent);
                Finish();
            }
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            // Set global hour and minute to selected time for tracking
            hour = hourOfDay;
            this.minute = minute;

            // Convert time to second
            int totalInSecond = (hourOfDay * 3600) + (minute * 60);
            // Create timespan from the total second
            onTimeSet = TimeSpan.FromSeconds(totalInSecond);

            // Parse timespan to the time formate of e.g 12:00 AM
            string strTime = DateTime.ParseExact(onTimeSet.ToString(@"hh\:mm"), "HH:mm", null).ToString("hh:mm tt",
                CultureInfo.GetCultureInfo("en-US"));

            tvArriveTime.Text = strTime;
        }

        private void SetDayArrayBool(bool b)
        {
            for (int i = 0; i < arrCheckedDay.Length; i++)
            {
                arrCheckedDay[i] = b;
            }
        }

        private void ShowTimeDialog(Object sender, EventArgs e)
        {
            TimePickerDialog mTimePicker;
            mTimePicker = new TimePickerDialog(this, Android.Resource.Style.ThemeHoloLightDialog, this,
                hour, minute, false);
            mTimePicker.SetTitle("Select Time");
            mTimePicker.Show();
        }

        private void ShowDayDialog(Object sender, EventArgs e)
        {
            Dialog dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.DayPickerDialog);
            dialog.SetTitle("Set Day");

            tbtnMon = (ToggleButton)dialog.FindViewById(Resource.Id.togglebtn_mon);
            tbtnTue = (ToggleButton)dialog.FindViewById(Resource.Id.togglebtn_tue);
            tbtnWed = (ToggleButton)dialog.FindViewById(Resource.Id.togglebtn_wed);
            tbtnThu = (ToggleButton)dialog.FindViewById(Resource.Id.togglebtn_thu);
            tbtnFri = (ToggleButton)dialog.FindViewById(Resource.Id.togglebtn_fri);
            tbtnSat = (ToggleButton)dialog.FindViewById(Resource.Id.togglebtn_sat);
            tbtnSun = (ToggleButton)dialog.FindViewById(Resource.Id.togglebtn_sun);

            var tbtnList = new List<ToggleButton>
            {
                tbtnMon,
                tbtnTue,
                tbtnWed,
                tbtnThu,
                tbtnFri,
                tbtnSat,
                tbtnSun
            };

            for (int i = 0; i < tbtnList.Count; i++)
            {
                if (arrCheckedDay[i])
                    tbtnList[i].Checked = true;
            }

            btnDayConfirm = (Button)dialog.FindViewById(Resource.Id.btn_tripcon_day_confirm);
            btnDayConfirm.Click += (sender2, e2) =>
            {
                var listDay = new List<KeyValuePair<bool, string>>();
                List<CustomPair> listDays = new List<CustomPair>();
                SetDayArrayBool(false);

                strPickedDays = "";

                if (tbtnMon.Checked)
                {
                    listDays.Add(new CustomPair(1, "Mon"));
                    arrCheckedDay[0] = true;
                }
                if (tbtnTue.Checked)
                {
                    listDays.Add(new CustomPair(2, "Tue"));
                    arrCheckedDay[1] = true;
                }
                if (tbtnWed.Checked)
                {
                    listDays.Add(new CustomPair(3, "Wed"));
                    arrCheckedDay[2] = true;
                }
                if (tbtnThu.Checked)
                {
                    listDays.Add(new CustomPair(4, "Thu"));
                    arrCheckedDay[3] = true;
                }
                if (tbtnFri.Checked)
                {
                    listDays.Add(new CustomPair(5, "Fri"));
                    arrCheckedDay[4] = true;
                }
                if (tbtnSat.Checked)
                {
                    listDays.Add(new CustomPair(6, "Sat"));
                    arrCheckedDay[5] = true;
                }
                if (tbtnSun.Checked)
                {
                    listDays.Add(new CustomPair(7, "Sun"));
                    arrCheckedDay[6] = true;
                }

                for (int i = 0; i < listDays.Count(); i++)
                {
                    strPickedDays += listDays[i].value;
                    if (i != (listDays.Count() - 1))
                        strPickedDays += ", ";
                }

                if (strPickedDays.Equals(""))
                    Toast.MakeText(this, "Day(s) is not selected.", ToastLength.Short).Show();
                else
                {
                    tvDay.Text = strPickedDays;
                    dialog.Dismiss();
                }
            };
            dialog.Show();
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
            string urlGoogleDirection = MapHelper.GoogleDirectionAPIAddress(iTrip.originLatLng, iTrip.destinationLatLng);
            string strGoogleDirection = await MapHelper.DownloadStringAsync(urlGoogleDirection);

            var googleDirectionAPIRoute = JsonConvert.DeserializeObject<GoogleDirectionAPI>(strGoogleDirection);
            string encodedPoints = googleDirectionAPIRoute.routes[0].overview_polyline.points;
            var lstDecodedPoints = MapHelper.DecodePolylinePoint(encodedPoints);
            //convert list of location point to array of latlng type

            var latLngPoints = lstDecodedPoints.ToArray();
            var polylineoption = new PolylineOptions();
            polylineoption.InvokeColor(Android.Graphics.Color.SkyBlue);
            polylineoption.Geodesic(true);
            polylineoption.Add(latLngPoints);
            mMap.AddPolyline(polylineoption);
            mMap.AnimateCamera(cu);

            // Call google matrix api 
            string urlGoogleMatrix = MapHelper.GoogleDistanceMatrixAPIAddress(iTrip.originLatLng, iTrip.destinationLatLng);
            string strGoogleMatrix = await MapHelper.DownloadStringAsync(urlGoogleMatrix);            
            var googleDirectionMatrix = JsonConvert.DeserializeObject<GoogleDistanceMatrixAPI>(strGoogleMatrix);
            txtviewDistance.Text = googleDirectionMatrix.rows[0].elements[0].distance.text.ToString();
            txtviewDuration.Text = googleDirectionMatrix.rows[0].elements[0].duration.text.ToString();
            distanceValue = double.Parse(googleDirectionMatrix.rows[0].elements[0].distance.value.ToString());
            durationValue = int.Parse(googleDirectionMatrix.rows[0].elements[0].duration.value.ToString());

            double cost;
            if (iMember.type.Equals("Driver"))
                cost = Math.Round(dblDriverCostKM * googleDirectionMatrix.rows[0].elements[0].distance.value, 2);
            else
                cost = Math.Round(dblPassengerCostKM * googleDirectionMatrix.rows[0].elements[0].distance.value, 2);
            txtviewCost.Text = string.Format("RM{0:0.00}", cost);

            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }
    }
}