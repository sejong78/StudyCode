using UnityEngine;
using static MessageBroker_Send;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class MessageBroker_Recv : MonoBehaviour
{

	private IDisposable _messageBrokerStream = null;

	void Start()
    {
		// 받아 들이는 메세지 중에 "key2" 인 것 만 받고자 한다.
		_messageBrokerStream = MessageBroker.Default.Receive<MessageBroker_Send.EventMsg>().
			Where( x => 0 == string.Compare( x.key, "key2" ) ).
			Subscribe( OnEvent );
            
		
    }

    private void OnEvent( MessageBroker_Send.EventMsg msg )
    {
		DebugExtensions.Log( $"MessageBroker_Recv {msg.value}", Color.white );
	}

	private void OnDestroy()
	{
		// 물론 AddTo 로  설정해도 무방하다.
		_messageBrokerStream.Dispose();
	}

}
