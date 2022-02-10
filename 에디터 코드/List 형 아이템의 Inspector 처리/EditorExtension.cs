/**---------------------------------------------------------------------------------
 * @file EditorExtension.cs
 * @date 2022/2/9
 * @author sejong
 * @brief 유니디 에디터용 익스텐션
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// @class EditorExtension
/// @date 2022/2/9
/// @author sejong
/// @brief 유니디 에디터용 익스텐션 클레스
/// </summary>
public class EditorExtension : Editor
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// List 형 아이템의 임의 정렬을 위한 헤더 버튼 상태
	/// </summary>
	public enum eListButtonState
	{
		NONE = -1,

		MOVE_UP = 0,
		MOVE_DOWN = 1,
		ADD = 2,
		REMOVE = 3,
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// List 형 아이템의 임의 정렬을 위한 헤더 버튼 그리기 및 이벤트
	/// </summary>
	/// <returns></returns>
	public static eListButtonState DrawListItemHeaderButtons()
	{
		#region Layout
		int buttonSpacer = 6;

		EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 100 ) );
		// The up arrow will move things towards the beginning of the List
		var upArrow = '\u25B2'.ToString();
		bool upPressed = GUILayout.Button(new GUIContent(upArrow, "Click to shift up"),
										  EditorStyles.toolbarButton);

		// The down arrow will move things towards the end of the List
		var dnArrow = '\u25BC'.ToString();
		bool downPressed = GUILayout.Button(new GUIContent(dnArrow, "Click to shift down"),
											EditorStyles.toolbarButton);

		// A little space between button groups
		GUILayout.Space( buttonSpacer );

		// Remove Button - Process presses later
		bool removePressed = GUILayout.Button(new GUIContent("-", "Click to remove"),
											  EditorStyles.toolbarButton);

		// Add button - Process presses later
		bool addPressed = GUILayout.Button(new GUIContent("+", "Click to insert new"),
										   EditorStyles.toolbarButton);

		EditorGUILayout.EndHorizontal();
		#endregion Layout

		// Return the pressed button if any
		if( upPressed == true ) return eListButtonState.MOVE_UP;
		if( downPressed == true ) return eListButtonState.MOVE_DOWN;
		if( removePressed == true ) return eListButtonState.REMOVE;
		if( addPressed == true ) return eListButtonState.ADD;

		return eListButtonState.NONE;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// List 형 아이템의 관리를 위한 헤더 버튼 처리
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="index"></param>
	/// <param name="itemList"></param>
	/// <param name="pressedButton"></param>
	public static void UpdateListItemHeaderButtons<T>( int index, List<T> itemList, eListButtonState pressedButton )
	{
		// Don't allow 'up' presses for the first list item
		switch( pressedButton )
		{
			case eListButtonState.NONE: // Nothing was pressed, do nothing
				break;

			case eListButtonState.MOVE_UP:
				if( index > 0 )
				{
					T shiftItem = itemList[index];
					itemList.RemoveAt( index );
					itemList.Insert( index - 1, shiftItem );
				}
				break;

			case eListButtonState.MOVE_DOWN:
				// Don't allow 'down' presses for the last list item
				if( index + 1 < itemList.Count )
				{
					T shiftItem = itemList[index];
					itemList.RemoveAt( index );
					itemList.Insert( index + 1, shiftItem );
				}
				break;

			case eListButtonState.REMOVE:
				itemList.RemoveAt( index );
				break;

			case eListButtonState.ADD:
				itemList.Insert( index, default( T ) );
				break;
		}

	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 리플렉션을 통한 인자 드로잉
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="instance"></param>
	/// <param name="collapseBools"></param>
	public static void SerializedObjectFields<T>( T instance, bool collapseBools )
	{
		// get all public properties of T to see if there is one called 'name'
		System.Reflection.FieldInfo[] fields = typeof(T).GetFields();

		bool boolCollapseState = false;  // False until bool is found
		string boolCollapseName = "";    // The name of the last bool member
		string currentMemberName = "";   // The name of the member being processed

		// Display Fields Dynamically
		foreach( System.Reflection.FieldInfo fieldInfo in fields )
		{
			if( !collapseBools )
			{
				FieldInfoField<T>( instance, fieldInfo );
				continue;
			}

			// USING collapseBools...
			currentMemberName = fieldInfo.Name;

			// If this is a bool. Add the field and set collapse to true until  
			//   the end or until another bool is hit
			if( fieldInfo.FieldType == typeof( bool ) )
			{
				// If the first 4 letters of this bool match the last one, include this
				//   in the collapse group, rather than starting a new one.
				if( boolCollapseName.Length > 4 &&
					currentMemberName.StartsWith( boolCollapseName.Substring( 0, 4 ) ) )
				{
					if( !boolCollapseState ) FieldInfoField<T>( instance, fieldInfo );
					continue;
				}

				FieldInfoField<T>( instance, fieldInfo );


				boolCollapseName = currentMemberName;
				boolCollapseState = !( bool )fieldInfo.GetValue( instance );
			}
			else
			{
				// Add the field unless collapse is true
				if( !boolCollapseState ) FieldInfoField<T>( instance, fieldInfo );
			}

		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Uses a System.Reflection.FieldInfo to add a field
	/// Handles most built-in types and components
	/// includes automatic naming like the inspector does 
	/// by default
	/// </summary>
	/// <param name="fieldInfo"></param>
	public static void FieldInfoField<T>( T instance, System.Reflection.FieldInfo fieldInfo )
	{
		string label = fieldInfo.Name.DeCamel();

		#region Built-in Data Types
		if( fieldInfo.FieldType == typeof( string ) )
		{
			var val = (string)fieldInfo.GetValue(instance);
			val = EditorGUILayout.TextField( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( int ) )
		{
			var val = (int)fieldInfo.GetValue(instance);
			val = EditorGUILayout.IntField( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( float ) )
		{
			var val = (float)fieldInfo.GetValue(instance);
			val = EditorGUILayout.FloatField( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( bool ) )
		{
			var val = (bool)fieldInfo.GetValue(instance);
			val = EditorGUILayout.Toggle( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		#endregion Built-in Data Types

		#region Basic Unity Types
		else if( fieldInfo.FieldType == typeof( GameObject ) )
		{
			var val = (GameObject)fieldInfo.GetValue(instance);
			val = ObjectField<GameObject>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( Transform ) )
		{
			var val = (Transform)fieldInfo.GetValue(instance);
			val = ObjectField<Transform>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( Rigidbody ) )
		{
			var val = (Rigidbody)fieldInfo.GetValue(instance);
			val = ObjectField<Rigidbody>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( Renderer ) )
		{
			var val = (Renderer)fieldInfo.GetValue(instance);
			val = ObjectField<Renderer>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( Mesh ) )
		{
			var val = (Mesh)fieldInfo.GetValue(instance);
			val = ObjectField<Mesh>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( Vector3 ) )
		{
			var val = (Vector3)fieldInfo.GetValue(instance);
			val = EditorGUILayout.Vector3Field( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		#endregion Basic Unity Types

		#region Unity Collider Types
		else if( fieldInfo.FieldType == typeof( BoxCollider ) )
		{
			var val = (BoxCollider)fieldInfo.GetValue(instance);
			val = ObjectField<BoxCollider>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( SphereCollider ) )
		{
			var val = (SphereCollider)fieldInfo.GetValue(instance);
			val = ObjectField<SphereCollider>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( CapsuleCollider ) )
		{
			var val = (CapsuleCollider)fieldInfo.GetValue(instance);
			val = ObjectField<CapsuleCollider>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( MeshCollider ) )
		{
			var val = (MeshCollider)fieldInfo.GetValue(instance);
			val = ObjectField<MeshCollider>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		else if( fieldInfo.FieldType == typeof( WheelCollider ) )
		{
			var val = (WheelCollider)fieldInfo.GetValue(instance);
			val = ObjectField<WheelCollider>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		#endregion Unity Collider Types

		#region Other Unity Types
		else if( fieldInfo.FieldType == typeof( CharacterController ) )
		{
			var val = (CharacterController)fieldInfo.GetValue(instance);
			val = ObjectField<CharacterController>( label, val );
			fieldInfo.SetValue( instance, val );
			return;
		}
		#endregion Other Unity Types
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// A generic version of EditorGUILayout.ObjectField.
	/// Allows objects to be drag and dropped or picked.
	/// This version defaults to 'allowSceneObjects = true'.
	/// 
	/// Instead of this:
	///     var script = (MyScript)target;
	///     script.transform = (Transform)EditorGUILayout.ObjectField("My Transform", script.transform, typeof(Transform), true);        
	/// 
	/// Do this:    
	///     var script = (MyScript)target;
	///     script.transform = EditorGUILayout.ObjectField<Transform>("My Transform", script.transform);        
	/// </summary>
	/// <typeparam name="T">The type of object to use</typeparam>
	/// <param name="label">The label (text) to show to the left of the field</param>
	/// <param name="obj">The obj variable of the script this GUI field is for</param>
	/// <returns>A reference to what is in the field. An object or null.</returns>

	public static T ObjectField<T>( string label, T obj ) where T : UnityEngine.Object
	{
		return ObjectField<T>( label, obj, true );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// A generic version of EditorGUILayout.ObjectField.
	/// Allows objects to be drag and dropped or picked.
	/// </summary>
	/// <typeparam name="T">The type of object to use</typeparam>
	/// <param name="label">The label (text) to show to the left of the field</param>
	/// <param name="obj">The obj variable of the script this GUI field is for</param>
	/// <param name="allowSceneObjects">Allow scene objects. See Unity Docs for more.</param>
	/// <returns>A reference to what is in the field. An object or null.</returns>
	public static T ObjectField<T>( string label, T obj, bool allowSceneObjects )
			where T : UnityEngine.Object
	{
		return ( T )EditorGUILayout.ObjectField( label, obj, typeof( T ), allowSceneObjects );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//class EditorExtension
