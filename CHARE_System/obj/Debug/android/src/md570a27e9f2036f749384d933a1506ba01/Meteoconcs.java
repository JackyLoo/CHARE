package md570a27e9f2036f749384d933a1506ba01;


public class Meteoconcs
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.mikepenz.iconics.typeface.ITypeface
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getAuthor:()Ljava/lang/String;:GetGetAuthorHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getCharacters:()Ljava/util/HashMap;:GetGetCharactersHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getDescription:()Ljava/lang/String;:GetGetDescriptionHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getFontName:()Ljava/lang/String;:GetGetFontNameHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getIconCount:()I:GetGetIconCountHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getIcons:()Ljava/util/Collection;:GetGetIconsHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getLicense:()Ljava/lang/String;:GetGetLicenseHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getLicenseUrl:()Ljava/lang/String;:GetGetLicenseUrlHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getMappingPrefix:()Ljava/lang/String;:GetGetMappingPrefixHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getUrl:()Ljava/lang/String;:GetGetUrlHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getVersion:()Ljava/lang/String;:GetGetVersionHandler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getIcon:(Ljava/lang/String;)Lcom/mikepenz/iconics/typeface/IIcon;:GetGetIcon_Ljava_lang_String_Handler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"n_getTypeface:(Landroid/content/Context;)Landroid/graphics/Typeface;:GetGetTypeface_Landroid_content_Context_Handler:Mikepenz.Iconics.Typeface.ITypefaceInvoker, android-iconics\n" +
			"";
		mono.android.Runtime.register ("Mikepenz.Typeface.Meteoconcs, android-iconics, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null", Meteoconcs.class, __md_methods);
	}


	public Meteoconcs () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Meteoconcs.class)
			mono.android.TypeManager.Activate ("Mikepenz.Typeface.Meteoconcs, android-iconics, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public java.lang.String getAuthor ()
	{
		return n_getAuthor ();
	}

	private native java.lang.String n_getAuthor ();


	public java.util.HashMap getCharacters ()
	{
		return n_getCharacters ();
	}

	private native java.util.HashMap n_getCharacters ();


	public java.lang.String getDescription ()
	{
		return n_getDescription ();
	}

	private native java.lang.String n_getDescription ();


	public java.lang.String getFontName ()
	{
		return n_getFontName ();
	}

	private native java.lang.String n_getFontName ();


	public int getIconCount ()
	{
		return n_getIconCount ();
	}

	private native int n_getIconCount ();


	public java.util.Collection getIcons ()
	{
		return n_getIcons ();
	}

	private native java.util.Collection n_getIcons ();


	public java.lang.String getLicense ()
	{
		return n_getLicense ();
	}

	private native java.lang.String n_getLicense ();


	public java.lang.String getLicenseUrl ()
	{
		return n_getLicenseUrl ();
	}

	private native java.lang.String n_getLicenseUrl ();


	public java.lang.String getMappingPrefix ()
	{
		return n_getMappingPrefix ();
	}

	private native java.lang.String n_getMappingPrefix ();


	public java.lang.String getUrl ()
	{
		return n_getUrl ();
	}

	private native java.lang.String n_getUrl ();


	public java.lang.String getVersion ()
	{
		return n_getVersion ();
	}

	private native java.lang.String n_getVersion ();


	public com.mikepenz.iconics.typeface.IIcon getIcon (java.lang.String p0)
	{
		return n_getIcon (p0);
	}

	private native com.mikepenz.iconics.typeface.IIcon n_getIcon (java.lang.String p0);


	public android.graphics.Typeface getTypeface (android.content.Context p0)
	{
		return n_getTypeface (p0);
	}

	private native android.graphics.Typeface n_getTypeface (android.content.Context p0);

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
