package mono.com.doomonafireball.betterpickers.radialtimepicker;


public class RadialTimePickerDialog_OnTimeSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.radialtimepicker.RadialTimePickerDialog.OnTimeSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimeSet:(Lcom/doomonafireball/betterpickers/radialtimepicker/RadialTimePickerDialog;II)V:GetOnTimeSet_Lcom_doomonafireball_betterpickers_radialtimepicker_RadialTimePickerDialog_IIHandler:BetterPickers.RadialTimePickers.RadialTimePickerDialog/IOnTimeSetListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.RadialTimePickers.RadialTimePickerDialog+IOnTimeSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RadialTimePickerDialog_OnTimeSetListenerImplementor.class, __md_methods);
	}


	public RadialTimePickerDialog_OnTimeSetListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RadialTimePickerDialog_OnTimeSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.RadialTimePickers.RadialTimePickerDialog+IOnTimeSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTimeSet (com.doomonafireball.betterpickers.radialtimepicker.RadialTimePickerDialog p0, int p1, int p2)
	{
		n_onTimeSet (p0, p1, p2);
	}

	private native void n_onTimeSet (com.doomonafireball.betterpickers.radialtimepicker.RadialTimePickerDialog p0, int p1, int p2);

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
