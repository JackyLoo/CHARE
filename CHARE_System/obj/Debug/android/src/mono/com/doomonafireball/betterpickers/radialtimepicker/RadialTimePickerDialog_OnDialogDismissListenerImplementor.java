package mono.com.doomonafireball.betterpickers.radialtimepicker;


public class RadialTimePickerDialog_OnDialogDismissListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.radialtimepicker.RadialTimePickerDialog.OnDialogDismissListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDialogDismiss:(Landroid/content/DialogInterface;)V:GetOnDialogDismiss_Landroid_content_DialogInterface_Handler:BetterPickers.RadialTimePickers.RadialTimePickerDialog/IOnDialogDismissListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.RadialTimePickers.RadialTimePickerDialog+IOnDialogDismissListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RadialTimePickerDialog_OnDialogDismissListenerImplementor.class, __md_methods);
	}


	public RadialTimePickerDialog_OnDialogDismissListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RadialTimePickerDialog_OnDialogDismissListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.RadialTimePickers.RadialTimePickerDialog+IOnDialogDismissListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDialogDismiss (android.content.DialogInterface p0)
	{
		n_onDialogDismiss (p0);
	}

	private native void n_onDialogDismiss (android.content.DialogInterface p0);

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
