package mono.com.mikepenz.materialdrawer.interfaces;


public class OnCheckedChangeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.materialdrawer.interfaces.OnCheckedChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCheckedChanged:(Lcom/mikepenz/materialdrawer/model/interfaces/IDrawerItem;Landroid/widget/CompoundButton;Z)V:GetOnCheckedChanged_Lcom_mikepenz_materialdrawer_model_interfaces_IDrawerItem_Landroid_widget_CompoundButton_ZHandler:Com.Mikepenz.Materialdrawer.Interfaces.IOnCheckedChangeListenerInvoker, material-drawer\n" +
			"";
		mono.android.Runtime.register ("Com.Mikepenz.Materialdrawer.Interfaces.IOnCheckedChangeListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OnCheckedChangeListenerImplementor.class, __md_methods);
	}


	public OnCheckedChangeListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == OnCheckedChangeListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Mikepenz.Materialdrawer.Interfaces.IOnCheckedChangeListenerImplementor, material-drawer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCheckedChanged (com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p0, android.widget.CompoundButton p1, boolean p2)
	{
		n_onCheckedChanged (p0, p1, p2);
	}

	private native void n_onCheckedChanged (com.mikepenz.materialdrawer.model.interfaces.IDrawerItem p0, android.widget.CompoundButton p1, boolean p2);

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
