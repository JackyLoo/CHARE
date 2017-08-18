package md5e8fccd3fcd8f74a82435f3bbd25804a2;


public class DriverViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("CHARE_System.DriverViewHolder, CHARE_System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DriverViewHolder.class, __md_methods);
	}


	public DriverViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DriverViewHolder.class)
			mono.android.TypeManager.Activate ("CHARE_System.DriverViewHolder, CHARE_System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
