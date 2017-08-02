package mono.com.doomonafireball.betterpickers.timezonepicker;


public class TimeZonePickerView_OnTimeZoneSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.timezonepicker.TimeZonePickerView.OnTimeZoneSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimeZoneSet:(Lcom/doomonafireball/betterpickers/timezonepicker/TimeZoneInfo;)V:GetOnTimeZoneSet_Lcom_doomonafireball_betterpickers_timezonepicker_TimeZoneInfo_Handler:BetterPickers.TimeZonePickers.TimeZonePickerView/IOnTimeZoneSetListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.TimeZonePickers.TimeZonePickerView+IOnTimeZoneSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TimeZonePickerView_OnTimeZoneSetListenerImplementor.class, __md_methods);
	}


	public TimeZonePickerView_OnTimeZoneSetListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TimeZonePickerView_OnTimeZoneSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.TimeZonePickers.TimeZonePickerView+IOnTimeZoneSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
