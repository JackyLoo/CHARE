using Android.App;
using Android.Content;
using Android.Gms.Location.Places.UI;
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
using System.Net;
using System.Threading.Tasks;
using static Android.App.TimePickerDialog;

namespace CHARE_System
{
    [Activity(Label = "TripPassDetailsRow_Edit")]
    public class TripPassDetailsRow_Edit : Activity, IOnMapReadyCallback, IOnTimeSetListener
    {
        Context context;
        private ProgressDialog progress;                
        private TripDetails iTripDetail;        
        private LatLng originLatLng;
        private LatLng destLatLng;        
        private GoogleMap mMap;
        private const string strGoogleMatrixAPIOri = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=";
        private const string strGoogleMatrixAPIDest = "&destinations=";
        private const string strGoogleDirectionAPIOri = "https://maps.googleapis.com/maps/api/directions/json?origin=";
        private const string strGoogleDirectionAPIDest = "&destination=";
        private const string strGoogleApiKey = "&key=AIzaSyBxXCmp-C6i5LwwRSTuvzIjD9_roPjJ4EI";
        private TextView tvDriver;
        private TextView tvOrigin;
        private TextView tvDest;
        private TextView txtviewDistance;
        private TextView txtviewDuration;
        private TextView txtviewCost;        
        private TextView tvArriveTime;
        private TextView tvDay;       
        private Switch switchFemaleOnly;
        private Spinner spinnerSeat;        
        private Button btnUpdate;
        private ToggleButton tbtnMon;
        private ToggleButton tbtnTue;
        private ToggleButton tbtnWed;
        private ToggleButton tbtnThu;
        private ToggleButton tbtnFri;
        private ToggleButton tbtnSat;
        private ToggleButton tbtnSun;
        private Button btnDayConfirm;
        private bool[] arrCheckedDay;
        
        private int hour, minute, totalInSecond;
        private string strPickedDays;
        private TimeSpan onTimeSet;
        private const double dblPassengerCostKM = 0.0003;
        private const double dblDriverCostKM = 0.0010;
        PlaceAutocompleteFragment originAutocompleteFragment;
        PlaceAutocompleteFragment destAutocompleteFragment;

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

            SetContentView(Resource.Layout.TripDetailsRow_Edit);

            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            context = this;

            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading...");
            progress.SetCancelable(false);

            // Get intent item and deserialize into object
            iTripDetail = JsonConvert.DeserializeObject<TripDetails>(Intent.GetStringExtra("Trip"));
            var o = iTripDetail.originLatLng.Split(',');
            var d = iTripDetail.destinationLatLng.Split(',');
            originLatLng = new LatLng(Double.Parse(o[0]), Double.Parse(o[1]));
            destLatLng = new LatLng(Double.Parse(d[0]), Double.Parse(d[1]));

            arrCheckedDay = new bool[7];
            SetDayArrayBool(false);

            // Views Initialization  
            LinearLayout memberLayout = (LinearLayout)FindViewById(Resource.Id.member_layout);
            LinearLayout originLayout = (LinearLayout)FindViewById(Resource.Id.origin_layout);
            LinearLayout destLayout = (LinearLayout)FindViewById(Resource.Id.dest_layout);
            LinearLayout upperLayout = (LinearLayout)FindViewById(Resource.Id.upperlayout);
            LinearLayout lowerLayout = (LinearLayout)FindViewById(Resource.Id.lowerlayout_btn);
            LinearLayout upperContainer  = (LinearLayout)FindViewById(Resource.Id.upper_container);
            LinearLayout lowerContainer = (LinearLayout)FindViewById(Resource.Id.lower_container);
            LinearLayout seatLayout = (LinearLayout)FindViewById(Resource.Id.availableSeat_layout);           

            tvDriver = (TextView)FindViewById(Resource.Id.textview_member);
            tvOrigin = (TextView)FindViewById(Resource.Id.textview_origin);
            tvDest = (TextView)FindViewById(Resource.Id.textview_dest);
            txtviewDistance = (TextView)FindViewById(Resource.Id.textview_distance);
            txtviewDuration = (TextView)FindViewById(Resource.Id.textview_time);
            txtviewCost = (TextView)FindViewById(Resource.Id.textview_cost);
            tvArriveTime = (TextView)FindViewById(Resource.Id.textview_arrivetime);
            tvDay = (TextView)FindViewById(Resource.Id.textview_day);
            switchFemaleOnly = (Switch)FindViewById(Resource.Id.switch_femaleonly);            
            spinnerSeat = (Spinner)FindViewById(Resource.Id.spinner_seat);
            btnUpdate = (Button)FindViewById(Resource.Id.trip_pass_edit_edit_continue);

            // Initialize map fragment and initialize the map system
            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.trip_pass_edit_googlemap);
            mapFragment.GetMapAsync(this);

            // Initialize google autocomplete text view
            originAutocompleteFragment = (PlaceAutocompleteFragment)
                FragmentManager.FindFragmentById(Resource.Id.trip_pass_edit_edit_origin_fragment);            
            destAutocompleteFragment = (PlaceAutocompleteFragment)
                FragmentManager.FindFragmentById(Resource.Id.trip_pass_edit_edit_dest_fragment);

            // Convert trip detail span time format into HH:mm tt formant
            string[] time = iTripDetail.arriveTime.Split(':');
            totalInSecond = (int.Parse(time[0]) * 3600) + (int.Parse(time[1]) * 60);
            onTimeSet = TimeSpan.FromSeconds(totalInSecond);
            string strTime = DateTime.ParseExact(onTimeSet.ToString(@"hh\:mm"), "HH:mm", null).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US"));

            // Initialize for keeping track of the trip arrive time in time dialog
            hour = int.Parse(time[0]);
            minute = int.Parse(time[1]);

            // Initialize view value            
            txtviewCost.Text = iTripDetail.costStr;
            txtviewDuration.Text = iTripDetail.durationStr;
            txtviewDistance.Text = iTripDetail.distanceStr;
            tvArriveTime.Text = strTime;
            tvDay.Text = iTripDetail.days;
            if (iTripDetail.femaleOnly.Equals("Yes"))
                switchFemaleOnly.Checked = true;
            else
                switchFemaleOnly.Checked = false;

            seatLayout.Visibility = ViewStates.Gone;
            upperContainer.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 0, 5.8f);
            lowerContainer.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 0, 4.2f);

            // Add click events if intent has pass "Status" to here
            // It is used to validate if trip is editable
            if (Intent.HasExtra("Status"))
            {
                originAutocompleteFragment.SetHint("Enter the origin");
                originAutocompleteFragment.SetText(iTripDetail.origin);
                destAutocompleteFragment.SetHint("Enter the destination");
                destAutocompleteFragment.SetText(iTripDetail.destination);
                tvArriveTime.Click += ShowTimeDialog;
                tvDay.Click += ShowDayDialog;
                btnUpdate.Click += UpdateTripDetail;                
                originAutocompleteFragment.PlaceSelected += OnOriginSelected;                
                destAutocompleteFragment.PlaceSelected += OnDestinationSelected;                                
            }
            else
            {
                switchFemaleOnly.Enabled = false;
                // Hide button layout
                lowerLayout.Visibility = ViewStates.Gone;
                originAutocompleteFragment.View.Visibility = ViewStates.Gone;
                destAutocompleteFragment.View.Visibility = ViewStates.Gone;
                memberLayout.Visibility = ViewStates.Visible;
                originLayout.Visibility = ViewStates.Visible;                
                destLayout.Visibility = ViewStates.Visible;
                tvDriver.Text = iTripDetail.Requests[0].status;
                tvOrigin.Text = iTripDetail.origin;
                tvDest.Text = iTripDetail.destination;

                // Change layout weight 
                upperContainer.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, 0, 5.7f);
                lowerContainer.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, 0, 4.3f);
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
        
        private async void UpdateTripDetail(Object sender, EventArgs e)
        {
            Android.Net.ConnectivityManager cm = (Android.Net.ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            if (cm.ActiveNetworkInfo == null)
                Toast.MakeText(context, "Network error. Try again later.", ToastLength.Long).Show();
            {
                iTripDetail.originLatLng = originLatLng.Latitude.ToString() + "," + originLatLng.Longitude.ToString();
                iTripDetail.destinationLatLng = destLatLng.Latitude.ToString() + "," + destLatLng.Longitude.ToString();
                iTripDetail.arriveTime = onTimeSet.ToString();
                iTripDetail.days = tvDay.Text.ToString();

                string duration = txtviewDuration.Text.ToString();
                int totalInSecond;
                // If duration is more than 1 hour
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
                iTripDetail.duration = totalInSecond;
                iTripDetail.cost = Double.Parse(txtviewCost.Text.ToString().Substring(2, txtviewCost.Text.ToString().Length - 2),
                System.Globalization.CultureInfo.InvariantCulture);

                iTripDetail.distanceStr = txtviewDistance.Text.ToString();
                iTripDetail.durationStr = txtviewDuration.Text.ToString();
                iTripDetail.costStr = txtviewCost.Text.ToString();
                if (switchFemaleOnly.Checked)
                    iTripDetail.femaleOnly = "Yes";
                else
                    iTripDetail.femaleOnly = "No";
                RunOnUiThread(() =>
                {
                    progress.Show();
                });
                await RESTClient.UpdateTripPassengerAsync(context, iTripDetail);
                RunOnUiThread(() =>
                {
                    progress.Dismiss();
                });
                Intent intent = new Intent(context, typeof(TripPassListViewActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                this.StartActivity(intent);
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

        private void OnDestinationSelected(object sender, PlaceSelectedEventArgs e)
        {
            // Set destination latlng to iDestLatLng.
            destLatLng = e.Place.LatLng;
            iTripDetail.destination = e.Place.NameFormatted.ToString();
            LoadMap(true);            
        }

        private void OnOriginSelected(object sender, PlaceSelectedEventArgs e)
        {
            originLatLng = e.Place.LatLng;
            iTripDetail.origin = e.Place.NameFormatted.ToString();
            LoadMap(true);
        }

        private async void LoadTripInformation()
        {
            // Calculate distance and duration
            string urlGoogleMatrix = strGoogleMatrixAPIOri + iTripDetail.origin +
                                        strGoogleMatrixAPIDest + iTripDetail.destination + strGoogleApiKey;

            string strGoogleMatrix = await MapHelper.DownloadStringAsync(urlGoogleMatrix);

            var googleDirectionMatrix = JsonConvert.DeserializeObject<GoogleDistanceMatrixAPI>(strGoogleMatrix);

            double cost;
            cost = Math.Round(dblPassengerCostKM * googleDirectionMatrix.rows[0].elements[0].distance.value, 2);

            txtviewDistance.Text = googleDirectionMatrix.rows[0].elements[0].distance.text.ToString();
            txtviewDuration.Text = googleDirectionMatrix.rows[0].elements[0].duration.text.ToString();
            txtviewCost.Text = string.Format("RM{0:0.00}", cost);            
        }

        private async void LoadMap(bool loadTripInfo)
        {
            mMap.Clear();

            RunOnUiThread(() =>
            {
                progress.Show();
            });
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            builder.Include(originLatLng);
            builder.Include(destLatLng);
            LatLngBounds bounds = builder.Build();
            // Off set the map at edges of 100p
            int padding = 100; 
            CameraUpdate cu = CameraUpdateFactory.NewLatLngBounds(bounds, padding);

            // Add markers to oriign and destination
            mMap.AddMarker(new MarkerOptions().SetPosition(originLatLng).SetTitle("Origin"));
            mMap.AddMarker(new MarkerOptions().SetPosition(destLatLng).SetTitle("Destination"));

            // Combine Google Direction API string 
            string urlGoogleDirection = strGoogleDirectionAPIOri + iTripDetail.origin +
                strGoogleDirectionAPIDest + iTripDetail.destination + strGoogleApiKey;

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

            if (loadTripInfo)
                LoadTripInformation();
            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });
            Console.WriteLine("===== LoadMap END");
        }

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }     

        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;
            LoadMap(false);
        }
    }
}