using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class ReactiveProperty_Recv : MonoBehaviour
{
	// Start is called before the first frame update
	private IEnumerator Start()
	{
		var send = GetComponent<ReactiveProperty_Send>();

		yield return new WaitForSeconds( 3f );

		// Subscribe 시점에서 해당 값을 가져오고, 이후 변동이 생기면 그 값을 가져온다.
		send.PR_DATA.VALUE.
            Do( n =>
            {
                DebugExtensions.Log( $"[PR_DATA] n = {n}", Color.white );
            } ).
            Subscribe().
			AddTo( this );

		// class 를 ReactiveProperty 로 할당 한 경우, 그 멤버에 값에 대한 개별 추적이 가능하다.
		send.NORMAL_DATA.ObserveEveryValueChanged( x => x.Value.VALUE ).
			// 스트림으로 추적하는 값이 5 미만인 경우만 받는다.
			TakeWhile( x => x < 5 ).
			Do( n =>
			{
				DebugExtensions.Log( $"[NORMAL_DATA] n = {n}", Color.yellow );
			} ).
			// TakeWhile 에서 지정한 범위 를 벗어난 경우 OnCompleted 가 불린다.
			DoOnCompleted( () => 
			{
				DebugExtensions.Log( $"[NORMAL_DATA] 끝났다~", Color.yellow );
			} ).
			Subscribe().
			AddTo( this );

		// 값의 변화가 없다면 Subscribe 시 초기화 이후 값이 들어오지 않는다.
		send.NORMAL_DATA.ObserveEveryValueChanged( x => x.Value.VALUE2 ).
			Do( n =>
			{
				DebugExtensions.Log( $"[NORMAL_DATA] n2 = {n}", Color.magenta );
			} ).
			Subscribe().
			AddTo( this );

/*
		결과 :

		[PR_DATA] n = 15
		[NORMAL_DATA] n = 1
		[NORMAL_DATA] n2 = 0

		[PR_DATA] n = 20
		[NORMAL_DATA] n = 2

		[PR_DATA] n = 25
		[NORMAL_DATA] n = 3

		[PR_DATA] n = 30
		[NORMAL_DATA] n = 4

		[PR_DATA] n = 35
		[NORMAL_DATA] 끝났다~

		[PR_DATA] n = 40

		[PR_DATA] n = 45

		[PR_DATA] n = 50

		[PR_DATA] n = 55
*/
	}

}
