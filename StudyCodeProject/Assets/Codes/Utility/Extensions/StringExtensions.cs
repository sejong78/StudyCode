/**---------------------------------------------------------------------------------
 * @file StringExtensions.cs
 * @date 2020/7/21
 * @author sejong
 * @brief 게임 내 스트링 관련 확장함수들의 관리
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @class StringExtensions
/// @date 2020/7/21
/// @author sejong
/// @brief 게임 내 스트링 관련 확장함수들의 관리 클레스
/// </summary>
public static class StringExtensions
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 문자열에 값이 비어 있는가?
	/// </summary>
	/// <param name="target"></param>
	/// <returns></returns>
	public static bool IsNullorEmpty( this string target )
	{
		return string.IsNullOrEmpty( target );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 문자열에 매개변수를 적용하여 문자열을 완성한다.
	/// </summary>
	/// <param name="target"></param>
	/// <param name="parms"></param>
	/// <returns></returns>
	public static string ApplyParameter( this string target, params object[] parms )
	{
		if( parms.Length < 1 )
		{
			return target;
		}

		return string.Format( target, parms );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
}//class StringExtensions
