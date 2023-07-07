using BaseSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EventBusBase<T> where T : Enum
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	public delegate void EventFunc( params object[ ] prm );

	protected EventFunc[] _eventArray = null;

	protected Type _busEnumType = null;


	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	public virtual void Initialize()
	{
		Release();

		_busEnumType = typeof( T );

		Array array = Enum.GetValues( _busEnumType );

		int count = array.Length;

		_eventArray = new EventFunc[ count ];
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 할당된 이벤트를 모두 정리한다.
	/// </summary>
	public virtual void Release()
	{
		_busEnumType = null;

		if( null == _eventArray )
			return;

		for( int i = 0; i < _eventArray.Length; ++i )
			_eventArray[ i ] = null;

		_eventArray = null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 이벤트를 구독합니다.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="func"></param>
	public void AddEventSubscribe( int key, EventFunc func )
	{
		if( null == _eventArray || null == func )
		{
			return;
		}

		// 기존에 등록된 이벤트가 없으면 할당 후 리턴
		if( null == _eventArray[ key ] )
		{
			_eventArray[ key ] = func;
			return;
		}

		// 기 등록된 이벤트가 있으므로, 추가
		_eventArray[ key ] -= func;
		_eventArray[ key ] += func;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 특정 이벤트 구독을 해제 합니다.
	/// 이벤트가 null 이면, 해당 키값의 이벤트 모두 제거
	/// </summary>
	/// <param name="key"></param>
	/// <param name="func"></param>
	public void RemoveEventSubscribe( int key, EventFunc func = null )
	{
		if( null == _eventArray )
		{
			return;
		}

		if( null == func )
		{
			_eventArray[ key ] = null;
			return;
		}

		_eventArray[ key ] -= func;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 구독자에게 알림
	/// </summary>
	/// <param name="key"></param>
	/// <param name="prms"></param>
	public void FireEvent( int key, params object[] prms )
	{
		if( null == _eventArray )
		{
			return;
		}

		_eventArray[ ( int )key ]?.Invoke( prms );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
}// EventBusBase
