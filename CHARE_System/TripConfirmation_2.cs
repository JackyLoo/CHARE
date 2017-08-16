using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using CHARE_REST_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using static Android.App.TimePickerDialog;

namespace CHARE_System
{
    [Activity(Label = "Set Route")]
    public class TripConfirmation_2 : FragmentActivity, 
        IOnTimeSetListener
    {
        private ProgressDialog progress;

        // Intent Putextra Data
        private Member iMember;
        private Trip iTrip;
        
        private LatLng originLatLng;
        private LatLng destLatLng;

        // Views        
        private TextView tvArriveTime;
        private TextView tvDay;        
        private Switch switchFemaleOnly;
        private LinearLayout seatLayout;
        private TextView tvSeat;
        private Button btnConfirm;

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
        private Button btnSeatConfirm;

        private int hour, minute;
        private TimeSpan onTimeSet ;
        private string strPickedDays = "";

        private HttpClient client;

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
            SetContentView(Resource.Layout.TripConfirmation_2);            

            ActionBar ab = ActionBar;
            ab.SetDisplayHomeAsUpEnabled(true);

            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading...");
            progress.SetCancelable(false);

            client = new HttpClient();
            client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            iMember = JsonConvert.DeserializeObject<Member>(Intent.GetStringExtra("Member"));
            iTrip = JsonConvert.DeserializeObject<Trip>(Intent.GetStringExtra("Trip"));
            
            var o = iTrip.originLatLng.Split(',');
            var d = iTrip.destinationLatLng.Split(',');

            originLatLng = new LatLng(Double.Parse(o[0]), Double.Parse(o[1]));
            destLatLng = new LatLng(Double.Parse(d[0]), Double.Parse(d[1]));

            arrCheckedDay = new bool[7];
            SetDayArrayBool(false);
            
            tvArriveTime = (TextView)FindViewById(Resource.Id.textview_arrivetime);
            tvDay = (TextView)FindViewById(Resource.Id.textview_day);
            switchFemaleOnly = (Switch)FindViewById(Resource.Id.switch_femaleonly);
            seatLayout = (LinearLayout)FindViewById(Resource.Id.trip_linearlyt_seat);
            spinnerSeat = (Spinner)FindViewById(Resource.Id.spinner_seat);            
            btnConfirm = (Button)FindViewById(Resource.Id.btn_tripcon_comfirm);

            // Set available seat string array to adapter
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.trip_available_seat,
                Resource.Layout.Spinner_Seat);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerSeat.Adapter = adapter;

            Android.Icu.Util.Calendar mcurrentTime = Android.Icu.Util.Calendar.Instance;
            hour = mcurrentTime.Get(Android.Icu.Util.Calendar.HourOfDay);
            minute = mcurrentTime.Get(Android.Icu.Util.Calendar.Minute);

            tvArriveTime.Click += (sender, e) =>
            {                                
                TimePickerDialog mTimePicker;
                mTimePicker = new TimePickerDialog(this, Android.Resource.Style.ThemeHoloLightDialog, this,
                    hour, minute, false);
                mTimePicker.SetTitle("Select Time");
                mTimePicker.Show();
            };

            tvDay.Click += (sender, e) =>
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
                            strPickedDays += ",";
                    }
                    tvDay.Text = strPickedDays;
                    dialog.Dismiss();
                };

                dialog.Show();
            };

            if (iMember.type.Equals("Passenger"))
                seatLayout.Visibility = ViewStates.Invisible;

            btnConfirm.Click += BtnConfirm_Click;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            // Set global hour and minute to selected time for tracking
            this.hour = hourOfDay;
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

        override
        public bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }

        private void BtnConfirm_Click(Object sender, EventArgs e)
        {            
            // Assign user input and request POST to REST API            
            iTrip.arriveTime = onTimeSet.ToString();
            iTrip.days = tvDay.Text.ToString();

            if (switchFemaleOnly.Checked)
                iTrip.femaleOnly = "Yes";
            else
                iTrip.femaleOnly = "No";

            if (tvArriveTime.Text.ToString().Equals("Set time"))
                Toast.MakeText(this, "Set arrive time", ToastLength.Long).Show();
            else if (tvDay.Text.ToString().Equals("Set days") || tvDay.Text.ToString().Trim() == "")
                Toast.MakeText(this, "Select day", ToastLength.Long).Show();
            else
            {
                TripDriver tripDriver;
                TripPassenger tripPassenger;

                if (iMember.type.Equals("Driver"))
                {
                    tripDriver = new TripDriver(iTrip);
                    tripDriver.DriverID = iMember.MemberID;
                    tripDriver.availableSeat = int.Parse(spinnerSeat.SelectedItem.ToString());
                    var json = JsonConvert.SerializeObject(tripDriver);
                    Console.WriteLine("========================= 1 =========================");
                    Console.WriteLine(json.ToString());
                    Console.WriteLine("========================= 2 =========================");
                    CreateTripDriverAsync(tripDriver);
                }
                else
                {
                    tripPassenger = new TripPassenger(iTrip);
                    tripPassenger.PassengerID = iMember.MemberID;
                    var json = JsonConvert.SerializeObject(tripPassenger);
                    Console.WriteLine("========================= 1 =========================");
                    Console.WriteLine(json.ToString());
                    Console.WriteLine("========================= 2 =========================");
                    CreateTripPassAsync(tripPassenger);
                }               
            }
        }

        private void SetDayArrayBool(bool b)
        {
            for (int i = 0; i < arrCheckedDay.Length; i += 2)
            {
                arrCheckedDay[i] = b;
            }
        }

        async void CreateTripDriverAsync(TripDriver trip)
        {
            RunOnUiThread(() =>
            {
                progress.Show();
            });

            HttpResponseMessage response = await client.PostAsJsonAsync("api/TripDrivers", trip);
            Console.WriteLine("================ 1 ================ ");
            //response.EnsureSuccessStatusCode();
            Console.WriteLine("Response code " + response.StatusCode.ToString());
            Console.WriteLine("Response code 2 " + response.RequestMessage.ToString());
            Console.WriteLine("Response code 3 " + response.Content.ToString());
            Console.WriteLine("================ 2 ================ ");

            if (response.IsSuccessStatusCode)
                Toast.MakeText(this, "Trip created.", ToastLength.Short).Show();
            else
                Toast.MakeText(this, "Failed to create trip.", ToastLength.Short).Show();

            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });

            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Member", JsonConvert.SerializeObject(iMember));
            StartActivity(intent);
            Finish();
        }

        async void CreateTripPassAsync(TripPassenger trip)
        {
            RunOnUiThread(() =>
            {
                progress.Show();
            });
            
            HttpResponseMessage response = await client.PostAsJsonAsync("api/TripPassengers", trip);
            Console.WriteLine("================ 1 ================ ");
            //response.EnsureSuccessStatusCode();
            Console.WriteLine("Response code " + response.StatusCode.ToString());
            Console.WriteLine("Response code 2 " + response.RequestMessage.ToString());
            Console.WriteLine("Response code 3 " + response.Content.ToString());
            Console.WriteLine("================ 2 ================ ");

            if (response.IsSuccessStatusCode)
                Toast.MakeText(this, "Trip created.", ToastLength.Short).Show();
            else
                Toast.MakeText(this, "Failed to create trip.", ToastLength.Short).Show();

            RunOnUiThread(() =>
            {
                progress.Dismiss();
            });

            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Member", JsonConvert.SerializeObject(iMember));
            StartActivity(intent);
            Finish();
        }
    }
}