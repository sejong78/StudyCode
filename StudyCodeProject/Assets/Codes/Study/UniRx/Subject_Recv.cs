using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class Subject_Recv : MonoBehaviour
{
    //@@-------------------------------------------------------------------------------------------------------------------------
    //@@-------------------------------------------------------------------------------------------------------------------------
    
    void Start()
    {
        var mySubject = GetComponent<Subject_Send>().SUBJECT;

        mySubject.
            Do( SomeMethod ).
            DoOnCompleted( () => 
            { 
                DebugExtensions.Log( $"³¡~", Color.white );
            } ).
            Subscribe();
	}

    //@@-------------------------------------------------------------------------------------------------------------------------
    
	private void SomeMethod( int n )
    {
		DebugExtensions.Log( $"n = {n}", Color.white );
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//Subject_Recv
