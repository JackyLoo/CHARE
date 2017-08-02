package mono.com.doomonafireball.betterpickers.recurrencepicker;


public class RecurrencePickerDialog_OnRecurrenceSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.doomonafireball.betterpickers.recurrencepicker.RecurrencePickerDialog.OnRecurrenceSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onRecurrenceSet:(Ljava/lang/String;)V:GetOnRecurrenceSet_Ljava_lang_String_Handler:BetterPickers.RecurrencePickers.RecurrencePickerDialog/IOnRecurrenceSetListenerInvoker, BetterPickers\n" +
			"";
		mono.android.Runtime.register ("BetterPickers.RecurrencePickers.RecurrencePickerDialog+IOnRecurrenceSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RecurrencePickerDialog_OnRecurrenceSetListenerImplementor.class, __md_methods);
	}


	public RecurrencePickerDialog_OnRecurrenceSetListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RecurrencePickerDialog_OnRecurrenceSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("BetterPickers.RecurrencePickers.RecurrencePickerDialog+IOnRecurrenceSetListenerImplementor, BetterPickers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onRecurrenceSet (java.lang.String p0)
	{
		n_onRecurrenceSet (p0);
	}

	private native void n_onRecurrenceSet (java.lang.String p0);

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
