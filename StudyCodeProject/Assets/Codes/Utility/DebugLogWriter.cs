/**---------------------------------------------------------------------------------
 * @file DebugLogWriter.cs
 * @date 2022/8/31
 * @author sejong
 * @brief 유니티 콘솔창에 System.Console 출력 내용이 표시되도록 하는 유틸 클래스
 *///-------------------------------------------------------------------------------
using UnityEngine;

/// <summary>
/// @class DebugLogWriter
/// @date 2022/8/31
/// @author sejong
/// @brief 유니티 콘솔창에 System.Console 출력 내용이 표시되도록 하는 유틸 클래스
/// 시스템 콘솔 출력 후, 추가로 유니티 로그 출력하도록 오버라이드 한다.
/// </summary>
public class DebugLogWriter : System.IO.TextWriter
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 게임 시작시 호출하여 활성화
	/// </summary>
	public static void Activate()
	{
		if( Application.isPlaying )
		{
			System.Console.SetOut( new DebugLogWriter() );
			Debug.Log( $"[DebugLogWriter] Activated !!!!" );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private static bool IsError( string message )
	{
		return !string.IsNullOrEmpty( message ) && ( message.Contains( "Error" ) || message.Contains( "error" ) );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	public override System.Text.Encoding Encoding
	{
		get { return System.Text.Encoding.UTF8; }
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void Write( string value )
	{
		base.Write( value );
		if( IsError( value ) )
		{
			UnityEngine.Debug.LogError( value );
		}
		else
		{
			UnityEngine.Debug.Log( value );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void WriteLine( string value )
	{
		base.WriteLine( value );
		if( IsError( value ) )
		{
			UnityEngine.Debug.LogError( value );
		}
		else
		{
			UnityEngine.Debug.Log( value );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void Write( string format, object arg0 )
	{
		base.Write( format, arg0 );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg0 );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg0 );
		}
	}
	
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void Write( string format, params object[] arg )
	{
		base.Write( format, arg );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void Write( string format, object arg0, object arg1 )
	{
		base.Write( format, arg0, arg1 );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg0, arg1 );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg0, arg1 );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void Write( string format, object arg0, object arg1, object arg2 )
	{
		base.Write( format, arg0, arg1, arg2 );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg0, arg1, arg2 );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg0, arg1, arg2 );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void WriteLine( string format, object arg0 )
	{
		base.Write( format, arg0 );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg0 );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg0 );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void WriteLine( string format, params object[] arg )
	{
		base.Write( format, arg );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void WriteLine( string format, object arg0, object arg1 )
	{
		base.Write( format, arg0, arg1 );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg0, arg1 );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg0, arg1 );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	public override void WriteLine( string format, object arg0, object arg1, object arg2 )
	{
		base.Write( format, arg0, arg1, arg2 );
		if( true == IsError( format ) )
		{
			UnityEngine.Debug.LogErrorFormat( format, arg0, arg1, arg2 );
		}
		else
		{
			UnityEngine.Debug.LogFormat( format, arg0, arg1, arg2 );
		}
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
}//DebugLogWriter