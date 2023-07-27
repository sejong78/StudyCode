using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// UniRX���� �����ϴ� Subject<T>�� ���� UniRX�� ��Ʈ���� ������ �� �ִ�.
/// ISubject�� IObservable �� IObserver�� �����ϰ� �ִ�.
/// IObserver : OnNext(), OnCompleted(), OnError()
/// IObservable : Subscribe()
/// subject ��ü���� OnNext(), OnCompleted(), OnError() �޽����� ���� ȣ���� �� �ִ�.
/// </summary>
public class Subject_Send : MonoBehaviour
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------
	
	private Subject<int> mySubject = new Subject<int>();

	/// <summary>
	/// �ܺο��� Subscribe �� ������ �����ϰ� �ϱ� ����
	/// </summary>
	public IObservable<int> SUBJECT => mySubject;

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	private IEnumerator Start()
	{
		var wait = new WaitForSeconds(1f);

		yield return wait;
		mySubject.OnNext( 1 );
		yield return wait;
		mySubject.OnNext( 10 );
		yield return wait;
		mySubject.OnCompleted();
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//Subject_Send
