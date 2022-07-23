/**---------------------------------------------------------------------------------
 * @file UniTaskManager.cs
 * @date 2022/7/16
 * @author sejong
 * @brief UniTask 의 래핑 클래스
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSingleton;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

/// <summary>
/// @class UniTaskManager
/// @date 2022/7/16
/// @author sejong
/// @brief UniTask 의 래핑 클래스
/// </summary>
public class UniTaskManager : BaseSingleton<UniTaskManager>, IBaseSingleton
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// CancellationToken 관리용 딕셔너리
	/// </summary>
	private Dictionary<string,CancellationTokenSource> _cancelTokenDic = new Dictionary<string, CancellationTokenSource>();

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Unity 통신
	/// </summary>
	/// <param name="url"></param>
	/// <param name="cancleKey"></param>
	/// <returns></returns>
	public async UniTask<string> WebRequest( string url, string cancleKey = "" )
	{
		// Url 이 비어 있다면 무시한다.
		if( true == string.IsNullOrEmpty( url ) )
			return "";
		
		try
		{
			var cancelToken = GetCancellationToken( cancleKey );
	
			var wr = UnityWebRequest.Get( url );
	
			if( cancelToken == default )
				await wr.SendWebRequest();
			else
				await wr.SendWebRequest().WithCancellation( cancelToken );

			switch( wr.result )
			{
				case UnityWebRequest.Result.InProgress:
				{
#if DEBUG
					Debug.LogError( $"[Error] UnityWebRequest InProgress!!" );
#endif//DEBUG
					return "";
				}

				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
				case UnityWebRequest.Result.ProtocolError:
				{
#if DEBUG
					Debug.LogError( $"[Error] UnityWebRequest result = {wr.result}\n - {wr.error}" );
#endif//DEBUG
					return "";
				}

				case UnityWebRequest.Result.Success:
					break;
			}

			return wr.downloadHandler.text;
		}
		catch( System.Exception ex )
		{
#if DEBUG
			Debug.Log( $"[Exception] UnityWebRequest = {ex.Message}" );
#endif//DEBUG
			return "";
		}

	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 리소스 로딩을 비동기로 처리한다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path"></param>
	/// <param name="cancleKey"></param>
	/// <returns></returns>
	public async UniTask<T> ResourceLoad<T>( string path, string cancleKey = "" ) where T : UnityEngine.Object
	{
		// path 가 비어 있다면 무시한다.
		if( true == string.IsNullOrEmpty( path ) )
			return null;

		try
		{
			var res = default( UnityEngine.Object );
			var cancelToken = GetCancellationToken( cancleKey );

			if( cancelToken == default )
				res = await Resources.LoadAsync<T>( path );
			else
				res = await Resources.LoadAsync<T>( path ).WithCancellation( cancelToken );

			return res as T;
		}
		catch( Exception ex )
		{
#if DEBUG
			Debug.Log( $"[Exception] Resources.LoadAsync = {ex.Message}" );
#endif//DEBUG
			return null;
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 여러개의 캔슬 토큰을 묶어서 하나로 만들어 준다.
	/// </summary>
	/// <param name="ctns"></param>
	/// <returns></returns>
	public CancellationTokenSource LinkCancellationToken( params CancellationToken[] ctns )
	{
		var linkedCtn = CancellationTokenSource.CreateLinkedTokenSource( ctns );

		return linkedCtn; 
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 캔슬 토큰을 얻어 온다.\n
	/// 캔슬 토큰은 비동기 작업 함수 내에서 리스너 역할을 한다.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public CancellationToken GetCancellationToken( string key )
	{
		if( true == string.IsNullOrEmpty( key ) )
			return default;

		// 이미 있는 경우에
		if( true == _cancelTokenDic.ContainsKey( key ) )
		{
			// 사용한 적이 없다면 그대로 리턴
			if( false == _cancelTokenDic[key].IsCancellationRequested )
				return _cancelTokenDic[ key ].Token;

			// 사용한 적이 있으면 제거
			_cancelTokenDic[ key ].Dispose();
			_cancelTokenDic.Remove( key );
		}

		// 할당 한다.
		_cancelTokenDic[ key ] = new CancellationTokenSource();

		return _cancelTokenDic[ key ].Token;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 해당 비동기 작업을 캔슬 한다.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="isForce">예외를 즉시 전파해야 하는 경우 true이고, 그렇지 않으면 false입니다</param>
	public void CancelTask( string key, bool isForce = false )
	{
		if( false == _cancelTokenDic.ContainsKey( key ) )
		{
#if DEBUG
			Debug.Log( $"[UniTaskManager] {key} 는 등록되지 않은 캔슬 토큰 입니다." );
#endif//DEBUG
			return;
		}

		/// 중지 시킨 후에는 지워준다.
		_cancelTokenDic[ key ].Cancel( isForce );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	#region IBaseSingleton

	/// <summary>
	/// 초기화
	/// </summary>
	public void Initialize()
	{
		Dispose();
				
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 릴리즈 함수 
	/// </summary>
	public void Dispose()
	{
		// 살아 있는 토큰은 모두 정리하고 클리어
		foreach( var ctn in _cancelTokenDic.Values )
		{
			ctn.Cancel();
			ctn.Dispose();
		}

		_cancelTokenDic.Clear();

	}

	#endregion//IBaseSingleton

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//UniTaskManager
