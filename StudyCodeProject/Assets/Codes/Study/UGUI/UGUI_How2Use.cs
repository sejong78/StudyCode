/**---------------------------------------------------------------------------------
 * @file UGUI_How2Use.cs
 * @date 2022/10/29
 * @author sejong
 * @brief UGUI 의 테스트용 클레스 
 *///-------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// @class UGUI_How2Use
/// @date 2022/10/29
/// @author sejong
/// @brief UGUI 의 테스트용 클레스 
/// </summary>
public class UGUI_How2Use : MonoBehaviour
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	[SerializeField]
	private SpriteAtlas _atlas = null;

	[SerializeField]
	private Transform _contentRoot = null;

	[SerializeField]
	private GameObject _listItemPrefab = null;

	string[] _imgNames = { "LBO_Button_Block_Down", "LBO_Button_Control_Dodge_Active" };

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	private void Start()
	{
		UIListItem item = null;
		Sprite img = null;

		for( int i = 0; i < 10; ++i )
		{
			item = Instantiate( _listItemPrefab, _contentRoot ).GetComponent<UIListItem>();
			img = _atlas.GetSprite( _imgNames[ i % 2 ] );
			item.Init( img );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// uGui 역시 public 으로 선언해서 이벤트를 등록한다.
	/// </summary>
	public void OnClick_Start()
	{
		Debug.LogFormat( "[UGUI_How2Use.OnClick_Start] Clicked" );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public void OnClick_Quit()
	{

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else//UNITY_EDITOR
		Application.Quit();
#endif//UNITY_EDITOR
	
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public void OnClick_ImageButton()
	{
		Debug.Log( $"[UGUI_How2Use.OnClick_ImageButton] Clicked" );
	}


	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//UGUI_How2Use
