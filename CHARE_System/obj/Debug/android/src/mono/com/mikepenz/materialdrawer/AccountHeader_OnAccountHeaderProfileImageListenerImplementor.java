package mono.com.mikepenz.materialdrawer;


public class AccountHeader_OnAccountHeaderProfileImageListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.AccountHeader.OnAccountHeaderProfileImageListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onProfileImageClick:(Landroid/view/View;Lcom/mikepenz/materialdrawer/model/interfaces/IProfile;Z)Z:GetOnProfileImageClick_Landroid_view_View_Lcom_mikepenz_materialdrawer_model_interfaces_IProfile_ZHandler:Mikepenz.MaterialDrawer.AccountHeader/IOnAccountHeaderProfileImageListenerInvoker, material-drawer\n" +
			"n_onProfileImageLongClick:(Landroid/view/View;Lcom/mikepenz/materialdrawer/model/interfaces/IProfile;Z)Z:GetOnProfileImageLongClick_Landroid_view_View_Lcom_mikepenz_materialdrawer_model_interfaces_IProfile_ZHandler:Mikepenz.MaterialDrawer.AccountHeader/IOnAccountHeaderProfileImageListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderProfileImageListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AccountHeader_OnAccountHeaderProfileImageListenerImplementor.class, __md_methods);
	}


	public AccountHeader_OnAccountHeaderProfileImageListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AccountHeader_OnAccountHeaderProfileImageListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderProfileImageListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onProfileImageClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2)
	{
		return n_onProfileImageClick (p0, p1, p2);
	}

	private native boolean n_onProfileImageClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2);


	public boolean onProfileImageLongClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2)
	{
		return n_onProfileImageLongClick (p0, p1, p2);
	}

	private native boolean n_onProfileImageLongClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1, boolean p2);

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
