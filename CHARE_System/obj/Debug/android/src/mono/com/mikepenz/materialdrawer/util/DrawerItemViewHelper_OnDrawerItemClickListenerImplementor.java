package mono.com.mikepenz.materialdrawer.util;


public class DrawerItemViewHelper_OnDrawerItemClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.util.DrawerItemViewHelper.OnDrawerItemClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onItemClick:(Landroid/view/View;Lcom/mikepenz/materialdrawer/model/interfaces/IDrawerItem;)V:GetOnItemClick_Landroid_view_View_Lcom_mikepenz_materialdrawer_model_interfaces_IDrawerItem_Handler:Mikepenz.MaterialDrawer.Utils.DrawerItemViewHelper/IOnDrawerItemClickListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.Utils.DrawerItemViewHelper+IOnDrawerItemClickListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DrawerItemViewHelper_OnDrawerItemClickListenerImplementor.class, __md_methods);
	}


	public DrawerItemViewHelper_OnDrawerItemClickListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DrawerItemViewHelper_OnDrawerItemClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.Utils.DrawerItemViewHelper+IOnDrawerItemClickListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onItemClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p1)
	{
		n_onItemClick (p0, p1);
	}

	private native void n_onItemClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p1);

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
