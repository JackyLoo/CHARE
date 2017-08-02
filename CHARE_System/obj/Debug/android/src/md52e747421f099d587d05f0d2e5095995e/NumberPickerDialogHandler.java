package md52e747421f099d587d05f0d2e5095995e;


public class NumberPickerDialogHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.numberpicker.NumberPickerDialogFragment.NumberPickerDialogHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDialogNumberSet:(IIDZD)V:GetOnDialogNumberSet_IIDZDHandler:BetterPickers.NumberPickers.NumberPickerDialogFragment/INumberPickerDialogHandlerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.NumberPickers.NumberPickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NumberPickerDialogHandler.class, __md_methods);
	}


	public NumberPickerDialogHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NumberPickerDialogHandler.class)
			mono.android.TypeManager.Activate ("BetterPickers.NumberPickers.NumberPickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDialogNumberSet (int p0, int p1, double p2, boolean p3, double p4)
	{
		n_onDialogNumberSet (p0, p1, p2, p3, p4);
	}

	private native void n_onDialogNumberSet (int p0, int p1, double p2, boolean p3, double p4);

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
