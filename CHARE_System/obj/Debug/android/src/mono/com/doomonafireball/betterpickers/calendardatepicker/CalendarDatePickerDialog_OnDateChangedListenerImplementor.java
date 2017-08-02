package mono.com.doomonafireball.betterpickers.calendardatepicker;


public class CalendarDatePickerDialog_OnDateChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.calendardatepicker.CalendarDatePickerDialog.OnDateChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDateChanged:()V:GetOnDateChangedHandler:BetterPickers.CalendarDatePickers.CalendarDatePickerDialog/IOnDateChangedListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.CalendarDatePickers.CalendarDatePickerDialog+IOnDateChangedListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CalendarDatePickerDialog_OnDateChangedListenerImplementor.class, __md_methods);
	}


	public CalendarDatePickerDialog_OnDateChangedListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CalendarDatePickerDialog_OnDateChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.CalendarDatePickers.CalendarDatePickerDialog+IOnDateChangedListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDateChanged ()
	{
		n_onDateChanged ();
	}

	private native void n_onDateChanged ();

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
