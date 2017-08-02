package mono.com.doomonafireball.betterpickers.calendardatepicker;


public class MonthView_OnDayClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.calendardatepicker.MonthView.OnDayClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDayClick:(Lcom/doomonafireball/betterpickers/calendardatepicker/MonthView;Lcom/doomonafireball/betterpickers/calendardatepicker/MonthAdapter$CalendarDay;)V:GetOnDayClick_Lcom_doomonafireball_betterpickers_calendardatepicker_MonthView_Lcom_doomonafireball_betterpickers_calendardatepicker_MonthAdapter_CalendarDay_Handler:BetterPickers.CalendarDatePickers.MonthView/IOnDayClickListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.CalendarDatePickers.MonthView+IOnDayClickListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MonthView_OnDayClickListenerImplementor.class, __md_methods);
	}


	public MonthView_OnDayClickListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MonthView_OnDayClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.CalendarDatePickers.MonthView+IOnDayClickListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDayClick (com.doomonafireball.betterpickers.calendardatepicker.MonthView p0, com.doomonafireball.betterpickers.calendardatepicker.MonthAdapter.CalendarDay p1)
	{
		n_onDayClick (p0, p1);
	}

	private native void n_onDayClick (com.doomonafireball.betterpickers.calendardatepicker.MonthView p0, com.doomonafireball.betterpickers.calendardatepicker.MonthAdapter.CalendarDay p1);

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
