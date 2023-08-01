using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class Subject_Recv : MonoBehaviour
{
    //@@-------------------------------------------------------------------------------------------------------------------------
    //@@-------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
		// 4.5초 후 시작
		Observable.Timer( TimeSpan.FromSeconds( 4.5 ) ).
			Subscribe( _ =>
			{
                LinkSubscribe();
			} );

	}

	void LinkSubscribe()
    {

        var send_subject = GetComponent<Subject_Send>();
		var mySubject = send_subject.SUBJECT;

        mySubject.
            Do( SomeMethod ).
            DoOnCompleted( () => 
            { 
                DebugExtensions.Log( $"[Subject_Recv][mySubject] 끝~", Color.white );
            } ).
            Subscribe();


		// 전부 무시하다가 OnCompleted 호출시에 마지막값 받고, OnCompleted 실행 후 종료
		var asyncSubject = send_subject.ASYNC_SUBJECT;

        asyncSubject.
            Do( AsyncMethod ).
            DoOnCompleted( () =>
            {
				DebugExtensions.Log( $"[Subject_Recv][asyncSubject] 끝~", Color.red );
			} ).Subscribe();

		// Subscribe 시 가장 최근 발행한 메세지를 받고 이후엔 정상적으로 받는다.
		var behaviorSubject = send_subject.BEHAVIOR_SUBJECT;

		behaviorSubject.
			Do( BehaviorMethod ).
			DoOnCompleted( () =>
			{
				DebugExtensions.Log( $"[Subject_Recv][behaviorSubject] 끝~", Color.green );
			} ).Subscribe();

		// Subscribe 시 그동안 배출한 메세지를 순차적으로 받고 이후엔 정상적으로 받는다.
		var replaySubject = send_subject.REPLAY_SUBJECT;

		replaySubject.
			Do( ReplayMethod ).
			DoOnCompleted( () =>
			{
				DebugExtensions.Log( $"[Subject_Recv][replaySubject] 끝~", Color.blue );
			} ).Subscribe();

/*
		// 결과

		[Subject_Recv][BehaviorMethod] n = 3
		[Subject_Recv][ReplayMethod] n = 0
		[Subject_Recv][ReplayMethod] n = 1
		[Subject_Recv][ReplayMethod] n = 2
		[Subject_Recv][ReplayMethod] n = 3

		[Subject_Recv][SomeMethod] n = 4
		[Subject_Recv][BehaviorMethod] n = 4
		[Subject_Recv][ReplayMethod] n = 4

		[Subject_Recv][SomeMethod] n = 5
		[Subject_Recv][BehaviorMethod] n = 5
		[Subject_Recv][ReplayMethod] n = 5

		[Subject_Recv][SomeMethod] n = 6
		[Subject_Recv][BehaviorMethod] n = 6
		[Subject_Recv][ReplayMethod] n = 6

		[Subject_Recv][SomeMethod] n = 7
		[Subject_Recv][BehaviorMethod] n = 7
		[Subject_Recv][ReplayMethod] n = 7

		[Subject_Recv][SomeMethod] n = 8
		[Subject_Recv][BehaviorMethod] n = 8
		[Subject_Recv][ReplayMethod] n = 8

		[Subject_Recv][SomeMethod] n = 9
		[Subject_Recv][BehaviorMethod] n = 9
		[Subject_Recv][ReplayMethod] n = 9

		[Subject_Recv][mySubject] 끝~
		[Subject_Recv][AsyncMethod] n = 9
		[Subject_Recv][asyncSubject] 끝~
		[Subject_Recv][behaviorSubject] 끝~
		[Subject_Recv][replaySubject] 끝~

*/

	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	private void SomeMethod( int n )
    {
		DebugExtensions.Log( $"[Subject_Recv][SomeMethod] n = {n}", Color.white );
	}

    //@@-------------------------------------------------------------------------------------------------------------------------
    

	private void AsyncMethod( int n )
	{
		DebugExtensions.Log( $"[Subject_Recv][AsyncMethod] n = {n}", Color.red );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private void BehaviorMethod( int n )
	{
		DebugExtensions.Log( $"[Subject_Recv][BehaviorMethod] n = {n}", Color.green );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private void ReplayMethod( int n )
	{
		DebugExtensions.Log( $"[Subject_Recv][ReplayMethod] n = {n}", Color.blue );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//Subject_Recv
