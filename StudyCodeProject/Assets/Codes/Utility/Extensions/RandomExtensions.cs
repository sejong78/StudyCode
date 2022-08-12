/**---------------------------------------------------------------------------------
 * @file RandomExtensions.cs
 * @date 2022/8/11
 * @author sejong
 * @brief Random 확장함수
 *///-------------------------------------------------------------------------------
using UnityEngine;

/// <summary>
/// @class RandomExtensions
/// @date 2022/8/11
/// @author sejong
/// @brief Random 확장함수
/// </summary>
public static class RandomExtensions
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private static readonly int		_percent		= 100;
	private static readonly float	_percentFloat	= 100f;

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 퍼센트값을 확률값으로 처리합니다.
	/// </summary>
	/// <param name="chance">0~100 사이의 값</param>
	/// <returns></returns>
	public static bool RandomPercent( int chance )
	{
		return Random.Range( 0, _percent ) < chance;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 퍼센트값을 확률값으로 처리합니다.(float)
	/// </summary>
	/// <param name="chance">0~100 사이의 값</param>
	/// <returns></returns>
	public static bool RandomPercent( float chance )
	{
		if( true == chance.Equals( _percentFloat ) )
		{
			return true;
		}

		return Random.Range( 0f, _percentFloat ) < chance;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 0 ~ 1 값을 확률값으로 처리합니다.
	/// </summary>
	/// <param name="chance">0 ~ 1 사이의 확률값</param>
	/// <returns></returns>
	public static bool Random01( float chance )
	{
		if( true == chance.Equals( 1f ) )
		{
			return true;
		}

		return Random.Range( 0, 1f ) < chance;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//RandomExtensions
