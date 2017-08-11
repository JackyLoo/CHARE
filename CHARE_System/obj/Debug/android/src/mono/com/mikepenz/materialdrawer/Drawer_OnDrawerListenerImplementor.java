package mono.com.mikepenz.materialdrawer;


public class Drawer_OnDrawerListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.Drawer.OnDrawerListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDrawerClosed:(Landroid/view/View;)V:GetOnDrawerClosed_Landroid_view_View_Handler:Mikepenz.MaterialDrawer.Drawer/IOnDrawerListenerInvoker, material-drawer\n" +
			"n_onDrawerOpened:(Landroid/view/View;)V:GetOnDrawerOpened_Landroid_view_View_Handler:Mikepenz.MaterialDrawer.Drawer/IOnDrawerListenerInvoker, material-drawer\n" +
			"n_onDrawerSlide:(Landroid/view/View;F)V:GetOnDrawerSlide_Landroid_view_View_FHandler:Mikepenz.MaterialDrawer.Drawer/IOnDrawerListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.Drawer+IOnDrawerListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Drawer_OnDrawerListenerImplementor.class, __md_methods);
	}


	public Drawer_OnDrawerListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Drawer_OnDrawerListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.Drawer+IOnDrawerListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDrawerClosed (android.view.View p0)
	{
		n_onDrawerClosed (p0);
	}

	private native void n_onDrawerClosed (android.view.View p0);


	public void onDrawerOpened (android.view.View p0)
	{
		n_onDrawerOpened (p0);
	}

	private native void n_onDrawerOpened (android.view.View p0);


	public void onDrawerSlide (android.view.View p0, float p1)
	{
		n_onDrawerSlide (p0, p1);
	}

	private native void n_onDrawerSlide (android.view.View p0, float p1);

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
