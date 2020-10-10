//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEditor;
using UnityEngine;

/// <summary>
/// Inspector class used to edit UISprites.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UIPolygonSprite), true)]
public class UIPolygonInspector : UIBasicSpriteEditor
{
	/// <summary>
	/// Atlas selection callback.
	/// </summary>

	void OnSelectAtlas (Object obj)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("mAtlas");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
		NGUITools.SetDirty(serializedObject.targetObject);
		NGUISettings.atlas = obj as UIAtlas;
	}

	/// <summary>
	/// Sprite selection callback function.
	/// </summary>

	void SelectSprite (string spriteName)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("mSpriteName");
		sp.stringValue = spriteName;
		serializedObject.ApplyModifiedProperties();
		NGUITools.SetDirty(serializedObject.targetObject);
		NGUISettings.selectedSprite = spriteName;
	}

    void SelectSideSprite(string spriteName)
    {
        serializedObject.Update();
        SerializedProperty sp = serializedObject.FindProperty("mSideSpriteName");
        sp.stringValue = spriteName;
        serializedObject.ApplyModifiedProperties();
        NGUITools.SetDirty(serializedObject.targetObject);
        NGUISettings.selectedSprite = spriteName;
    }

    /// <summary>
    /// Draw the atlas and sprite selection fields.
    /// </summary>

    protected override bool ShouldDrawProperties ()
	{
		GUILayout.BeginHorizontal();
		if (NGUIEditorTools.DrawPrefixButton("Atlas"))
			ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
		SerializedProperty atlas = NGUIEditorTools.DrawProperty("", serializedObject, "mAtlas", GUILayout.MinWidth(20f));
		
		if (GUILayout.Button("Edit", GUILayout.Width(40f)))
		{
			if (atlas != null)
			{
				UIAtlas atl = atlas.objectReferenceValue as UIAtlas;
				NGUISettings.atlas = atl;
				if (atl != null) NGUIEditorTools.Select(atl.gameObject);
			}
		}
		GUILayout.EndHorizontal();

		SerializedProperty sp = serializedObject.FindProperty("mSpriteName");
		NGUIEditorTools.DrawAdvancedSpriteField(atlas.objectReferenceValue as UIAtlas, sp.stringValue, SelectSprite, false);

        UIPolygonSprite sprite = target as UIPolygonSprite;

        GUILayout.BeginHorizontal();
        SerializedProperty fill = serializedObject.FindProperty("mFillCenter");
        fill.boolValue = EditorGUILayout.Toggle("Fill", fill.boolValue, GUILayout.Width(95f));
        if (!fill.boolValue)
        {
            SerializedProperty thickness = serializedObject.FindProperty("mThickness");
            thickness.intValue = EditorGUILayout.IntSlider("Thickness", thickness.intValue, 1, sprite.width / 2);
        }
        GUILayout.EndHorizontal();

        SerializedProperty test = serializedObject.FindProperty("mTest");
        test.boolValue = EditorGUILayout.Toggle("SliceTest", test.boolValue, GUILayout.Width(95f));

        SerializedProperty spSide = serializedObject.FindProperty("mSideSpriteName");
        NGUIEditorTools.DrawAdvancedSpriteField(atlas.objectReferenceValue as UIAtlas, spSide.stringValue, SelectSideSprite, false);

        SerializedProperty sides = serializedObject.FindProperty("mPolygonSides");
        sides.intValue = EditorGUILayout.IntSlider("Sides", sides.intValue, 3, 360);
        
        SerializedProperty verticesDistance = serializedObject.FindProperty("mVerticesDistances");
        SerializedProperty spriteSides = serializedObject.FindProperty("mSpriteSides");

        if (verticesDistance.arraySize != sides.intValue + 1)
        {
            verticesDistance.ClearArray();
            verticesDistance.arraySize = sides.intValue + 1;
            for (int i = 0; i < sides.intValue; i++)
            {
                verticesDistance.GetArrayElementAtIndex(i).floatValue = 1f;
            }

            if (test.boolValue)
            {
                for (int i = 0; i < spriteSides.arraySize; ++i)
                {
                    UISprite spriteSide = spriteSides.GetArrayElementAtIndex(i).objectReferenceValue as UISprite;
                    if (spriteSide != null)
                    {
                        NGUITools.Destroy(spriteSide.gameObject);
                        spriteSide = null;
                    }
                }

                spriteSides.ClearArray();
                spriteSides.arraySize = sides.intValue;

                for (int i = 0; i < sides.intValue; ++i)
                {
                    if (spSide.stringValue.Length > 0)
                    {
                        UISprite spriteSide = NGUITools.AddSprite(sprite.gameObject, sprite.atlas, spSide.stringValue, sprite.depth + 1);
                        spriteSide.MakePixelPerfect();

                        spriteSides.GetArrayElementAtIndex(i).objectReferenceValue = spriteSide;
                    }
                }
            }
        }

        if (NGUIEditorTools.DrawMinimalisticHeader("Distance"))
        {
            NGUIEditorTools.BeginContents(true);

            for (int i = 0; i < sides.intValue; i++)
            {
                verticesDistance.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.Slider("Distances " + i, verticesDistance.GetArrayElementAtIndex(i).floatValue, 0, 1);
                if (test.boolValue)
                {
                    if (sprite.width * verticesDistance.GetArrayElementAtIndex(i).floatValue < sprite.minWidth)
                    {
                        verticesDistance.GetArrayElementAtIndex(i).floatValue = sprite.minWidth / (float)sprite.width;
                    }
                }
            }

            NGUIEditorTools.EndContents();
        }

        // last vertex is also the first!
        verticesDistance.GetArrayElementAtIndex(sides.intValue).floatValue = verticesDistance.GetArrayElementAtIndex(0).floatValue;

        return true;
	}

	/// <summary>
	/// All widgets have a preview.
	/// </summary>

	public override bool HasPreviewGUI ()
	{
		return (Selection.activeGameObject == null || Selection.gameObjects.Length == 1);
	}

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>

	public override void OnPreviewGUI (Rect rect, GUIStyle background)
	{
		UIPolygonSprite sprite = target as UIPolygonSprite;
		if (sprite == null || !sprite.isValid) return;

		Texture2D tex = sprite.mainTexture as Texture2D;
		if (tex == null) return;

		UISpriteData sd = sprite.atlas.GetSprite(sprite.spriteName);
		NGUIEditorTools.DrawSprite(tex, rect, sd, sprite.color);
	}
}
