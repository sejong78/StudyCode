using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UGUI_How2Use : MonoBehaviour
{
	public void OnClick_Start()
	{
		Debug.LogFormat( "[UGUI_How2Use.OnClick_Start] Clicked" );
	}

	public void OnClick_Quit()
	{

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else//UNITY_EDITOR
		Application.Quit();
#endif//UNITY_EDITOR
	
	}

}//UGUI_How2Use
