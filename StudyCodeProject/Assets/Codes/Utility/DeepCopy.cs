/**---------------------------------------------------------------------------------
 * @file DeepCopy.cs
 * @date 2022/8/3
 * @author sejong
 * @brief 사용시 내부적으로 리플렉션을 사용하기 때문에 실행속도가 느림을 고려해야 한다.\n
 * class , struct instance 의 deep copy를 제공함.\n
 * 게임에서 사용하는 Data Class 를 복사하려고 만듬.\n
 * container 는 Array , List , Dictionary 만 복사 제공함. 
 *///-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

/// <summary>
/// @class DeepCopy
/// @date 2022/8/3
/// @author sejong
/// @brief 사용시 내부적으로 리플렉션을 사용하기 때문에 실행속도가 느림을 고려해야 한다.\n
/// class , struct instance 의 deep copy를 제공함.\n
/// 게임에서 사용하는 Data Class 를 복사하려고 만듬.\n
/// container 는 Array , List , Dictionary 만 복사 제공함.  
/// </summary>
public static class DeepCopy
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	///  DeepCopy 에서 사용할 수 있는 타입들
	/// </summary>
	public enum eUSABLE_TYPE
	{
		SingleValue,
		Array,
		List,
		Dictionary,
		Class,
		Struct,
		UnKnown
	}

	// 제외시킬 타입들 여기에 넣어주세요.
	private static Type[] excludeTypes =
	{
		typeof(ArrayList),
	};

	/// <summary>
	///  아래의 BindingFlags 들로 , 멤버들 얻어와서 복사함.
	/// </summary>  
	private static BindingFlags _accessLevel = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	///  DeepCopyUtility 에서 사용할 수 있는 타입을 얻어옴. UnKnown 타입인지 사전 체크 요망.
	///  참고 : Type을 따로 받아야함. 인자로 받는 obj의 타입을 얻어오면 System object type 얻어옴.
	/// </summary>    
	private static eUSABLE_TYPE GetUsableType( Type type, object obj )
	{
		if( null == obj || true == type.IsInterface ) 
			return eUSABLE_TYPE.UnKnown;

		// 제외 타입에 포함되어 있다면 
		if( true == excludeTypes.Contains( type ) )
			return eUSABLE_TYPE.UnKnown;

		// 타입 체크
		if( true == type.IsValueType && true == type.IsPrimitive || typeof( string ) == type ) 
			return eUSABLE_TYPE.SingleValue;

		if( true == type.IsValueType && false == type.IsPrimitive && typeof( string ) != type ) 
			return eUSABLE_TYPE.Struct;

		if( true == type.IsArray && true == obj is Array ) 
			return eUSABLE_TYPE.Array;

		if( true == type.IsGenericType && true == obj is IList ) 
			return eUSABLE_TYPE.List;

		if( true == type.IsGenericType && true == obj is IDictionary ) 
			return eUSABLE_TYPE.Dictionary;

		if( false == type.IsValueType && false == type.IsGenericType && true == type.IsClass ) 
			return eUSABLE_TYPE.Class;

		return eUSABLE_TYPE.UnKnown;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 인터페이스 복사 함수
	/// </summary>
	/// <param name="srcObj"></param>
	/// <returns></returns>
	public static object Copy( object srcObj )
	{
		return CopyObject( srcObj );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	///  DeepCopyUtility 에서 사용할 수 있는 타입을 얻어옴. UnKnown 타입인지 사전 체크 요망.
	/// </summary>  
	private static object CopyObject( object srcObj )
	{
		if( null == srcObj )
			return null;

		try
		{
			object			target		= null;
			var				srcObjType	= srcObj.GetType();
			eUSABLE_TYPE	usableType	= GetUsableType( srcObjType, srcObj );

			if( eUSABLE_TYPE.UnKnown == usableType ) 
				return null;

			switch( usableType )
			{
				case eUSABLE_TYPE.SingleValue:
					target = srcObj;
					break;
				case eUSABLE_TYPE.Array:
					target = CopyArray( srcObj as Array );
					break;

				case eUSABLE_TYPE.List:
					target = CopyList( srcObj as IList );
					break;

				case eUSABLE_TYPE.Dictionary:
					target = CopyDictionary( srcObj as IDictionary );
					break;

				case eUSABLE_TYPE.Struct:
					target = CopyStruct( srcObj );
					break;

				case eUSABLE_TYPE.Class:
					target = CopyClass( srcObj );
					break;

				case eUSABLE_TYPE.UnKnown:
#if UNITY_EDITOR
					DebugExtensions.Log( $"지원하지 않는 타입입니다!!!!!!!!!!!", UnityEngine.Color.red );
#endif//UNITY_EDITOR
					break;
			}

			return target;
		}
		catch( Exception e )
		{

#if UNITY_EDITOR
			DebugExtensions.Log( $"[Exception] CopyObject = {e.Message}", UnityEngine.Color.white );
#endif//UNITY_EDITOR
		}

		return null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 배열 복사
	/// </summary>
	/// <param name="srcArray"></param>
	/// <returns></returns>
	private static object CopyArray( Array srcArray )
	{
		try
		{
			if( null == srcArray  )
			{
				return null;
			}

			Type	type		= srcArray.GetType();
			Array	targetArray = Array.CreateInstance( type.GetElementType(), srcArray.Length );

			object val			= null;
			object copiedObj	= null;

			for( int i = 0; i < srcArray.Length; ++i )
			{
				val			= srcArray.GetValue( i );
				copiedObj	= CopyObject( val );

				targetArray.SetValue( copiedObj, i );
			}

			return targetArray;
		}
		catch( Exception e )
		{
#if UNITY_EDITOR
			DebugExtensions.Log( $"[Exception] CopyArray = {e.Message}", UnityEngine.Color.white );
#endif//UNITY_EDITOR
		}

		return null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 리스트 복사
	/// </summary>
	/// <param name="srcList"></param>
	/// <returns></returns>
	private static object CopyList( IList srcList )
	{
		try
		{
			if( null == srcList )
			{
				return null;
			}

			// srcList 를 리플렉션을 통해 얻은 정보를 기반으로 복사할 리스트를 생성한다.
			Type    itemType	= srcList.GetType().GetGenericArguments()[0];
			Type	listType	= typeof( List<> );
			Type	actualType	= listType.MakeGenericType( itemType );
			IList	targetList	= Activator.CreateInstance( actualType ) as IList;

			if( null == targetList )
			{
				return null;
			}

			object copiedObj = null;

			foreach( var v in srcList )
			{
				copiedObj = CopyObject(v);

				targetList.Add( copiedObj );
			}

			return targetList;
		}
		catch( Exception e )
		{
#if UNITY_EDITOR
			DebugExtensions.Log( $"[Exception] CopyList = {e.Message}", UnityEngine.Color.white );
#endif//UNITY_EDITOR
		}

		return null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 딕셔너리 복사
	/// </summary>
	/// <param name="srcDic"></param>
	/// <returns></returns>
	private static object CopyDictionary( IDictionary srcDic )
	{
		try
		{
			if( null == srcDic )
			{
				return null;
			}

			// srcDic 를 리플렉션을 통해 얻은 정보를 기반으로 복사할 딕셔너리를 생성한다.
			Type[]		argTypes	= srcDic.GetType().GetGenericArguments();
			Type		keyType		= argTypes[0]; // 0 : Key Type
			Type		valueType	= argTypes[1]; // 1 : Value Type
			Type		dicType		= typeof( Dictionary<,> );
			Type		actualType	= dicType.MakeGenericType(keyType, valueType);
			IDictionary targetDic	= Activator.CreateInstance(actualType) as IDictionary;

			if( null == targetDic )
			{
				return null;
			}

			object copiedKey = null;
			object copiedObj = null;

			foreach( DictionaryEntry de in srcDic )
			{
				 
				copiedKey = CopyObject( de.Key );
				copiedObj = CopyObject( de.Value );

				targetDic[ copiedKey ] = copiedObj;
			}

			return targetDic;
		}
		catch( Exception e )
		{
#if UNITY_EDITOR
			DebugExtensions.Log( $"[Exception] CopyDictionary = {e.Message}", UnityEngine.Color.white );
#endif//UNITY_EDITOR
		}
		return null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	
	/// <summary>
	/// 구조체 복사
	/// </summary>
	/// <param name="srcStruct"></param>
	/// <returns></returns>
	private static object CopyStruct( object srcStruct )
	{
		try
		{
			if( null == srcStruct )
				return null;

			var srcStructType = srcStruct.GetType();

			if( null == srcStructType )
				return null;

			FieldInfo[]     srcFields   = null;
			FieldInfo       field       = null;

			object srcMember            = null;
			object copiedMember         = null;

			object target               = null;

			target = Activator.CreateInstance( srcStructType );

			if( null == target )
			{
#if UNITY_EDITOR
				DebugExtensions.LogError( $"fail!! Activator.CreateInstance({srcStructType})", UnityEngine.Color.red );
#endif//UNITY_EDITOR
				return null;
			}


			srcFields = srcStructType.GetFields( _accessLevel );

			for( int i = 0; i < srcFields.Length; ++i )
			{
				field = srcFields[ i ];

				if( null == field )
					continue;

				srcMember = field.GetValue( srcStruct );
				copiedMember = CopyObject( srcMember );

				field.SetValue( target, copiedMember );
			}

			return target;
		}
		catch( Exception e )
		{
#if UNITY_EDITOR
			DebugExtensions.Log( $"[Exception] CopyStruct = {e.Message}", UnityEngine.Color.white );
#endif//UNITY_EDITOR
		}

		return null;

	}

	//@@-------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 클레스 복사
	/// </summary>
	/// <param name="srcClass"></param>
	/// <returns></returns>
	private static object CopyClass( object srcClass )
	{
		try
		{
			if( null == srcClass )
				return null;

			var srcClassType = srcClass.GetType();

			if( null == srcClassType )
				return null;

			ConstructorInfo constructor = null;
			FieldInfo[]     srcFields   = null;
			FieldInfo       field       = null;

			object srcMember            = null;
			object copiedMember         = null;

			object target				= null;

			constructor = srcClassType.GetConstructors().OrderBy( c => c.GetParameters().Length ).FirstOrDefault();
	
			if( null == constructor )
			{
	#if UNITY_EDITOR
				DebugExtensions.LogError( $"생성자 없음!!!", UnityEngine.Color.red );
	#endif//UNITY_EDITOR
				return null;
			}
	
			target = constructor.Invoke( new object[ constructor.GetParameters().Length ] );
	
			srcFields = srcClassType.GetFields( _accessLevel );
	
			for( int i = 0; i < srcFields.Length; ++i )
			{
				field = srcFields[ i ];
	
				if( null == field )
					continue;
	
				srcMember	 = field.GetValue( srcClass );
				copiedMember = CopyObject( srcMember );
	
				field.SetValue( target, copiedMember );
			}

			return target;
		}
		catch( Exception e )
		{
#if UNITY_EDITOR
			DebugExtensions.Log( $"[Exception] CopyClass = {e.Message}", UnityEngine.Color.white );
#endif//UNITY_EDITOR
		}

		return null;
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}// DeepCopy
