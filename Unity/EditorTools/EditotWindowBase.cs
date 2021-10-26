/**---------------------------------------------------------------------------------
 * @file EditotWindowBase.cs
 * @date 2021/10/25
 * @author sejong
 * @brief Unity 에디터 윈도우 Base
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**---------------------------------------------------------------------------------
 * @file EditotWindowBase.cs
 * @date 2021/10/25
 * @author sejong
 * @brief Unity 에디터 윈도우 Base Class
 *///-------------------------------------------------------------------------------
public abstract class EditotWindowBase : EditorWindow
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 에디터 윈도우가 스크롤 되었을때 그 위치 기억용 벡터
	/// </summary>
	protected Vector3 _scrollPos = Vector3.zero;

	/// <summary>
	/// 컨트롤용 윈도우 클레스
	/// </summary>
	protected EditotWindowBase _baseWindow = null;

	/// <summary>
	/// 텍스트 기본
	/// </summary>
	protected Color _defaultTextColor = Color.white;

	/// <summary>
	/// 폰트 기본 너비
	/// </summary>
	protected float _fontWidth = 18f;

	/// <summary>
	/// 필드류 기본 너비
	/// </summary>
	protected float _defaultFieldWidth = 280f;

	/// <summary>
	/// 폴딩 여부판단 딕션너리
	/// </summary>
	protected Dictionary<string, bool> _foldStateDictionary = new Dictionary<string, bool>();

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 초기화 함수. 에디터 윈도우 기동시 호출
	/// </summary>
	protected virtual void Initialize( EditotWindowBase win )
	{
		_baseWindow = win;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 텍스트 쓰기
	/// </summary>
	/// <param name="text"></param>
	/// <param name="width"></param>
	protected void DrawText( string text, float width = -1f )
	{
		if( width < 0 )
			width = _fontWidth * ( text.Length + 1 );

		GUI.color = _defaultTextColor;

		EditorGUILayout.LabelField( text, GUILayout.Width( width ) );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 텍스트 쓰기
	/// </summary>
	/// <param name="text"></param>
	/// <param name="color"></param>
	/// <param name="width"></param>
	protected void DrawText( string text, Color color, float width = -1f )
	{
		if( width < 0 )
			width = _fontWidth * ( text.Length + 1 );

		GUI.color = color;

		EditorGUILayout.LabelField( text, GUILayout.Width( width ) );

		GUI.color = _defaultTextColor;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 라벨 필드 쓰기
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="width"></param>
	protected void DrawLabel( string key, string value, float width = 280f )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		GUI.color = _defaultTextColor;

		EditorGUILayout.LabelField( key, value, GUILayout.Width( width ) );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 라벨 필드 쓰기
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="color"></param>
	/// <param name="width"></param>
	protected void DrawLabel( string key, string value, Color color, float width = 280f )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		GUI.color = color;

		EditorGUILayout.LabelField( key, value, GUILayout.Width( width ) );

		GUI.color = _defaultTextColor;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	protected void InputField_Int( string key, int value, float width = 280f, Action<int> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = _defaultTextColor;
		int val = EditorGUILayout.IntField( key, value, GUILayout.Width( width ) );

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	protected void InputField_Int( string key, int value, Color color, float width = 280f, Action<int> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = color;
		int val = EditorGUILayout.IntField( key, value, GUILayout.Width( width ) );
		GUI.color = _defaultTextColor;

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}

	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	protected void InputField_Float( string key, float value, float width = 280f, Action<float> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = _defaultTextColor;
		float val = EditorGUILayout.FloatField( key, value, GUILayout.Width( width ) );

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	protected void InputField_Float( string key, float value, Color color, float width = 280f, Action<float> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = color;
		float val = EditorGUILayout.FloatField( key, value, GUILayout.Width( width ) );
		GUI.color = _defaultTextColor;

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	protected void InputField_String( string key, string value, float width = 280f, Action<string> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = _defaultTextColor;
		string val = EditorGUILayout.TextField( key, value, GUILayout.Width( width ) );

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	protected void InputField_String( string key, string value, Color color, float width = 280f, Action<string> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = color;
		string val = EditorGUILayout.TextField( key, value, GUILayout.Width( width ) );
		GUI.color = _defaultTextColor;

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	protected void Slider_Int( string key, int value, int minVal, int maxVal, float width = 280f, Action<int> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = _defaultTextColor;
		int val = EditorGUILayout.IntSlider( key, value, minVal, maxVal, GUILayout.Width( width ) );

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	protected void Slider_Int( string key, int value, int minVal, int maxVal, Color color, float width = 280f, Action<int> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = color;
		int val = EditorGUILayout.IntSlider( key, value, minVal, maxVal, GUILayout.Width( width ) );
		GUI.color = _defaultTextColor;

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	protected void Slider_float( string key, float value, float minVal, float maxVal, float width = 280f, Action<float> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = _defaultTextColor;
		float val = EditorGUILayout.Slider( key, value, minVal, maxVal, GUILayout.Width( width ) );

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	protected void Slider_float( string key, float value, float minVal, float maxVal, Color color, float width = 280f, Action<float> onChanged = null )
	{
		width = Mathf.Max( width, _defaultFieldWidth );

		EditorGUI.BeginChangeCheck();

		GUI.color = color;
		float val = EditorGUILayout.Slider( key, value, minVal, maxVal, GUILayout.Width( width ) );
		GUI.color = _defaultTextColor;

		if( EditorGUI.EndChangeCheck() == true && onChanged != null )
		{
			onChanged( val );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	protected void Button( string key, float width, Action OnClicked )
	{
		width = Mathf.Min( DivLine.WindowWidth - 10, width );

		if( GUILayout.Button( key, GUILayout.Width( width ) ) == true && OnClicked != null )
		{
			OnClicked();
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 들여쓰기를 한다.
	/// </summary>
	protected void PushTab()
	{
		++EditorGUI.indentLevel;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 들여쓰기를 되돌린다.
	/// </summary>
	protected void PopTab()
	{
		--EditorGUI.indentLevel;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 폴딩 여부 확인
	/// </summary>
	/// <param name="key"></param>
	/// <param name="name"></param>
	/// <param name="tooltip"></param>
	/// <param name="forceOpen"></param>
	/// <returns></returns>
	protected bool IsFoldout( string key, string name = "", string tooltip = "", bool forceOpen = false )
	{
		if( _foldStateDictionary.ContainsKey( key ) == false )
		{
			_foldStateDictionary[ key ] = forceOpen;
		}

		if( string.IsNullOrEmpty( name ) )
			name = key;

		var content = string.IsNullOrEmpty(tooltip) ? new GUIContent( name ) : new GUIContent( name, tooltip );

		_foldStateDictionary[ key ] = EditorGUILayout.Foldout( _foldStateDictionary[ key ], content );

		return _foldStateDictionary[ key ];
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 상속부에서 구현한다.
	/// </summary>
	protected abstract void OnDraw();

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 에디터 내용 기록
	/// </summary>
	protected void OnGUI()
	{
		if( _baseWindow == null )
		{
			_baseWindow = EditorWindow.GetWindow<EditotWindowBase>();
		}

		DivLine.Update( _baseWindow );

		_scrollPos = EditorGUILayout.BeginScrollView( _scrollPos );

		if( _baseWindow == null )
		{
			DrawText( "초기화가 되어있지 않습니다.", Color.red );

			EditorGUILayout.EndScrollView();
			return;
		}

		OnDraw();

		EditorGUILayout.EndScrollView();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 화면 분할 선
	/// </summary>
	public static class DivLine
	{
		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 에디터 윈도우 width
		/// </summary>
		public static int WindowWidth = 0;

		/// <summary>
		/// 분할선 텍스트를 이루는 문자갯수
		/// </summary>
		public static int StringCount = 0;

		/// <summary>
		/// 분할선 스트링
		/// </summary>
		public static string DivLineString = "";

		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------

		public static void Update( EditotWindowBase win )
		{
			int width = (int)win.position.width;

			if( WindowWidth == width )
				return;

			WindowWidth = width;

			StringCount		= 0;
			DivLineString	= "";
			for( int i = 0; i < width; i += 9 )
			{
				++StringCount;
				DivLineString += "=";
			}

		}

		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------
	}//DivLine

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//EditotWindowBase
