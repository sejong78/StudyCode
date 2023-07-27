using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// UniRX에서 제공하는 Subject<T>를 통해 UniRX의 스트림을 생성할 수 있다.
/// ISubject는 IObservable 과 IObserver를 구현하고 있다.
/// IObserver : OnNext(), OnCompleted(), OnError()
/// IObservable : Subscribe()
/// subject 객체에서 OnNext(), OnCompleted(), OnError() 메시지를 직접 호출할 수 있다.
/// </summary>
public class Subject_Send : MonoBehaviour
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private Subject<int> mySubject = new Subject<int>();

	/// <summary>
	/// 외부에서 Subscribe 를 구독만 가능하게 하기 위해
	/// </summary>
	public IObservable<int> SUBJECT => mySubject;

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	private IEnumerator Start()
	{
		var wait = new WaitForSeconds(1f);

		yield return wait;
		mySubject.OnNext( 1 );
		yield return wait;
		mySubject.OnNext( 10 );
		yield return wait;
		mySubject.OnCompleted();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//Subject_Send
