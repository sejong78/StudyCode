/**---------------------------------------------------------------------------------
 * @file ScreenTouchEffectManager.cs
 * @date 2020/2/10
 * @author sejong
 * @brief 화면 터치 효과 이펙트 제어용 메니저
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @class ScreenTouchEffectManager
/// @date 2020/2/10
/// @author sejong
/// @brief 화면 터잋 효과 이펙트 제어용 메니저 클레스
/// </summary>
public class ScreenTouchEffectManager : MonoBehaviour
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	#region CONST VALUES
	private const string LAYERMASK_NAME		= "NEW_UI";
	private const string EFFECT_POOL_ROOT	= "EffectRoot";
	#endregion//CONST VALUES

	#region INSTANCE

	static private ScreenTouchEffectManager _instance = null;

	public static ScreenTouchEffectManager INSTANCE
	{
		get
		{
			if( _instance == null )
			{
				GameObject go = new GameObject( typeof(ScreenTouchEffectManager).ToString() );
				GameObject.DontDestroyOnLoad( go );

				go.SetLayer( LayerMask.NameToLayer( LAYERMASK_NAME ), false );
				go.AddComponent<UIPanel>();
				_instance = go.AddComponent<ScreenTouchEffectManager>();
			}

			return _instance;
		}
	}

	#endregion//INSTANCE

	/// <summary>
	/// 이펙트를 설치할 루트 오브젝트
	/// </summary>
	private GameObject _EffectRoot = null;

	/// <summary>
	/// 이펙트 풀
	/// </summary>
	private List<ScreenTouchEffect> _effectList = new List<ScreenTouchEffect>();

	private int _effectIndex = 0;
	private int _effectCountMax = 5;

	// Temp
	private float	_tempPositionZ = 0f;
	private Vector3 _tempPositionVector = Vector3.zero;
	private ScreenTouchEffect _tempTouchEffect = null;
	static private UIRoot _tempRoot = null;

	//@@-------------------------------------------------------------------------------------------------------------------------

	private void Awake()
	{
		// 루트 오브젝트 생성
		if( _EffectRoot == null )
		{
			GameObject go = new GameObject( EFFECT_POOL_ROOT );
			_EffectRoot = NGUITools.AddChild( gameObject, go );
			go = null;
		}

		Initialize();
		DoSceneChanged();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private void OnDestroy()
	{

		for( int i = 0; i < _effectList.Count; ++i )
		{
			GameObject.Destroy( _effectList[ i ].gameObject );
		}

		_effectList.Clear();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	private void Initialize()
	{
		if( 0 < _effectList.Count )
		{
			return;
		}

		GameObject go = Resources.Load( "prefabs/effects/ScreenTouchEffect" ) as GameObject;

		for( int i = 0; i < _effectCountMax; ++i )
		{
			_tempTouchEffect = NGUITools.AddChild( _EffectRoot, go ).GetComponent<ScreenTouchEffect>();
			_tempTouchEffect.Initalize( i );

			_effectList.Add( _tempTouchEffect );
		}

		_effectIndex = 0;

		_tempTouchEffect = null;
		go = null;

		Resources.UnloadUnusedAssets();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// UICamera 에 CallBack함수를 동기화 한다.
	/// </summary>
	public void SyncCallBack()
	{
		UICamera.onPress -= OnEventPressScreen;
		UICamera.onPress += OnEventPressScreen;

		UICamera.onDrag -= OnEventDragScreen;
		UICamera.onDrag += OnEventDragScreen;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 활성화된 신이 바뀔때 마다 UICamera 를 동기화 한다.
	/// </summary>
	/// <param name="prev"></param>
	/// <param name="next"></param>
	public static void DoSceneChanged()
	{
		// 할당 되지 않은 상태에서의 호출도 생각해야 한다.
		if( _instance == null )
		{
			return;
		}

#if DEBUG
		Debug.LogFormat( "<color=#ff00ff>[ScreenTouchEffectManager] 신 이동이 완료 되었습니다. UI 카메라를 동기화 합니다.</color>" );
#endif//DEBUG

		_instance.SyncCallBack();

		// 이펙트 중지
		_instance.StopAll();

		// Scale 동기화
		if( UIRoot.list.Count == 0 )
		{
			_tempRoot = GameObject.FindObjectOfType<UIRoot>();
		}
		else
		{
			_tempRoot = UIRoot.list[ 0 ];
		}

		if( _tempRoot != null )
		{
			_instance._EffectRoot.transform.localScale = _tempRoot.transform.localScale;
		}

		_tempRoot = null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 활성중인 모든 Press 이펙트와 corutine 이 Null 인 Release 이벤트를 멈춘다.
	/// </summary>
	private void StopAll()
	{
		for( int i = 0; i < _effectList.Count; ++i )
		{
			_effectList[ i ].StopPlay();
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 이벤트를 스킵해야 할 상황인가?
	/// </summary>
	/// <returns></returns>
	private bool IsSkipEvent()
	{
		if( _instance == null )
		{
			return true ;
		}

		// FightMode 에서는 이펙트를 출력하지 않는다.
		if( GameModeManager.instance != null && GameModeManager.instance.curGameMode != null )
		{
			if( GameModeManager.instance.curGameMode.name.Contains( "FightMode" ) == true )
			{
				return true;
			}
		}

		return false;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 터치 및 드레그 이벤트 발생 시점의 좌표 연산
	/// </summary>
	/// <param name="clickedObject"></param>
	/// <returns></returns>
	private Vector3 CalculateScreenPositon( GameObject clickedObject )
	{
		_tempPositionZ = null == clickedObject ? 0f : clickedObject.transform.position.z;

		_tempPositionVector = new Vector3( UICamera.currentTouch.pos.x, UICamera.currentTouch.pos.y, _tempPositionZ );

		_tempPositionVector = UICamera.currentCamera.ScreenToWorldPoint( _tempPositionVector );

		return _tempPositionVector;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 스크린 터치시 동작할 이벤트
	/// </summary>
	/// <param name="clickedObject"></param>
	private void OnEventPressScreen( GameObject clickedObject, bool state )
	{
		if( IsSkipEvent() == true )
		{
			return;
		}

		if( state == true )
		{
			++_effectIndex;
			_effectIndex %= _effectCountMax;

			StopAll();
			_effectList[ _effectIndex ].PlayPress( CalculateScreenPositon( clickedObject ) );

			return;
		}

		_effectList[ _effectIndex ].PlayRelease( CalculateScreenPositon( clickedObject ) );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 드레그시 동작할 이벤트
	/// </summary>
	/// <param name="clickedObject"></param>
	/// <param name="delta"></param>
	private void OnEventDragScreen( GameObject clickedObject, Vector2 delta )
	{
		if( IsSkipEvent() == true )
		{
			return;
		}

		_effectList[ _effectIndex ].PlayPress( CalculateScreenPositon( clickedObject ) );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//class ScreenTouchEffectManager
