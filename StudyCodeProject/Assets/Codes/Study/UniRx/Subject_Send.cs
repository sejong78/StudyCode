using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks.Linq;

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

	/// <summary>
	/// 구독 이후에 Observable이 배출한 메시지만 배출한다.
	/// </summary>
	private Subject<int> mySubject = new Subject<int>();

	/// <summary>
	/// 중간에 아무것도 안받다가, 종료될때, 구독중인 Observer 들에게 마지막 값을 배출한다.
	/// 중간에 오류가 발생했다면, 값 배출 없이 오류메시지만 출력
	/// </summary>
	private AsyncSubject<int> asyncSubject = new AsyncSubject<int>();

	/// <summary>
	/// 구독 시작시 가장 최근 발생한 메시지를 초기값으로 배출
	/// </summary>
	private BehaviorSubject<int> behaviorSubject = new BehaviorSubject<int>( defaultValue:-1 );

	/// <summary>
	/// 구독 시작 시점과 관계없이 지금까지 Observable이 배출한 항목들을 Observer에게 배출한다.
	/// </summary>
	private ReplaySubject<int> replaySubject = new ReplaySubject<int>();

	/// <summary>
	/// 외부에서 Subscribe 를 구독만 가능하게 하기 위해
	/// </summary>
	public IObservable<int> SUBJECT => mySubject;
	public IObservable<int> ASYNC_SUBJECT => asyncSubject;
	public IObservable<int> BEHAVIOR_SUBJECT => behaviorSubject;
	public IObservable<int> REPLAY_SUBJECT => replaySubject;

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	private IEnumerator Start()
	{
		var wait = new WaitForSeconds(1f);

		yield return wait;

		for( int i = 0; i < 10; ++i )
		{
			mySubject.OnNext( i );
			asyncSubject.OnNext( i );
			behaviorSubject.OnNext( i );
			replaySubject.OnNext( i );

			yield return wait;
		}

		mySubject.OnCompleted();
		asyncSubject.OnCompleted();
		behaviorSubject.OnCompleted();
		replaySubject.OnCompleted();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//Subject_Send
