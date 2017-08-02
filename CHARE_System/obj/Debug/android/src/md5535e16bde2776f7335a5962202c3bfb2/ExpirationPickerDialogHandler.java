package md5535e16bde2776f7335a5962202c3bfb2;


public class ExpirationPickerDialogHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.expirationpicker.ExpirationPickerDialogFragment.ExpirationPickerDialogHandler
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDialogExpirationSet:(III)V:GetOnDialogExpirationSet_IIIHandler:BetterPickers.ExpirationPickers.ExpirationPickerDialogFragment/IExpirationPickerDialogHandlerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.ExpirationPickers.ExpirationPickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ExpirationPickerDialogHandler.class, __md_methods);
	}


	public ExpirationPickerDialogHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ExpirationPickerDialogHandler.class)
			mono.android.TypeManager.Activate ("BetterPickers.ExpirationPickers.ExpirationPickerDialogHandler, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDialogExpirationSet (int p0, int p1, int p2)
	{
		n_onDialogExpirationSet (p0, p1, p2);
	}

	private native void n_onDialogExpirationSet (int p0, int p1, int p2);

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
