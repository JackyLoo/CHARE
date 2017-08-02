package mono.com.doomonafireball.betterpickers.timezonepicker;


public class TimeZoneFilterTypeAdapter_OnSetFilterListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.timezonepicker.TimeZoneFilterTypeAdapter.OnSetFilterListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSetFilter:(ILjava/lang/String;I)V:GetOnSetFilter_ILjava_lang_String_IHandler:BetterPickers.TimeZonePickers.TimeZoneFilterTypeAdapter/IOnSetFilterListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.TimeZonePickers.TimeZoneFilterTypeAdapter+IOnSetFilterListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TimeZoneFilterTypeAdapter_OnSetFilterListenerImplementor.class, __md_methods);
	}


	public TimeZoneFilterTypeAdapter_OnSetFilterListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == TimeZoneFilterTypeAdapter_OnSetFilterListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.TimeZonePickers.TimeZoneFilterTypeAdapter+IOnSetFilterListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onSetFilter (int p0, java.lang.String p1, int p2)
	{
		n_onSetFilter (p0, p1, p2);
	}

	private native void n_onSetFilter (int p0, java.lang.String p1, int p2);

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
