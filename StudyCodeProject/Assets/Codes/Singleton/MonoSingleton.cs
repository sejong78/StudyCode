/**---------------------------------------------------------------------------------
 * @file MonoSingleton.cs
 * @date 2023/7/6
 * @author sejong
 * @brief 모노를 상속받는 싱글턴 베이스, 사용은 상속을 받아서 사용한다.
 *///-------------------------------------------------------------------------------
using System;
using UnityEngine;

namespace BaseSingleton
{
	/// <summary>
	/// @class MonoSingleton
	/// @date 2023/7/6
	/// @author sejong
	/// @brief 모노를 상속받는 싱글턴 베이스, 사용은 상속을 받아서 사용한다.
	/// Lazy<T>는 초기화를 지연시켜서 접근하려고 하면 그때 객체를 생성하는 클래스로,
	/// 멀티 스레드에서도 안전하기 때문에 lock 대신 사용할 수 있다.
	/// </summary>
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour, IBaseSingleton
	{
		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------

		protected static readonly Lazy<T> _instance = new Lazy<T>( CreateSingleton );

		public static T INSTANCE
		{
			get
			{
				return _instance.Value;
			}
		}

		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 생성 함수 변경의 여지가 없다.
		/// </summary>
		/// <returns></returns>
		private static T CreateSingleton()
		{
			// 일단 하이어라키를 뒤져서 이미 생성 되어 있는 오브젝트가 있는지 찾는다.
			T instance = GameObject.FindObjectOfType( typeof( T ) ) as T;

			// 있다면 그걸 사용한다.
			if( null != instance )
			{
				instance.Initialize();

				return instance;
			}

			// 없다면 생성한다.
			GameObject go = new GameObject( typeof(T).Name );

			instance = go.AddComponent<T>();

			DontDestroyOnLoad( go );

			instance.Initialize();

			return instance;
		}

		//@@-------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 싱글턴 초기화
		/// </summary>
		public static void Reset()
		{
			if( null != _instance )
				_instance.Value.Release();
		}

		//@@-------------------------------------------------------------------------------------------------------------------------
		//@@-------------------------------------------------------------------------------------------------------------------------

	}//MonoSingleton

}//BaseSingleton
