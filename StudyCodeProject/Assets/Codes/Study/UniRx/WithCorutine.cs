using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// @class WithCorutine
/// @date 2023/7/31
/// @author sejong
/// @brief 코루틴과 함께 사용하기
///  - 복잡한 처리를 코루틴에 숨기고 외부에서는 Observable로서 취급하는 것이 가능하다.
///  - Cold 라서 Subscribe 될 때마다 새롭게 코루틴이 생성되고 시작한다.
/// </summary>
public class WithCorutine : MonoBehaviour
{
    void Start()
    {
		// 2번째 인자는 코루틴의 매 yield 마다 OnNext를 발행 할지의 여부이다.
		Observable.FromCoroutine( ACoroutine, false ).Subscribe( _ => DebugExtensions.Log( "OnNext", Color.red ),() => DebugExtensions.Log( "OnCompleted", Color.red ) );
		Observable.FromCoroutine( BCoroutine, true ).Subscribe( _ => DebugExtensions.Log( "OnNext", Color.green ), () => DebugExtensions.Log( "OnCompleted", Color.green ) );

		// 코루틴의 yield return 값을 OnNext 메시지로 받는다.
		// 리턴된 값이 형식 인수와 맞지 않으면 오류가 발생한다.
		Observable.FromCoroutineValue<string>( CCoroutine, true ).Subscribe( x => DebugExtensions.Log( x, Color.blue ), () => DebugExtensions.Log( "OnCompleted", Color.blue ) );
	}

	private IEnumerator ACoroutine()
	{
		DebugExtensions.Log( "코루틴 ACoroutine 시작", Color.red );
		yield return new WaitForSeconds( 2f );
		DebugExtensions.Log( "코루틴 ACoroutine 끝", Color.red );
	}

	private IEnumerator BCoroutine()
	{
		DebugExtensions.Log( "코루틴 BCoroutine 시작", Color.green );
		yield return new WaitForSeconds( 2f );
		DebugExtensions.Log( "코루틴 BCoroutine 끝", Color.green );
	}

	private IEnumerator CCoroutine()
	{
		for( int i = 0; i < 3; ++i )
		{
			yield return new WaitForSeconds( 1f );
			yield return "CCoroutine 리턴";
		}
	}

}//WithCorutine
