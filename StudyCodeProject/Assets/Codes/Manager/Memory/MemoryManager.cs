using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSingleton;
using Cysharp.Threading.Tasks;

public class MemoryManager : BaseSingleton<MemoryManager>, IBaseSingleton
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	// 저사양 단말의 기준 메모리 값
	private readonly int LOWSPEC_MEMORY = 3072;


	/// <summary>
	/// 저사양 단말인가?
	/// </summary>
	private bool? _isLowMemoryDevice = null;

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	#region IBaseSingleton
	public void Initialize()
	{
		Application.lowMemory -= OnEvent_LowMemory;
		Application.lowMemory += OnEvent_LowMemory;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	public void Release()
	{
		Application.lowMemory -= OnEvent_LowMemory;
	}
	#endregion//IBaseSingleton

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 사용하지 않는 메모리를 수집한다.
	/// </summary>
	/// <param name="collectGC"></param>
	/// <returns></returns>
	public void UnloadUnusedAssets( bool collectGC = false )
	{
		// 메모리를 정리한다.
		UnloadUnusedAssetsAsync( collectGC ).Forget();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 사용하지 않는 메모리를 수집한다.
	/// </summary>
	/// <param name="collectGC"></param>
	/// <returns></returns>
	public async UniTaskVoid UnloadUnusedAssetsAsync( bool collectGC = false )
	{
		await ResourceManager.INSTANCE.UnloadUnusedAssets( collectGC );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 저사양 단말인가?
	/// </summary>
	/// <returns></returns>
	public bool IsLowMemoryDevice()
	{
		if( null != _isLowMemoryDevice )
			return ( bool )_isLowMemoryDevice;

		int sysMemMB = SystemInfo.systemMemorySize;

		_isLowMemoryDevice = false;

		// lowSpecMemoryValue 값 이하이면 작으면
		if( sysMemMB <= LOWSPEC_MEMORY )
			_isLowMemoryDevice = true;

#if DEBUG
		DebugExtensions.Log( $"본 디바이스의 총 메모리는 {sysMemMB}MB ( {sysMemMB / 1024}GB ) 이므로 {( true == ( bool )_isLowMemoryDevice ? "저사양 단말입니다." : "저사양 단말이 아닙니다." )}", Color.white );
#endif//DEBUG

		return ( bool )_isLowMemoryDevice;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 메로리 부족시 해당 사항을 노티 합니다.
	/// </summary>
	private void OnEvent_LowMemory()
	{
		// 다른곳에서 뭔가 할것 다 하고
		SystemEventBusManager.INST.FireEvent( ( int )eSystemEvent.APPLICATION_LOW_MEMORY );

		// 메모리를 정리한다.
		UnloadUnusedAssetsAsync( true ).Forget();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
}//MemoryManagerc
