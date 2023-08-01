using System;
using System.Collections;
using UniRx;
using UnityEngine;

/// <summary>
/// @class Observable_Hot
/// @date 2023/7/31
/// @author sejong
/// @brief Hot Observable 
///  - Observable이 생성되자마자 메시지를 발행하기 시작한다.
///  -구독하는 Observer들은 중간부터 구독을 하게된다.
///  - 스트림을 분기 시키거나, 메시지를 분배하는 것이 가능하다.
/// </summary>
public class Observable_Hot : MonoBehaviour
{

    private IEnumerator Start()
    {
		// .Publish().RefCount() 를 추가해서 Hot 임을 선언한다.
		// Subscribe 와 OnNext 시점은 상관이 없어지며, Publish 시점으로 고정된다.
		var stream = Observable.Interval( TimeSpan.FromSeconds(1) ).Publish().RefCount();

		stream.Subscribe( x => DebugExtensions.Log( $"첫번째 구독 : { Time.time.ToString() }", Color.red ) );

		yield return new WaitForSeconds( 0.1f );

		stream.Subscribe( x => DebugExtensions.Log( $"두번째 구독 : { Time.time.ToString() }", Color.red ) );

		yield return new WaitForSeconds( 0.1f );

		stream.Subscribe( x => DebugExtensions.Log( $"세번째 구독 : { Time.time.ToString() }", Color.red ) );

/*
		결과 :

		첫번째 구독 : 1.000108
		두번째 구독 : 1.000108
		세번째 구독 : 1.000108

		첫번째 구독 : 2.001221
		두번째 구독 : 2.001221
		세번째 구독 : 2.001221

		첫번째 구독 : 3.003944
		두번째 구독 : 3.003944
		세번째 구독 : 3.003944

		첫번째 구독 : 4.005189
		두번째 구독 : 4.005189
		세번째 구독 : 4.005189
 */ 
	}

}//Observable_Hot 
