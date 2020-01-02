using UnityEngine;
using UnityEditor;

public class ScifiFXUI : ShaderGUI
{

	MaterialEditor editor;
	MaterialProperty[] properties;

	//get preperties function
	MaterialProperty FindProperty (string name) 
	{
		return FindProperty(name, properties);
	}
	//

	////
	static GUIContent staticLabel = new GUIContent();
	static GUIContent MakeLabel (MaterialProperty property, string tooltip = null) 
	{
		staticLabel.text = property.displayName;
		staticLabel.tooltip = tooltip;
		return staticLabel;
	}
	////

	public override void OnGUI (MaterialEditor editor, MaterialProperty[] properties) 
	{
		this.editor = editor;
		this.properties = properties;
		DoMain();

	}


	// GUI FUNCTION	
	void DoMain() 
	{
		//--- Logo
		Texture2D myGUITexture  = (Texture2D)Resources.Load("SciFiFXSphericalPack");
		GUILayout.Label(myGUITexture,EditorStyles.centeredGreyMiniLabel);

		//LABELS
		GUILayout.Label("/---------------/ SCI-FI SPHERIC PACK /---------------/", EditorStyles.centeredGreyMiniLabel);
		GUILayout.Label("FRONT FACE", EditorStyles.helpBox);

		// get properties
		MaterialProperty _FrontFace_Diffuse_map = ShaderGUI.FindProperty("_FrontFace_Diffuse_map", properties);

		//Add to GUI
		editor.TexturePropertySingleLine(MakeLabel(_FrontFace_Diffuse_map,"FFace Map"), _FrontFace_Diffuse_map,FindProperty("_FrontFace_Color"));
		editor.TextureScaleOffsetProperty (_FrontFace_Diffuse_map);

		MaterialProperty _FrontFace_Intensity = FindProperty("_FrontFace_Intensity");
		editor.ShaderProperty(_FrontFace_Intensity, MakeLabel(_FrontFace_Intensity));


		//--------------------

		//LABELS
		GUILayout.Label("BACK FACE", EditorStyles.helpBox);

		// get properties
		MaterialProperty _BackFace_Diffuse_map = ShaderGUI.FindProperty("_BackFace_Diffuse_map", properties);

		//Add to GUI
		editor.TexturePropertySingleLine(MakeLabel(_BackFace_Diffuse_map,"BFace Map"), _BackFace_Diffuse_map,FindProperty("_BackFace_Color"));
		editor.TextureScaleOffsetProperty (_BackFace_Diffuse_map);

		MaterialProperty _BackFace_Intensity = FindProperty("_BackFace_Intensity");
		editor.ShaderProperty(_BackFace_Intensity, MakeLabel(_BackFace_Intensity));


		//--------------------

		//LABELS
		GUILayout.Label("OUTLINE", EditorStyles.helpBox);

		// get properties
		MaterialProperty _OutlineTex = ShaderGUI.FindProperty("_OutlineTex", properties);

		//Add to GUI
		editor.TexturePropertySingleLine(MakeLabel(_OutlineTex,"Outline Map"), _OutlineTex,FindProperty("_Outline_Color"));
		editor.TextureScaleOffsetProperty (_OutlineTex);

		MaterialProperty _Outline_Opacity = FindProperty("_Outline_Opacity");
		editor.ShaderProperty(_Outline_Opacity, MakeLabel(_Outline_Opacity));


		//--------------------

		//LABELS
		GUILayout.Label("DISPLACEMENT", EditorStyles.helpBox);

		// get properties
		MaterialProperty _DisplacementMask = ShaderGUI.FindProperty("_DisplacementMask", properties);

		//Add to GUI
		editor.TexturePropertySingleLine(MakeLabel(_DisplacementMask,"Mask Map"), _DisplacementMask);
		editor.TextureScaleOffsetProperty (_DisplacementMask);


		//--------------------

		//LABELS
		GUILayout.Label("SETTINGS", EditorStyles.helpBox);

		MaterialProperty _NormalPush = FindProperty("_NormalPush");
		editor.ShaderProperty(_NormalPush, MakeLabel(_NormalPush));

		MaterialProperty _Shrink_Faces_Amplitude = FindProperty("_Shrink_Faces_Amplitude");
		editor.ShaderProperty(_Shrink_Faces_Amplitude, MakeLabel(_Shrink_Faces_Amplitude));

		MaterialProperty _Animation_speed = FindProperty("_Animation_speed");
		editor.ShaderProperty(_Animation_speed, MakeLabel(_Animation_speed));

		MaterialProperty _deformation_type_Factor = FindProperty("_deformation_type_Factor");
		editor.ShaderProperty(_deformation_type_Factor, MakeLabel(_deformation_type_Factor));

		MaterialProperty _deformation_type = FindProperty("_deformation_type");
		editor.ShaderProperty(_deformation_type, MakeLabel(_deformation_type));


	}
}