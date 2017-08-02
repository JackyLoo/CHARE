package mono.com.doomonafireball.betterpickers.calendardatepicker;


public class CalendarDatePickerDialog_OnDateSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.calendardatepicker.CalendarDatePickerDialog.OnDateSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDateSet:(Lcom/doomonafireball/betterpickers/calendardatepicker/CalendarDatePickerDialog;III)V:GetOnDateSet_Lcom_doomonafireball_betterpickers_calendardatepicker_CalendarDatePickerDialog_IIIHandler:BetterPickers.CalendarDatePickers.CalendarDatePickerDialog/IOnDateSetListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.CalendarDatePickers.CalendarDatePickerDialog+IOnDateSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CalendarDatePickerDialog_OnDateSetListenerImplementor.class, __md_methods);
	}


	public CalendarDatePickerDialog_OnDateSetListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CalendarDatePickerDialog_OnDateSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.CalendarDatePickers.CalendarDatePickerDialog+IOnDateSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDateSet (com.doomonafireball.betterpickers.calendardatepicker.CalendarDatePickerDialog p0, int p1, int p2, int p3)
	{
		n_onDateSet (p0, p1, p2, p3);
	}

	private native void n_onDateSet (com.doomonafireball.betterpickers.calendardatepicker.CalendarDatePickerDialog p0, int p1, int p2, int p3);

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
