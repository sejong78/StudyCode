/**---------------------------------------------------------------------------------
 * @file DontDestroyOnLoad.cs
 * @date 2022/7/30
 * @author sejong
 * @brief 씬 이동시 삭제 하지 않을 게임오브젝트에 붙여줄 컴포넌트
 *///-------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif//UNITY_EDITOR

/// <summary>
/// @class DontDestroyOnLoad
/// @date 2022/7/30
/// @author sejong
/// @brief 씬 이동시 삭제 하지 않을 게임오브젝트에 붙여줄 컴포넌트
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad( gameObject );
	}

}//DontDestroyOnLoad

#if UNITY_EDITOR
/// <summary>
/// @class Editor_DontDestroyOnLoad
/// @date 2022/7/30
/// @author sejong
/// @brief 씬 이동시 삭제 하지 않을 게임오브젝트에 붙여줄 컴포넌트 에디터
/// </summary>
[CustomEditor( typeof( DontDestroyOnLoad ) )]
public class Editor_DontDestroyOnLoad : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField( "해당 컴포넌트가 붙어 있는 게임 오브젝트는 씬 이동시 자동으로 삭제 되지 않습니다." );
	}

}//Editor_DontDestroyOnLoad
#endif//UNITY_EDITOR
