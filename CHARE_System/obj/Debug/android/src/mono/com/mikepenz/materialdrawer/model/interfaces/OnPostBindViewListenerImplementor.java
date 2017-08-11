package mono.com.mikepenz.materialdrawer.model.interfaces;


public class OnPostBindViewListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.model.interfaces.OnPostBindViewListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBindView:(Lcom/mikepenz/materialdrawer/model/interfaces/IDrawerItem;Landroid/view/View;)V:GetOnBindView_Lcom_mikepenz_materialdrawer_model_interfaces_IDrawerItem_Landroid_view_View_Handler:Mikepenz.MaterialDrawer.Models.Interfaces.IOnPostBindViewListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.Models.Interfaces.IOnPostBindViewListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OnPostBindViewListenerImplementor.class, __md_methods);
	}


	public OnPostBindViewListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == OnPostBindViewListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.Models.Interfaces.IOnPostBindViewListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onBindView (com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p0, android.view.View p1)
	{
		n_onBindView (p0, p1);
	}

	private native void n_onBindView (com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p0, android.view.View p1);

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
