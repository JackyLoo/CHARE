package md5e8fccd3fcd8f74a82435f3bbd25804a2;


public class SignupActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("CHARE_System.SignupActivity, CHARE_System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SignupActivity.class, __md_methods);
	}


	public SignupActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SignupActivity.class)
			mono.android.TypeManager.Activate ("CHARE_System.SignupActivity, CHARE_System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
