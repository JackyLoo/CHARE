package mono.com.mikepenz.materialdrawer;


public class Drawer_OnDrawerItemSelectedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.Drawer.OnDrawerItemSelectedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onItemSelected:(Landroid/widget/AdapterView;Landroid/view/View;IJLcom/mikepenz/materialdrawer/model/interfaces/IDrawerItem;)V:GetOnItemSelected_Landroid_widget_AdapterView_Landroid_view_View_IJLcom_mikepenz_materialdrawer_model_interfaces_IDrawerItem_Handler:Mikepenz.MaterialDrawer.Drawer/IOnDrawerItemSelectedListenerInvoker, material-drawer\n" +
			"n_onNothingSelected:(Landroid/widget/AdapterView;)V:GetOnNothingSelected_Landroid_widget_AdapterView_Handler:Mikepenz.MaterialDrawer.Drawer/IOnDrawerItemSelectedListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.Drawer+IOnDrawerItemSelectedListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Drawer_OnDrawerItemSelectedListenerImplementor.class, __md_methods);
	}


	public Drawer_OnDrawerItemSelectedListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Drawer_OnDrawerItemSelectedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.Drawer+IOnDrawerItemSelectedListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onItemSelected (android.widget.AdapterView p0, android.view.View p1, int p2, long p3, com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p4)
	{
		n_onItemSelected (p0, p1, p2, p3, p4);
	}

	private native void n_onItemSelected (android.widget.AdapterView p0, android.view.View p1, int p2, long p3, com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p4);


	public void onNothingSelected (android.widget.AdapterView p0)
	{
		n_onNothingSelected (p0);
	}

	private native void n_onNothingSelected (android.widget.AdapterView p0);

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
