package md5e8fccd3fcd8f74a82435f3bbd25804a2;


public class TripDriverDetailsRow_Edit
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.OnMapReadyCallback,
		android.app.TimePickerDialog.OnTimeSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"n_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"n_onTimeSet:(Landroid/widget/TimePicker;II)V:GetOnTimeSet_Landroid_widget_TimePicker_IIHandler:Android.App.TimePickerDialog/IOnTimeSetListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("CHARE_System.TripDriverDetailsRow_Edit, CHARE_System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TripDriverDetailsRow_Edit.class, __md_methods);
	}


	public TripDriverDetailsRow_Edit () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TripDriverDetailsRow_Edit.class)
			mono.android.TypeManager.Activate ("CHARE_System.TripDriverDetailsRow_Edit, CHARE_System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onOptionsItemSelected (android.view.MenuItem p0)
	{
		return n_onOptionsItemSelected (p0);
	}

	private native boolean n_onOptionsItemSelected (android.view.MenuItem p0);


	public void onMapReady (com.google.android.gms.maps.GoogleMap p0)
	{
		n_onMapReady (p0);
	}

	private native void n_onMapReady (com.google.android.gms.maps.GoogleMap p0);


	public void onTimeSet (android.widget.TimePicker p0, int p1, int p2)
	{
		n_onTimeSet (p0, p1, p2);
	}

	private native void n_onTimeSet (android.widget.TimePicker p0, int p1, int p2);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
