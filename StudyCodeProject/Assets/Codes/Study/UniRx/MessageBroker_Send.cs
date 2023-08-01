using System.Collections;
using UnityEngine;
using UniRx;

/// <summary>
/// @class MessageBroker_Send
/// @date 2023/7/31
/// @author sejong
/// @brief 전역적으로 참조가 가능한 메시지 라우팅 시스템이다.
/// </summary>
public class MessageBroker_Send : MonoBehaviour
{
    public struct EventMsg
    {
        public string key;
		public string value;
	}

    IEnumerator Start()
    {
        var wait = new WaitForSeconds( 1f );
        
        yield return wait;

        EventMsg msg1 = new EventMsg{ key = "key1", value = "MessageBroker 1"  };

		MessageBroker.Default.Publish( msg1 );

		yield return wait;

		EventMsg msg2 = new EventMsg{ key = "key2", value = "MessageBroker 2"  };

		MessageBroker.Default.Publish( msg2 );

	}

}


