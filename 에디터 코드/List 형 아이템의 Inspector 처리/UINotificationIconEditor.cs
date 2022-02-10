using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Champion.UI;

[CustomEditor( typeof( UINotificationIcon ) )]
public class UINotificationIconEditor : Editor
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	private UINotificationIcon _target;

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	void OnEnable()
	{
		_target = ( UINotificationIcon )target;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();

		DrawList( _target.items );

		if( GUI.changed )
			EditorUtility.SetDirty( _target );

	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private void DrawList( List<NotificationIcon> listData )
	{
		#region List item
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();

		for( int i = 0; i < listData.Count; ++i )
		{
			EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

			// 리스트 아이템 헤더의 컨트롤 버튼이 눌렸다면
			EditorExtension.eListButtonState listButtonPressed = EditorExtension.DrawListItemHeaderButtons();

			EditorGUILayout.EndHorizontal();

			// 헤더의 컨트롤 버튼 상태 처리
			EditorExtension.UpdateListItemHeaderButtons( i, listData, listButtonPressed );

			// 뭔가 했다면 다른 동작은 무시하도록 리턴한다.
			if( listButtonPressed != EditorExtension.eListButtonState.NONE )
				return;

			EditorGUILayout.BeginHorizontal();
			GUILayout.BeginVertical( "box" );

			// 실제 리스트 아이템의 내용을 그린다.
			DrawListItem( listData[ i ] );

			GUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

		}

		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		#endregion//List item


		#region Add Button
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal( EditorStyles.toolbar );

		if( GUILayout.Button( new GUIContent( "+", "Click to add" ), EditorStyles.toolbarButton ) )
		{
			listData.Add( new NotificationIcon() );
		}

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		#endregion//Add Button
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	private void DrawListItem( NotificationIcon item )
	{
		EditorExtension.SerializedObjectFields<NotificationIcon>( item, false );
		// enum 은 상황에 따라 EnumMaskField 와 EnumFlagsField 을 알아서 세팅해야 하므로
		// EditorExtension.SerializedObjectFields 에 관련 세팅이 정의 되어 있지 않아서 별도로 그린다.
		item.resetFlag = ( ResetFlag )EditorGUILayout.EnumFlagsField( "ResetFlag", item.resetFlag );

		GUILayout.Space( 2 );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//class UINotificationIconEditor
