using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLintener_CharacterAnimation : MonoBehaviour
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	protected void OnEvent_AnimationStart( string aniName )
	{
		DebugExtensions.Log( $"[OnEvent_AnimationStart] ani = {aniName}", Color.white );
	}
	
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	protected void OnEvent_PlaySound( string soundName )
	{
		DebugExtensions.Log( $"[OnEvent_PlaySound] sound = {soundName}", Color.white );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// Animation의 Transitions 설정시 Exit 타임의 설정 상태에 따라 무시될 수 있으니 확인이 필요하다.
	/// </summary>
	/// <param name="aniName"></param>
	protected void OnEvent_AnimationFinish( string aniName )
	{
		DebugExtensions.Log( $"[OnEvent_AnimationFinish] ani = {aniName}", Color.white );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//EventLintener_CharacterAnimation
