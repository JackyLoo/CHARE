package mono.com.doomonafireball.betterpickers.radialtimepicker;


public class RadialPickerLayout_OnValueSelectedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.radialtimepicker.RadialPickerLayout.OnValueSelectedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onValueSelected:(IIZ)V:GetOnValueSelected_IIZHandler:BetterPickers.RadialTimePickers.RadialPickerLayout/IOnValueSelectedListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.RadialTimePickers.RadialPickerLayout+IOnValueSelectedListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RadialPickerLayout_OnValueSelectedListenerImplementor.class, __md_methods);
	}


	public RadialPickerLayout_OnValueSelectedListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RadialPickerLayout_OnValueSelectedListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.RadialTimePickers.RadialPickerLayout+IOnValueSelectedListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onValueSelected (int p0, int p1, boolean p2)
	{
		n_onValueSelected (p0, p1, p2);
	}

	private native void n_onValueSelected (int p0, int p1, boolean p2);

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
