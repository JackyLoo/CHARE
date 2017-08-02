package md5e96e589585119beea4ac2ace43b2184c;


public class TimePickerDialogHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.timepicker.TimePickerDialogFragment.TimePickerDialogHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDialogTimeSet:(III)V:GetOnDialogTimeSet_IIIHandler:BetterPickers.TimePickers.TimePickerDialogFragment/ITimePickerDialogHandlerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.TimePickers.TimePickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TimePickerDialogHandler.class, __md_methods);
	}


	public TimePickerDialogHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TimePickerDialogHandler.class)
			mono.android.TypeManager.Activate ("BetterPickers.TimePickers.TimePickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDialogTimeSet (int p0, int p1, int p2)
	{
		n_onDialogTimeSet (p0, p1, p2);
	}

	private native void n_onDialogTimeSet (int p0, int p1, int p2);

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
