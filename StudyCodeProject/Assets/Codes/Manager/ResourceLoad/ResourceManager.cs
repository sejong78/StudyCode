/**---------------------------------------------------------------------------------
 * @file ResourceManager.cs
 * @date 2023/7/6
 * @author sejong
 * @brief 리소스 로딩 관련 처리를 위한 매니저
 *///-------------------------------------------------------------------------------
using UnityEngine;
using System;
using BaseSingleton;
using Cysharp.Threading.Tasks;


/// <summary>
/// @class ResourceManager
/// @date 2023/7/6
/// @author sejong
/// @brief 리소스 로딩 관련 처리를 위한 매니저
/// </summary>
public class ResourceManager : BaseSingleton<ResourceManager>, IBaseSingleton
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 리소스를 로딩한다.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path"></param>
	/// <param name="onLoadSuccess"></param>
	/// <param name="onProgress"></param>
	/// <returns></returns>
	public async UniTask<T> Load<T>( string path, Action<T> onLoadSuccess = null, Action<float> onProgress = null ) where T : UnityEngine.Object
	{
		// path 가 비어 있다면 무시한다.
		if( true == string.IsNullOrEmpty( path ) )
			return null;

		var rtn = await UniTaskManager.INSTANCE.LoadResource<T>( 
			path, 
			cancleKey:"ResourceLoadManager", 
			onProgress:onProgress );

		onLoadSuccess.SafeExcute( rtn );

		return rtn;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	#region IBaseSingleton

	/// <summary>
	/// 생성자에서 호출될 초기화 함수
	/// </summary>
	public void Initialize()
	{
		Release();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 릴리즈 함수
	/// </summary>
	public void Release()
	{
		UniTaskManager.INSTANCE.CancelTask( "ResourceLoadManager" );
	}

	#endregion//IBaseSingleton

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//ResourceManager
