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
using BetterPickers.RecurrencePickers;
using Android.Text.Format;
using Android.Content.Res;
using Android.Text;
using Android.Icu.Util;
using static Android.App.TimePickerDialog;
using System.Globalization;

namespace CHARE_System
{
    [Activity(Label = "TripConfirmation_2")]
    public class TripConfirmation_2 : FragmentActivity, RecurrencePickerDialog.IOnRecurrenceSetListener, 
        IOnTimeSetListener
    {
        private Button btnConfirm;
        private TextView tvArriveTime;
        private TextView tvDay;
        private Switch switchRepeat;
        private Switch switchFemaleOnly;

        private LatLng originLatLng;
        private LatLng destLatLng;

        private EventRecurrence er = new EventRecurrence();
        private string mRrule;

        int hour, minute;        

        private ToggleButton tbtnMon;
        private ToggleButton tbtnTue;
        private ToggleButton tbtnWed;
        private ToggleButton tbtnThu;
        private ToggleButton tbtnFri;
        private ToggleButton tbtnSat;
        private ToggleButton tbtnSun;
        private Button btnDayConfirm;
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

        public void OnRecurrenceSet(string rrule)
        {
            mRrule = rrule;
            if (mRrule != null)
            {
                er.Parse(mRrule);
            }

            Resources r = Resources;
            String repeatString = "";
            bool enabled;
            if (!TextUtils.IsEmpty(mRrule))
            {
                repeatString = EventRecurrenceFormatter.GetRepeatString(this, r, er, true);
            }

            Console.WriteLine("=============================================================");
            Console.WriteLine(mRrule + "\n" + repeatString);
            Console.WriteLine("=============================================================");

        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            this.hour = hourOfDay;
            this.minute = minute;

            int totalInSecond = (hourOfDay * 3600) + (minute * 60);
            TimeSpan pickedTime = TimeSpan.FromSeconds(totalInSecond);
            string strTime = DateTime.ParseExact(pickedTime.ToString(@"hh\:mm"), "HH:mm", null).ToString("hh:mm tt", 
                CultureInfo.GetCultureInfo("en-US"));
           
            tvArriveTime.Text = strTime;
            Console.WriteLine("=============================================");
            Console.WriteLine("Hour = " + hourOfDay);
            Console.WriteLine("Mins = " + minute);
            Console.WriteLine("=============================================");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TripConfirmation_2);

            btnConfirm = (Button)FindViewById(Resource.Id.btn_tripcon_comfirm);

            tvArriveTime = (TextView)FindViewById(Resource.Id.textview_arrivetime);
            tvDay = (TextView)FindViewById(Resource.Id.textview_day);
            switchRepeat = (Switch)FindViewById(Resource.Id.switch_repeat);
            switchFemaleOnly = (Switch)FindViewById(Resource.Id.switch_femaleonly);

            
            btnConfirm.Click += (sender, e) =>
            {
                /*
                Intent intent = new Intent(this, typeof(TripConfirmation_2));
                intent.PutExtra("originLat", originLatLng.Latitude.ToString());
                intent.PutExtra("originLng", originLatLng.Longitude.ToString());
                intent.PutExtra("destLat", destLatLng.Latitude.ToString());
                intent.PutExtra("destLng", destLatLng.Longitude.ToString());
                StartActivity(intent);
                */
            };

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

                btnDayConfirm = (Button)dialog.FindViewById(Resource.Id.btn_tripcon_day_confirm);
                btnDayConfirm.Click += (sender2, e2) =>
                {
                    var listDay = new List<KeyValuePair<bool, string>>();
                    List<CustomPair> listDays = new List<CustomPair>();

                    strPickedDays = "";

                    if (tbtnMon.Checked)
                        listDays.Add(new CustomPair(1, "Mon"));
                    if (tbtnTue.Checked)
                        listDays.Add(new CustomPair(2, "Tue"));
                    if (tbtnWed.Checked)
                        listDays.Add(new CustomPair(3, "Wed"));
                    if (tbtnThu.Checked)
                        listDays.Add(new CustomPair(4, "Thu"));
                    if (tbtnFri.Checked)
                        listDays.Add(new CustomPair(5, "Fri"));
                    if (tbtnSat.Checked)
                        listDays.Add(new CustomPair(6, "Sat"));
                    if (tbtnSun.Checked)
                        listDays.Add(new CustomPair(7, "Sun"));

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



            double originLat = Convert.ToDouble(Intent.GetStringExtra("originLat") ?? "Data not available");
            double originLng = Convert.ToDouble(Intent.GetStringExtra("originLng") ?? "Data not available");
            double destLat = Convert.ToDouble(Intent.GetStringExtra("destLat") ?? "Data not available");
            double destLng = Convert.ToDouble(Intent.GetStringExtra("destLng") ?? "Data not available");

            originLatLng = new LatLng(originLat, originLng);
            destLatLng = new LatLng(destLat, destLng);            
        }
    }
}