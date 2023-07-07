/**---------------------------------------------------------------------------------
 * @file SystemEventBusManager.cs
 * @date 2023/7/7
 * @author sejong
 * @brief 시스템 이벤트의 전달을 위한 매니저
 *///-------------------------------------------------------------------------------
using BaseSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 시스템 이벤트 모음
/// </summary>
public enum eSystemEvent
{
	APPLICATION_LOW_MEMORY = 0,
}//eSystemEvent

/// <summary>
/// @class SystemEventBusManager
/// @date 2023/7/7
/// @author sejong
/// @brief 시스템 이벤트의 전달을 위한 매니저
/// </summary>
public class SystemEventBusManager : BaseSingleton<SystemEventBusManager>, IBaseSingleton
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	private SystemEventBus _eventNode = null;

	public static SystemEventBus INST
	{
		get
		{
			if( null == INSTANCE._eventNode )
				INSTANCE.Initialize();

			return INSTANCE._eventNode;
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	#region IBaseSingleton
	public void Initialize()
	{
		if( null == _eventNode )
			_eventNode = new SystemEventBus();

		_eventNode.Initialize();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public void Release()
	{
		if( null != _eventNode )
			_eventNode.Release();

		_eventNode = null;
	}
	#endregion//IBaseSingleton

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//SystemEventBusManager

/// <summary>
/// @class SystemEventBus
/// @date 2023/7/7
/// @author sejong
/// @brief 
/// </summary>
public class SystemEventBus: EventBusBase<eSystemEvent> { }