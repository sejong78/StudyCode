/**---------------------------------------------------------------------------------
 * @file UGUI_How2Use.cs
 * @date 2022/10/29
 * @author sejong
 * @brief UGUI 의 테스트용 클레스 
 *///-------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	//@@-------------------------------------------------------------------------------------------------------------------------

}//UGUI_How2Use
