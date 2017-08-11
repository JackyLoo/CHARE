package md570a27e9f2036f749384d933a1506ba01;


public class CommunityMaterial_Icon
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.iconics.typeface.IIcon
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCharacter:()C:GetGetCharacterHandler:Mikepenz.Iconics.Typeface.IIconInvoker, android-iconics\n" +
			"n_getFormattedName:()Ljava/lang/String;:GetGetFormattedNameHandler:Mikepenz.Iconics.Typeface.IIconInvoker, android-iconics\n" +
			"n_getName:()Ljava/lang/String;:GetGetNameHandler:Mikepenz.Iconics.Typeface.IIconInvoker, android-iconics\n" +
			"n_getTypeface:()Lcom/mikepenz/iconics/typeface/ITypeface;:GetGetTypefaceHandler:Mikepenz.Iconics.Typeface.IIconInvoker, android-iconics\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.Typeface.CommunityMaterial+Icon, android-iconics, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null", CommunityMaterial_Icon.class, __md_methods);
	}


	public CommunityMaterial_Icon () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CommunityMaterial_Icon.class)
			mono.android.TypeManager.Activate ("Mikepenz.Typeface.CommunityMaterial+Icon, android-iconics, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public CommunityMaterial_Icon (char p0, java.lang.String p1) throws java.lang.Throwable
	{
		super ();
		if (getClass () == CommunityMaterial_Icon.class)
			mono.android.TypeManager.Activate ("Mikepenz.Typeface.CommunityMaterial+Icon, android-iconics, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null", "System.Char, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1 });
	}


	public char getCharacter ()
	{
		return n_getCharacter ();
	}

	private native char n_getCharacter ();


	public java.lang.String getFormattedName ()
	{
		return n_getFormattedName ();
	}

	private native java.lang.String n_getFormattedName ();


	public java.lang.String getName ()
	{
		return n_getName ();
	}

	private native java.lang.String n_getName ();


	public com.mikepenz.iconics.typeface.ITypeface getTypeface ()
	{
		return n_getTypeface ();
	}

	private native com.mikepenz.iconics.typeface.ITypeface n_getTypeface ();

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
