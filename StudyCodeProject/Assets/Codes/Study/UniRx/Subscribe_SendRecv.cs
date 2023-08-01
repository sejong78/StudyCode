using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Subscribe_SendRecv : MonoBehaviour
{
	Subject<int> mySubject = new Subject<int>();


	private IEnumerator Start()
    {
		IDisposable disposalA = mySubject.
			Subscribe( _ => DebugExtensions.Log( "A", Color.red ), () => DebugExtensions.Log( "A Completed", Color.red ) );

		IDisposable disposalB = mySubject.
			Subscribe( _ => DebugExtensions.Log( "B", Color.green), () => DebugExtensions.Log( "B Completed", Color.green ) );

		StartCoroutine( MyCoroutine() );
		yield return new WaitForSeconds( 4.5f );

		// disposalA 의 동작만 멈춘다. 이때 OnCompleted 가 호출되지 않는다.
		disposalA.Dispose();

		// 스트림을 재 할당 받는것이기 때문에, 다시 동작한다.
		disposalA = mySubject.
					Subscribe( _ => DebugExtensions.Log( "A2", Color.red ), () => DebugExtensions.Log( "A2 Completed", Color.red ) );

		yield return new WaitForSeconds( 2f );

		// 스트림의 송출을 멈추기 때문에 구독자는 모두 OnCompleted 가 호출된다.
		mySubject.OnCompleted();

		// Subject 가 이미 멈춘 상태였기 때문에 곧바로 OnCompleted 가 호출된다.
		disposalA = mySubject.
			Subscribe( _ => DebugExtensions.Log( "A3", Color.red ), () => DebugExtensions.Log( "A3 Completed", Color.red ) );

		/*
				결과 :

				A
				B

				A
				B

				A
				B

				A
				B

				B
				A2

				B
				A2

				B Completed
				A2 Completed

				A3 Completed
		*/

	}

	IEnumerator MyCoroutine()
	{
		var wait = new WaitForSeconds(1f);

		yield return wait;

		for( int i = 0; i < 10; ++i )
		{
			mySubject.OnNext( 0 );
			yield return wait;
		}
	}

}//Subscribe_SendRecv
