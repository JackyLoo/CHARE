package md518e13deb993995f3472c9ed97fd22445;


public class DatePickerDialogHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.datepicker.DatePickerDialogFragment.DatePickerDialogHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDialogDateSet:(IIII)V:GetOnDialogDateSet_IIIIHandler:BetterPickers.DatePickers.DatePickerDialogFragment/IDatePickerDialogHandlerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.DatePickers.DatePickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DatePickerDialogHandler.class, __md_methods);
	}


	public DatePickerDialogHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DatePickerDialogHandler.class)
			mono.android.TypeManager.Activate ("BetterPickers.DatePickers.DatePickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDialogDateSet (int p0, int p1, int p2, int p3)
	{
		n_onDialogDateSet (p0, p1, p2, p3);
	}

	private native void n_onDialogDateSet (int p0, int p1, int p2, int p3);

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
