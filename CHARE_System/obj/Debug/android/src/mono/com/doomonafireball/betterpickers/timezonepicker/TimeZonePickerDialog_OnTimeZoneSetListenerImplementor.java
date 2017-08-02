package mono.com.doomonafireball.betterpickers.timezonepicker;


public class TimeZonePickerDialog_OnTimeZoneSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.timezonepicker.TimeZonePickerDialog.OnTimeZoneSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimeZoneSet:(Lcom/doomonafireball/betterpickers/timezonepicker/TimeZoneInfo;)V:GetOnTimeZoneSet_Lcom_doomonafireball_betterpickers_timezonepicker_TimeZoneInfo_Handler:BetterPickers.TimeZonePickers.TimeZonePickerDialog/IOnTimeZoneSetListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.TimeZonePickers.TimeZonePickerDialog+IOnTimeZoneSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TimeZonePickerDialog_OnTimeZoneSetListenerImplementor.class, __md_methods);
	}


	public TimeZonePickerDialog_OnTimeZoneSetListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TimeZonePickerDialog_OnTimeZoneSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.TimeZonePickers.TimeZonePickerDialog+IOnTimeZoneSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTimeZoneSet (com.doomonafireball.betterpickers.timezonepicker.TimeZoneInfo p0)
	{
		n_onTimeZoneSet (p0);
	}

	private native void n_onTimeZoneSet (com.doomonafireball.betterpickers.timezonepicker.TimeZoneInfo p0);

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
