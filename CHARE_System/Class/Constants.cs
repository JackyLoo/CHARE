using System.Collections.Generic;
using Android.Gms.Maps.Model;

namespace CHARE_System.Class
{
	public static class Constants
	{
		public const string PACKAGE_NAME = "com.xamarin.geofencing";
		public const string SHARED_PREFERENCES_NAME = PACKAGE_NAME + ".SHARED_PREFERENCES_NAME";
		public const string GEOFENCES_ADDED_KEY = PACKAGE_NAME + ".GEOFENCES_ADDED_KEY";

		public const long GEOFENCE_EXPIRATION_IN_HOURS = 12;
		public const long GEOFENCE_EXPIRATION_IN_MILLISECONDS =	GEOFENCE_EXPIRATION_IN_HOURS * 60 * 60 * 1000;
		public const float GEOFENCE_RADIUS_IN_METERS = 800;
        // Aman siara 3.272952,101.647964
        // City mall = 3.130228, 101.627903
        // 3.112311, 101.632065
        public static readonly Dictionary<string, LatLng> BAY_AREA_LANDMARKS = new Dictionary<string, LatLng> {
			{ "SFO", new LatLng (3.272952,101.647964) },
            { "TPC", new LatLng ( 3.130228, 101.627903) },
            { "UMW", new LatLng ( 3.112311, 101.632065) },
            { "SLY", new LatLng ( 3.243064, 101.646366) }
        };
	}
}