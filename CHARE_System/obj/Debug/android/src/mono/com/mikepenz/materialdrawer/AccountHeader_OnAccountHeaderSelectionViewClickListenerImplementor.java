package mono.com.mikepenz.materialdrawer;


public class AccountHeader_OnAccountHeaderSelectionViewClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.AccountHeader.OnAccountHeaderSelectionViewClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;Lcom/mikepenz/materialdrawer/model/interfaces/IProfile;)Z:GetOnClick_Landroid_view_View_Lcom_mikepenz_materialdrawer_model_interfaces_IProfile_Handler:Mikepenz.MaterialDrawer.AccountHeader/IOnAccountHeaderSelectionViewClickListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderSelectionViewClickListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AccountHeader_OnAccountHeaderSelectionViewClickListenerImplementor.class, __md_methods);
	}


	public AccountHeader_OnAccountHeaderSelectionViewClickListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AccountHeader_OnAccountHeaderSelectionViewClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("Mikepenz.MaterialDrawer.AccountHeader+IOnAccountHeaderSelectionViewClickListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1)
	{
		return n_onClick (p0, p1);
	}

	private native boolean n_onClick (android.view.View p0, com.mikepenz.materialdrawer.model.interfaces.IProfile p1);

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
