/**---------------------------------------------------------------------------------
 * @file UIListDataEditor.cs
 * @date 2020/1/8
 * @author sejong
 * @brief 리스트형 데이터를 관리하는 컴포넌트용 에디터
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// @class UIListDataEditor
/// @date 2020/1/8
/// @author sejong
/// @brief 리스트형 데이터를 관리하는 컴포넌트용 에디터 클레스
/// </summary>
public class UIListDataEditor<T> : Editor
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Used by AddFoldOutListItemButtons to return which button was pressed, and by 
	/// UpdateFoldOutListOnButtonPressed to process the pressed button for regular lists
	/// </summary>
	public enum EListButtons { None, Up, Down, Add, Remove }

	/// <summary>
	/// 관리할 데이터 리스트를 가져온다.
	/// </summary>
	/// <returns></returns>
	public List<T> GetListData()
	{
		return null;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		//DrawSceneData();

// 		if( GUI.changed )
// 			EditorUtility.SetDirty( m_Data );

	}

/*
	private void DrawSceneData(  )
	{
		List<T> _data = GetListData();

		if( _data == null )
		{
			return;
		}

		#region Header Foldout
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

		// BUTTONS...
		EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 100 ) );
		if( _data.Count > 0 )
		{
			//리스트 축소
			GUIContent content;
			var collapseIcon = '\u2261'.ToString();
			content = new GUIContent( collapseIcon, "Click to collapse all" );
			m_SceneDataMasterCollapse = GUILayout.Button( content, EditorStyles.toolbarButton );
			//리스트 확장
			var expandIcon = '\u25A1'.ToString();
			content = new GUIContent( expandIcon, "Click to expand all" );
			m_SceneDataMasterExpand = GUILayout.Button( content, EditorStyles.toolbarButton );
		}
		else
		{
			GUILayout.FlexibleSpace();
		}

		EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 50 ) );
		// A little space between button groups
		GUILayout.Space( m_ButtonSpacer );

		// Main Add button
		if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
			_data.Add( new DCSceneData() );
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		#endregion

		#region List Items
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space( m_Blank );
		EditorGUILayout.BeginVertical();

		for( int i = 0; i < _data.Count; i++ )
		{
			#region Section Header

			if( !m_editorSceneDataListItemStates.TryGetValue( _data[ i ], out m_SceneDataListfoldOutState ) )
			{
				m_editorSceneDataListItemStates[ _data[ i ] ] = true;
				m_SceneDataListfoldOutState = false;
			}

			// Force states if master buttons were pressed
			if( m_SceneDataMasterCollapse ) m_SceneDataListfoldOutState = false;
			if( m_SceneDataMasterExpand ) m_SceneDataListfoldOutState = true;

			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
			m_SceneDataListfoldOutState = EditorGUILayout.Foldout( m_SceneDataListfoldOutState, _data[ i ].m_KeyName );
			m_editorSceneDataListItemStates[ _data[ i ] ] = m_SceneDataListfoldOutState;

			if( m_SceneDataListfoldOutState == false )
			{
				EditorGUILayout.EndHorizontal();
				continue;
			}

			ED_Util.EListButtons listButtonPressed = ED_Util.AddFoldOutListItemButtons();

			EditorGUILayout.EndHorizontal();

			#region Process List Changes
			// Don't allow 'up' presses for the first list item
			ED_Util.UpdateFoldOutListOnButtonPressed( _data, i, listButtonPressed );
			if( listButtonPressed != ED_Util.EListButtons.None )
				return;
			#endregion Process List Changes

			#endregion

			#region ActiveUI List Items
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space( m_Blank );
			GUILayout.BeginVertical( "box" );

			EditorGUILayout.BeginHorizontal();

			_data[ i ].m_KeyName = EditorGUILayout.TextField( "Key Name", _data[ i ].m_KeyName );

			if( GUILayout.Button( new GUIContent( "해당 내용 추가하기", "Click to insert data" ), EditorStyles.toolbarButton ) == true )
			{
				m_TempSceneData = new DCSceneData();
				m_TempSceneData.Copy( _data[ i ] );
				_data.Insert( i, m_TempSceneData );

				m_TempSceneData = null;
			}

			EditorGUILayout.EndHorizontal();

			//_data[i].m_KeyName = EditorGUILayout.TextField("Key Name", _data[i].m_KeyName);

			#region ActiveUI List Header Foldout
			DrawActiveUIList( _data[ i ] );
			#endregion

			GUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			#endregion

			#region UICtr List Header Foldout
			DrawUICtrlList( _data[ i ] );
			#endregion

			#region UIEffect List Header Foldout
			DrawUIEffectData( _data[ i ] );
			#endregion

		}

		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		#endregion

	}
*/

	/*
		private void DrawSceneData( List<DCSceneData> _data )
		{

			#region Header Foldout
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

			if( m_editorChildSceneDataListItemStates.ContainsKey( _data ) == false )
				m_editorChildSceneDataListItemStates[ _data ] = true;

			m_editorChildSceneDataListItemStates[ _data ] = EditorGUILayout.Foldout( m_editorChildSceneDataListItemStates[ _data ], "SceneData" );

			if( m_editorChildSceneDataListItemStates[ _data ] == false )
			{
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
				return;
			}

			// BUTTONS...
			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 100 ) );
			if( _data.Count > 0 )
			{
				//리스트 축소
				GUIContent content;
				var collapseIcon = '\u2261'.ToString();
				content = new GUIContent( collapseIcon, "Click to collapse all" );
				m_SceneDataMasterCollapse = GUILayout.Button( content, EditorStyles.toolbarButton );
				//리스트 확장
				var expandIcon = '\u25A1'.ToString();
				content = new GUIContent( expandIcon, "Click to expand all" );
				m_SceneDataMasterExpand = GUILayout.Button( content, EditorStyles.toolbarButton );
			}
			else
			{
				GUILayout.FlexibleSpace();
			}

			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 50 ) );
			// A little space between button groups
			GUILayout.Space( m_ButtonSpacer );

			// Main Add button
			if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
				_data.Add( new DCSceneData() );
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			#endregion

			#region List Items
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space( m_Blank );
			EditorGUILayout.BeginVertical();

			for( int i = 0; i < _data.Count; i++ )
			{
				#region Section Header

				if( !m_editorSceneDataListItemStates.TryGetValue( _data[ i ], out m_SceneDataListfoldOutState ) )
				{
					m_editorSceneDataListItemStates[ _data[ i ] ] = true;
					m_SceneDataListfoldOutState = false;
				}

				// Force states if master buttons were pressed
				if( m_SceneDataMasterCollapse ) m_SceneDataListfoldOutState = false;
				if( m_SceneDataMasterExpand ) m_SceneDataListfoldOutState = true;

				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
				m_SceneDataListfoldOutState = EditorGUILayout.Foldout( m_SceneDataListfoldOutState, _data[ i ].m_KeyName );
				m_editorSceneDataListItemStates[ _data[ i ] ] = m_SceneDataListfoldOutState;

				if( m_SceneDataListfoldOutState == false )
				{
					EditorGUILayout.EndHorizontal();
					continue;
				}

				ED_Util.EListButtons listButtonPressed = ED_Util.AddFoldOutListItemButtons();

				EditorGUILayout.EndHorizontal();

				#region Process List Changes
				// Don't allow 'up' presses for the first list item
				ED_Util.UpdateFoldOutListOnButtonPressed( _data, i, listButtonPressed );
				if( listButtonPressed != ED_Util.EListButtons.None )
					return;
				#endregion Process List Changes

				#endregion

				#region ActiveUI List Items
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space( m_Blank );
				GUILayout.BeginVertical( "box" );

				EditorGUILayout.BeginHorizontal();

				_data[ i ].m_KeyName = EditorGUILayout.TextField( "Key Name", _data[ i ].m_KeyName );

				if( GUILayout.Button( new GUIContent( "해당 내용 추가하기", "Click to insert data" ), EditorStyles.toolbarButton ) == true )
				{
					m_TempSceneData = new DCSceneData();
					m_TempSceneData.Copy( _data[ i ] );
					_data.Insert( i, m_TempSceneData );

					m_TempSceneData = null;
				}

				EditorGUILayout.EndHorizontal();

				//_data[i].m_KeyName = EditorGUILayout.TextField("Key Name", _data[i].m_KeyName);

				#region ActiveUI List Header Foldout
				DrawActiveUIList( _data[ i ] );
				#endregion

				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				#endregion

				#region UICtr List Header Foldout
				DrawUICtrlList( _data[ i ] );
				#endregion

				#region UIEffect List Header Foldout
				DrawUIEffectData( _data[ i ] );
				#endregion

			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			#endregion

		}

		private void DrawActiveUIList( DCSceneData _data )
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

			if( !m_editorActiveUILListItemStates.TryGetValue( _data, out m_ActiveUIListfoldOutState ) )
			{
				m_editorActiveUILListItemStates[ _data ] = true;
				m_ActiveUIListfoldOutState = false;
			}

			// Force states if master buttons were pressed
			if( m_SceneDataMasterCollapse ) m_ActiveUIListfoldOutState = false;
			if( m_SceneDataMasterExpand ) m_ActiveUIListfoldOutState = true;

			m_ActiveUIListfoldOutState = EditorGUILayout.Foldout( m_ActiveUIListfoldOutState, "ActiveUIList" );
			m_editorActiveUILListItemStates[ _data ] = m_ActiveUIListfoldOutState;

			if( m_ActiveUIListfoldOutState == false )
			{
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
				return;
			}

			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 100 ) );
			if( _data.m_ActiveUIList.Count > 0 )
			{
				//리스트 축소
				GUIContent content;
				var collapseIcon = '\u2261'.ToString();
				content = new GUIContent( collapseIcon, "Click to collapse all" );
				m_SceneDataMasterCollapse = GUILayout.Button( content, EditorStyles.toolbarButton );
				//리스트 확장
				var expandIcon = '\u25A1'.ToString();
				content = new GUIContent( expandIcon, "Click to expand all" );
				m_SceneDataMasterExpand = GUILayout.Button( content, EditorStyles.toolbarButton );
			}
			else
			{
				GUILayout.FlexibleSpace();
			}

			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 50 ) );
			// A little space between button groups
			GUILayout.Space( m_ButtonSpacer );

			// Main Add button
			if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
				_data.m_ActiveUIList.Add( new DCAttachUIData() );
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();


			for( int j = 0; j < _data.m_ActiveUIList.Count; j++ )
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space( m_Blank );
				GUILayout.BeginVertical( "box" );
				if( !m_editorActiveUILListDataItemStates.TryGetValue( _data.m_ActiveUIList[ j ], out m_ActiveUIListDatafoldOutState ) )
				{
					m_editorActiveUILListDataItemStates[ _data.m_ActiveUIList[ j ] ] = true;
					m_ActiveUIListDatafoldOutState = false;
				}

				// Force states if master buttons were pressed
				if( m_SceneDataMasterCollapse ) m_ActiveUIListDatafoldOutState = false;
				if( m_SceneDataMasterExpand ) m_ActiveUIListDatafoldOutState = true;

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
				m_ActiveUIListDatafoldOutState = EditorGUILayout.Foldout( m_ActiveUIListDatafoldOutState, _data.m_ActiveUIList[ j ].m_Name );
				m_editorActiveUILListDataItemStates[ _data.m_ActiveUIList[ j ] ] = m_ActiveUIListDatafoldOutState;
				ED_Util.EListButtons listActiveUIButtonPressed = ED_Util.AddFoldOutListItemButtons();

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();

				if( m_ActiveUIListDatafoldOutState == true )
				{
					// Display Fields for the list instance
					ED_Util.SerializedObjectFields<DCAttachUIData>( _data.m_ActiveUIList[ j ] );
					GUILayout.Space( 2 );
				}

				#region Process ActiveUI List Changes
				// Don't allow 'up' presses for the first list item
				ED_Util.UpdateFoldOutListOnButtonPressed( _data.m_ActiveUIList, j, listActiveUIButtonPressed );
				#endregion Process tActiveUI List Changes

				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
		}

		private void DrawUICtrlList( DCSceneData _data )
		{
			#region UICtr List Header Foldout
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space( m_Blank );
			GUILayout.BeginVertical( "box" );

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

			if( !m_editorUICtrLListItemStates.TryGetValue( _data, out m_UICtrListfoldOutState ) )
			{
				m_editorUICtrLListItemStates[ _data ] = true;
				m_UICtrListfoldOutState = false;
			}

			// Force states if master buttons were pressed
			if( m_SceneDataMasterCollapse ) m_UICtrListfoldOutState = false;
			if( m_SceneDataMasterExpand ) m_UICtrListfoldOutState = true;

			m_UICtrListfoldOutState = EditorGUILayout.Foldout( m_UICtrListfoldOutState, "UICtrList" );
			m_editorUICtrLListItemStates[ _data ] = m_UICtrListfoldOutState;

			if( m_UICtrListfoldOutState == false )
			{
				EditorGUILayout.EndHorizontal();
				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
				return;
			}

			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 50 ) );
			// A little space between button groups
			GUILayout.Space( m_ButtonSpacer );

			// Main Add button
			if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
				_data.m_UICtrData.Add( new DCUICtrData() );
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			#endregion

			for( int j = 0; j < _data.m_UICtrData.Count; j++ )
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space( m_Blank );
				GUILayout.BeginVertical( "box" );
				if( !m_editorActiveUILListDataItemStates.TryGetValue( _data.m_UICtrData[ j ], out m_UICtrListDatafoldOutState ) )
				{
					m_editorActiveUILListDataItemStates[ _data.m_UICtrData[ j ] ] = true;
					m_UICtrListDatafoldOutState = true;
				}

				// Force states if master buttons were pressed
				if( m_SceneDataMasterCollapse ) m_UICtrListDatafoldOutState = false;
				if( m_SceneDataMasterExpand ) m_UICtrListDatafoldOutState = true;

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
				m_UICtrListDatafoldOutState = EditorGUILayout.Foldout( m_UICtrListDatafoldOutState, _data.m_UICtrData[ j ].m_UICtrName );
				m_editorActiveUILListDataItemStates[ _data.m_UICtrData[ j ] ] = m_UICtrListDatafoldOutState;
				ED_Util.EListButtons listUICtrButtonPressed = ED_Util.AddFoldOutListItemButtons();

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();

				if( m_UICtrListDatafoldOutState == true )
				{
					// Display Fields for the list instance
					ED_Util.SerializedObjectFields<DCUICtrData>( _data.m_UICtrData[ j ] );
					GUILayout.Space( 2 );
				}

				#region Process UICtr List Changes
				// Don't allow 'up' presses for the first list item
				ED_Util.UpdateFoldOutListOnButtonPressed( _data.m_UICtrData, j, listUICtrButtonPressed );
				#endregion Process UICtr List Changes

				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		private void DrawUIEffectData( DCSceneData _data )
		{
			#region UICtr List Header Foldout
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space( m_Blank );
			GUILayout.BeginVertical( "box" );

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

			if( !m_editorUIEffectDataListDataItemStates.TryGetValue( _data, out m_UIEffectListfoldOutState ) )
			{
				m_editorUIEffectDataListDataItemStates[ _data ] = true;
				m_UIEffectListfoldOutState = false;
			}

			// Force states if master buttons were pressed
			if( m_SceneDataMasterCollapse ) m_UIEffectListfoldOutState = false;
			if( m_SceneDataMasterExpand ) m_UIEffectListfoldOutState = true;

			m_UIEffectListfoldOutState = EditorGUILayout.Foldout( m_UIEffectListfoldOutState, "UIEffectDataList" );
			m_editorUIEffectDataListDataItemStates[ _data ] = m_UIEffectListfoldOutState;

			if( m_UIEffectListfoldOutState == false )
			{
				EditorGUILayout.EndHorizontal();
				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
				return;
			}

			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 50 ) );
			// A little space between button groups
			GUILayout.Space( m_ButtonSpacer );

			// Main Add button
			if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
				_data.m_UIEffectData.Add( new DCUIEffecData() );
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			#endregion

			for( int j = 0; j < _data.m_UIEffectData.Count; j++ )
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space( m_Blank );
				GUILayout.BeginVertical( "box" );
				if( !m_editorActiveUILListDataItemStates.TryGetValue( _data.m_UIEffectData[ j ], out m_UIEffectListDatafoldOutState ) )
				{
					m_editorActiveUILListDataItemStates[ _data.m_UIEffectData[ j ] ] = true;
					m_UIEffectListDatafoldOutState = true;
				}

				// Force states if master buttons were pressed
				if( m_SceneDataMasterCollapse ) m_UIEffectListDatafoldOutState = false;
				if( m_SceneDataMasterExpand ) m_UIEffectListDatafoldOutState = true;

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
				m_UIEffectListDatafoldOutState = EditorGUILayout.Foldout( m_UIEffectListDatafoldOutState, _data.m_UIEffectData[ j ].m_EffectName );
				m_editorActiveUILListDataItemStates[ _data.m_UIEffectData[ j ] ] = m_UIEffectListDatafoldOutState;
				ED_Util.EListButtons listUICtrButtonPressed = ED_Util.AddFoldOutListItemButtons();

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();

				if( m_UIEffectListDatafoldOutState == true )
				{
					// Display Fields for the list instance
					ED_Util.SerializedObjectFields<DCUIEffecData>( _data.m_UIEffectData[ j ] );
					GUILayout.Space( 2 );
				}

				#region Process UICtr List Changes
				// Don't allow 'up' presses for the first list item
				ED_Util.UpdateFoldOutListOnButtonPressed( _data.m_UIEffectData, j, listUICtrButtonPressed );
				#endregion Process UICtr List Changes

				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		private void DrawUIEffectData( DCSceneGroupData _groupData, List<DCUIEffecData> _uiEffectData )
		{
			#region ActiveUI List Header Foldout
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

			if( !m_editorUIEffectDataLListItemStates.TryGetValue( _groupData, out m_SceneGroupDataUIEffectListfoldOutState ) )
			{
				m_editorUIEffectDataLListItemStates[ _groupData ] = true;
				m_SceneGroupDataUIEffectListfoldOutState = true;
			}

			// Force states if master buttons were pressed
			if( m_SceneGroupDataMasterCollapse ) m_SceneGroupDataUIEffectListfoldOutState = false;
			if( m_SceneGroupDataMasterExpand ) m_SceneGroupDataUIEffectListfoldOutState = true;

			m_SceneGroupDataUIEffectListfoldOutState = EditorGUILayout.Foldout( m_SceneGroupDataUIEffectListfoldOutState, "UI Effect Data" );
			m_editorUIEffectDataLListItemStates[ _groupData ] = m_SceneGroupDataUIEffectListfoldOutState;

			if( m_SceneGroupDataUIEffectListfoldOutState == false )
			{
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
				return;
			}

			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 50 ) );
			// A little space between button groups
			GUILayout.Space( m_ButtonSpacer );

			// Main Add button
			if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
				_uiEffectData.Add( new DCUIEffecData() );
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			#endregion

			for( int j = 0; j < _uiEffectData.Count; j++ )
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space( m_Blank );
				GUILayout.BeginVertical( "box" );
				if( !m_editorUIEffectDataListDataItemStates.TryGetValue( _uiEffectData[ j ], out m_SceneGroupDataUIEffectListDatafoldOutState ) )
				{
					m_editorUIEffectDataListDataItemStates[ _uiEffectData[ j ] ] = true;
					m_SceneGroupDataUIEffectListDatafoldOutState = true;
				}

				// Force states if master buttons were pressed
				if( m_SceneDataMasterCollapse ) m_SceneGroupDataUIEffectListDatafoldOutState = false;
				if( m_SceneDataMasterExpand ) m_SceneGroupDataUIEffectListDatafoldOutState = true;

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
				m_SceneGroupDataUIEffectListDatafoldOutState = EditorGUILayout.Foldout( m_SceneGroupDataUIEffectListDatafoldOutState, _uiEffectData[ j ].m_EffectName );
				m_editorUIEffectDataListDataItemStates[ _uiEffectData[ j ] ] = m_SceneGroupDataUIEffectListDatafoldOutState;
				ED_Util.EListButtons listActiveUIButtonPressed = ED_Util.AddFoldOutListItemButtons();

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();

				if( m_SceneGroupDataUIEffectListDatafoldOutState == true )
				{
					// Display Fields for the list instance
					ED_Util.SerializedObjectFields<DCUIEffecData>( _uiEffectData[ j ] );
					GUILayout.Space( 2 );
				}

				#region Process ActiveUI List Changes
				// Don't allow 'up' presses for the first list item
				ED_Util.UpdateFoldOutListOnButtonPressed( _uiEffectData, j, listActiveUIButtonPressed );
				#endregion Process tActiveUI List Changes

				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
		}

		private void DrawSceneGroupData()
		{

			#region Header Foldout
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
			m_SceneGroupDataListFolding = EditorGUILayout.Foldout( m_SceneGroupDataListFolding, "SceneGroupData" );

			if( m_SceneGroupDataListFolding == false )
			{
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
				return;
			}
			// BUTTONS...
			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 100 ) );
			if( m_Data.m_SceneData.Count > 0 )
			{
				//리스트 축소
				GUIContent content;
				var collapseIcon = '\u2261'.ToString();
				content = new GUIContent( collapseIcon, "Click to collapse all" );
				m_SceneGroupDataMasterCollapse = GUILayout.Button( content, EditorStyles.toolbarButton );
				//리스트 확장
				var expandIcon = '\u25A1'.ToString();
				content = new GUIContent( expandIcon, "Click to expand all" );
				m_SceneGroupDataMasterExpand = GUILayout.Button( content, EditorStyles.toolbarButton );
			}
			else
			{
				GUILayout.FlexibleSpace();
			}

			EditorGUILayout.BeginHorizontal( GUILayout.MaxWidth( 50 ) );
			// A little space between button groups
			GUILayout.Space( m_ButtonSpacer );

			// Main Add button
			if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
				m_Data.m_SceneGroupData.Add( new DCSceneGroupData() );
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndHorizontal();
			#endregion

			#region List Items
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space( m_Blank );
			EditorGUILayout.BeginVertical();

			for( int i = 0; i < m_Data.m_SceneGroupData.Count; i++ )
			{
				#region Section Header

				if( !m_editorSceneGroupDataListItemStates.TryGetValue( m_Data.m_SceneGroupData[ i ], out m_SceneGroupDataListfoldOutState ) )
				{
					m_editorSceneGroupDataListItemStates[ m_Data.m_SceneGroupData[ i ] ] = true;
					m_SceneGroupDataListfoldOutState = false;
				}

				// Force states if master buttons were pressed
				if( m_SceneGroupDataMasterCollapse ) m_SceneGroupDataListfoldOutState = false;
				if( m_SceneGroupDataMasterExpand ) m_SceneGroupDataListfoldOutState = true;

				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );
				m_SceneGroupDataListfoldOutState = EditorGUILayout.Foldout( m_SceneGroupDataListfoldOutState, m_Data.m_SceneGroupData[ i ].m_ESceneGroupID.ToString() );
				m_editorSceneGroupDataListItemStates[ m_Data.m_SceneGroupData[ i ] ] = m_SceneGroupDataListfoldOutState;

				if( m_SceneGroupDataListfoldOutState == false )
				{
					EditorGUILayout.EndHorizontal();
					continue;
				}

				ED_Util.EListButtons listButtonPressed = ED_Util.AddFoldOutListItemButtons();

				EditorGUILayout.EndHorizontal();

				#region Process List Changes
				// Don't allow 'up' presses for the first list item
				ED_Util.UpdateFoldOutListOnButtonPressed( m_Data.m_SceneGroupData, i, listButtonPressed );
				if( listButtonPressed != ED_Util.EListButtons.None )
					return;
				#endregion Process List Changes

				#endregion

				#region Data
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space( m_Blank );
				GUILayout.BeginVertical( "box" );
				m_Data.m_SceneGroupData[ i ].m_ESceneGroupID = ( GlobalEnum.ESceneGroupID )EditorGUILayout.EnumPopup( "SceneGroupID", m_Data.m_SceneGroupData[ i ].m_ESceneGroupID );
				m_Data.m_SceneGroupData[ i ].m_EMainSceneID = ( GlobalEnum.ESceneID )EditorGUILayout.EnumPopup( "MainSceneID", m_Data.m_SceneGroupData[ i ].m_EMainSceneID );

				if( GUILayout.Button( new GUIContent( "해당 내용 추가하기", "Click to insert data" ), EditorStyles.toolbarButton ) == true )
				{
					m_TempSceneGroupData = new DCSceneGroupData();
					m_TempSceneGroupData.Copy( m_Data.m_SceneGroupData[ i ] );
					m_Data.m_SceneGroupData.Insert( i, m_TempSceneGroupData );

					m_TempSceneGroupData = null;
				}

				#region ActiveUI List Items
				DrawUIEffectData( m_Data.m_SceneGroupData[ i ], m_Data.m_SceneGroupData[ i ].m_UIEffectData );
				#endregion

				GUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				#endregion
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			#endregion

		}

	*/
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//class UIListDataEditor


/// <summary>
/// 리스트로 보여줄 데이터 인터페이스
/// 에디터에서 관리할 리스트를 가져온다.
/// </summary>
/// <typeparam name="T">리스트로 보여줄 데이터 타입</typeparam>
public interface IListDataNodeBase<T>
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
		
	List<T> GetListData();

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
}//interface IListDataNodeBase

/// <summary>
/// 리스트 에디터를 사용할 스크립트는 이걸 상속받아야 한다.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListDataBase<T> : MonoBehaviour, IListDataNodeBase<T>
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 리스트 노드의 
	/// </summary>
	public List<T> GetListData()
	{
		return null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
}//class ListDataBase
