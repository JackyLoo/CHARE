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
using Android.Support.V4.App;
using Android.Text.Format;
using Android.Content.Res;
using Android.Text;
using Android.Icu.Util;
using static Android.App.TimePickerDialog;
using System.Globalization;
using CHARE_REST_API.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace CHARE_System
{
    [Activity(Label = "Set Route")]
    public class TripConfirmation_2 : FragmentActivity, 
        IOnTimeSetListener
    {
        // Intent Putextra Data
        private Member iMember;
        private Trip iTrip;
        
        private LatLng originLatLng;
        private LatLng destLatLng;

        // Views
        private Button btnConfirm;
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

            client = new HttpClient();
            client.BaseAddress = new Uri(GetString(Resource.String.RestAPIBaseAddress));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            iMember = JsonConvert.DeserializeObject<Member>(Intent.GetStringExtra("Member"));
            iTrip = JsonConvert.DeserializeObject<Trip>(Intent.GetStringExtra("Trip"));
            
            var o = iTrip.origin.Split(',');
            var d = iTrip.destination.Split(',');

            originLatLng = new LatLng(Double.Parse(o[0]), Double.Parse(o[1]));
            destLatLng = new LatLng(Double.Parse(d[0]), Double.Parse(d[1]));

            arrCheckedDay = new bool[7];
            SetDayArrayBool(false);

            btnConfirm = (Button)FindViewById(Resource.Id.btn_tripcon_comfirm);
            tvArriveTime = (TextView)FindViewById(Resource.Id.textview_arrivetime);
            tvDay = (TextView)FindViewById(Resource.Id.textview_day);
            switchFemaleOnly = (Switch)FindViewById(Resource.Id.switch_femaleonly);
            
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
                iTrip.MemberID = iMember.MemberID;
                var json = JsonConvert.SerializeObject(iTrip);
                Console.WriteLine("========================= 1 =========================");
                Console.WriteLine(json.ToString());
                Console.WriteLine("========================= 2 =========================");
                CreateTripAsync(iTrip);
            }
        }

        private void SetDayArrayBool(bool b)
        {
            for (int i = 0; i < arrCheckedDay.Length; i += 2)
            {
                arrCheckedDay[i] = b;
            }
        }

        async void CreateTripAsync(Trip product)
        {            
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Trips", product);
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
            this.Finish();
        }
    }
}