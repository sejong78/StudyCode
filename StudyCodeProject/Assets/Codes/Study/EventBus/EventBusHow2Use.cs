using BaseSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusHow2Use : MonoBehaviour
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	public enum busSample
	{
		ZERO = 0,
		ONE = 1,	
		TWO = 2,
		THREE = 3,
	}

	private void Awake()
	{
 		var mgr0 = TestEventBusManager.INST;


		// 구독 설정한다음에
		mgr0.AddEventSubscribe( (int)busSample.ZERO, TempEventFunc );

		// Fire!!
		mgr0.FireEvent( ( int )busSample.ZERO, busSample.ZERO.ToString() );

		MemoryManager.INSTANCE.UnloadUnusedAssets( true );
	}

	private void TempEventFunc( params object[] prm )
	{
		var samplekey = ( string )prm[ 0 ];

		DebugExtensions.Log( $"{samplekey.ToString()} 이네!!", Color.white );
		
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	public class TestEventBus : EventBusBase<busSample>
	{

	}

	public class TestEventBusManager : BaseSingleton<TestEventBusManager>, IBaseSingleton
	{
		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------

		private TestEventBus _eventNode = null;

		public static TestEventBus INST
		{
			get
			{
				if( null == INSTANCE._eventNode )
					INSTANCE.Initialize();

				return INSTANCE._eventNode;
			}
		}

		//@@-------------------------------------------------------------------------------------------------------------------------

		public void Initialize()
		{
			if( null == _eventNode )
				_eventNode = new TestEventBus();

			_eventNode.Initialize();
		}

		//@@-------------------------------------------------------------------------------------------------------------------------

		public void Release()
		{
			if( null != _eventNode )
				_eventNode.Release();

			_eventNode = null;
		}

		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------
	}

}//EventBusHow2Use

