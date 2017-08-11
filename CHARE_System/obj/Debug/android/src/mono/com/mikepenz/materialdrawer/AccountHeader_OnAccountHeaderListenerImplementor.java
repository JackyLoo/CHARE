package mono.com.mikepenz.materialdrawer;


public class AccountHeader_OnAccountHeaderListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.AccountHeader.OnAccountHeaderListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onProfileChanged:(Landroid/view/View;Lcom/mikepenz/materialdrawer/model/interfaces/IProfile;Z)Z:GetOnProfileChanged_Landroid_view_View_Lcom_mikepenz_materialdrawer_model_interfaces_IProfile_ZHandler:Mikepenz.MaterialDrawer.AccountHeader/IOnAccountHeaderListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AccountHeader_OnAccountHeaderListenerImplementor.class, __md_methods);
	}


	public AccountHeader_OnAccountHeaderListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AccountHeader_OnAccountHeaderListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onProfileChanged (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2)
	{
		return n_onProfileChanged (p0, p1, p2);
	}

	private native boolean n_onProfileChanged (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2);

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
