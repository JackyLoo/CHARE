package md5886227016caf2c567dfead95280d352e;


public class HmsPickerDialogHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.hmspicker.HmsPickerDialogFragment.HmsPickerDialogHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDialogHmsSet:(IIII)V:GetOnDialogHmsSet_IIIIHandler:BetterPickers.HmsPickers.HmsPickerDialogFragment/IHmsPickerDialogHandlerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.HmsPickers.HmsPickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", HmsPickerDialogHandler.class, __md_methods);
	}


	public HmsPickerDialogHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == HmsPickerDialogHandler.class)
			mono.android.TypeManager.Activate ("BetterPickers.HmsPickers.HmsPickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDialogHmsSet (int p0, int p1, int p2, int p3)
	{
		n_onDialogHmsSet (p0, p1, p2, p3);
	}

	private native void n_onDialogHmsSet (int p0, int p1, int p2, int p3);

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
