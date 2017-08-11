package mono.com.mikepenz.materialdrawer;


public class Drawer_OnDrawerNavigationListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.Drawer.OnDrawerNavigationListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onNavigationClickListener:(Landroid/view/View;)Z:GetOnNavigationClickListener_Landroid_view_View_Handler:Mikepenz.MaterialDrawer.Drawer/IOnDrawerNavigationListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.Drawer+IOnDrawerNavigationListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Drawer_OnDrawerNavigationListenerImplementor.class, __md_methods);
	}


	public Drawer_OnDrawerNavigationListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Drawer_OnDrawerNavigationListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.Drawer+IOnDrawerNavigationListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onNavigationClickListener (android.view.View p0)
	{
		return n_onNavigationClickListener (p0);
	}

	private native boolean n_onNavigationClickListener (android.view.View p0);

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
