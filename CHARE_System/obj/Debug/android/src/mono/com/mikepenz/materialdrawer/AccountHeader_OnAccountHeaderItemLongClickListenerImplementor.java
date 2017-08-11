package mono.com.mikepenz.materialdrawer;


public class AccountHeader_OnAccountHeaderItemLongClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.AccountHeader.OnAccountHeaderItemLongClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onProfileLongClick:(Landroid/view/View;Lcom/mikepenz/materialdrawer/model/interfaces/IProfile;Z)Z:GetOnProfileLongClick_Landroid_view_View_Lcom_mikepenz_materialdrawer_model_interfaces_IProfile_ZHandler:Mikepenz.MaterialDrawer.AccountHeader/IOnAccountHeaderItemLongClickListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderItemLongClickListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AccountHeader_OnAccountHeaderItemLongClickListenerImplementor.class, __md_methods);
	}


	public AccountHeader_OnAccountHeaderItemLongClickListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AccountHeader_OnAccountHeaderItemLongClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderItemLongClickListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onProfileLongClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2)
	{
		return n_onProfileLongClick (p0, p1, p2);
	}

	private native boolean n_onProfileLongClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2);

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
