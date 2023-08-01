using System;
using System.Collections;
using UniRx;
using UnityEngine;

/// <summary>
/// @class Observable_Cold
/// @date 2023/7/31
/// @author sejong
/// @brief  
/// - Observable을 구독하기 전까지 메시지를 발행하지 않는다.\
/// - Observer가 없어지면 동작하지 않는다.
/// - Subscribe 될 때마다 새롭게 생성되며, 별도의 스트림이 된다.
/// - 대부분의 스트림은 Cold이다.
/// </summary>
public class Observable_Cold : MonoBehaviour
{
	private IEnumerator Start()
	{
		var stream = Observable.Interval( TimeSpan.FromSeconds(1) );

		// Subscribe 시점에 스트림이 생성되며, 각기 1초의 인터벌을 가지고 OnNext 가 이루어진다.
		stream.Subscribe( x => DebugExtensions.Log( $"첫번째 구독 : {Time.time.ToString()}", Color.green ) );

		yield return new WaitForSeconds( 0.1f );

		stream.Subscribe( x => DebugExtensions.Log( $"두번째 구독 : {Time.time.ToString()}", Color.green ) );

		yield return new WaitForSeconds( 0.1f );

		stream.Subscribe( x => DebugExtensions.Log( $"세번째 구독 : {Time.time.ToString()}", Color.green ) );

/*
		결과 :

		첫번째 구독 : 1.002675
		두번째 구독 : 1.354665
		세번째 구독 : 1.457639

		첫번째 구독 : 2.004912
		두번째 구독 : 2.356786
		세번째 구독 : 2.458693

		첫번째 구독 : 3.007428
		두번째 구독 : 3.358719
		세번째 구독 : 3.461481

		첫번째 구독 : 4.010394
		두번째 구독 : 4.358991
		세번째 구독 : 4.462405

*/
	}
}
