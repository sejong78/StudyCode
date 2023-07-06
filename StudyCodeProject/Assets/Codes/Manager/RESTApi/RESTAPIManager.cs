/**---------------------------------------------------------------------------------
 * @file RESTAPIManager.cs
 * @date 2022/6/22
 * @author sejong
 * @brief Unity 의 REST API 를 사용하기 위한 메니저
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BaseSingleton;
using LitJson;
using Cysharp.Threading.Tasks;

/// <summary>
/// @class RESTAPIManager
/// @date 2022/6/22
/// @author sejong
/// @brief Unity 의 REST API 를 사용하기 위한 메니저
/// </summary>
public class RESTAPIManager : BaseSingleton<RESTAPIManager>, IBaseSingleton
{

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------


	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	public async UniTask<(bool, JsonData)>Request( string uri, string cancleKey = "", params string[] prms )
	{
		if( true == string.IsNullOrEmpty( uri ) )
		{
			return (false, null);
		}

		if( null != prms )
		{
			if( 0 < prms.Length && 0 < prms.Length % 2 )
			{
#if DEBUG
				DebugExtensions.LogError( $"RESTAPIManager.Request 의 파라메타는 짝수여야 합니다.", Color.white );
#endif//DEBUG

				return (false, null);
			}
		}

#if DEBUG
		DebugExtensions.AppendLineLog( $"[RESTAPI] uri = {uri}", Color.white );
#endif//DEBUG

		// RestApi 의 경우는 Get 방식이다.
		var downHandler = await UniTaskManager.INSTANCE.WebRequest_Get( 
			uri, cancleKey, (req) => 
			{
				SetHeaderParams( req, prms );
#if DEBUG
				DebugExtensions.WriteDebugLog();
#endif//DEBUG
			} );

		// 결과가 비어 있다면 실패이다.
		if( null == downHandler )
			return (false, null);

		try
		{
			JsonData jsonData = JsonMapper.ToObject( downHandler.text );

			return (true, jsonData);
		}
		catch( System.Exception e )
		{
#if DEBUG
			DebugExtensions.Log( $"[ToJson Fail] {e.Message}", Color.red );
			DebugExtensions.Log( $"[receved] = {downHandler.text}", Color.red );
#endif//DEBUG
			return (false, null);
		}

	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	private void SetHeaderParams( UnityWebRequest req, params string[] prms )
	{
		if( null == req )
			return;

		if( null == prms || prms.Length < 2 )
			return;

		for( int i = 0; i < prms.Length; i += 2 )
		{
			req.SetRequestHeader( prms[ i ], prms[ i + 1 ] );
#if DEBUG
			DebugExtensions.AppendLineLog( $" ㄴ param {prms[ i ]} : {prms[ i + 1 ]}", Color.white );
#endif//DEBUG
		}

	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	private IEnumerator ProcessRequest( string uri, Action<bool, JsonData> onComplete, string[] prms )
	{
		using( UnityWebRequest req = UnityWebRequest.Get( uri ) )
		{
#if DEBUG
			DebugExtensions.AppendLineLog( $"[RESTAPI] uri = {uri}", Color.white );
#endif//DEBUG

			if( null != prms )
			{
				for( int i = 0; i < prms.Length; i += 2 )
				{
					req.SetRequestHeader( prms[ i ], prms[ i + 1 ] );
#if DEBUG
					DebugExtensions.AppendLineLog( $" ㄴ param {prms[ i ]} : {prms[ i + 1 ]}", Color.white );
#endif//DEBUG
				}
			}

#if DEBUG
			DebugExtensions.WriteDebugLog();
#endif//DEBUG

			yield return req.SendWebRequest();

			if( req.result == UnityWebRequest.Result.ConnectionError )
			{
#if DEBUG
				DebugExtensions.LogError( $"uri = {uri} \nError = {req.error}", Color.white );
#endif//DEBUG

				onComplete.SafeExcute( false, null );
				yield break;
			}

			try
			{
				JsonData json = JsonMapper.ToObject( req.downloadHandler.text );

				onComplete.SafeExcute( true, json );
			}
			catch( System.Exception e )
			{
#if DEBUG
				DebugExtensions.Log( $"[ToJeson Fail] {e.Message}", Color.red );
				DebugExtensions.Log( $"[receved] = {req.downloadHandler.text}", Color.red );
#endif//DEBUG
				onComplete.SafeExcute( false, null );
			}

		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	#region IBaseSingleton

	public void Release()
	{
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public void Initialize()
	{
	}

	#endregion//IBaseSingleton

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//RESTAPIManager
