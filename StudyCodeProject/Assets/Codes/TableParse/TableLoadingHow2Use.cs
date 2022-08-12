using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LitJson;

public class TableLoadingHow2Use : MonoBehaviour
{
    // Start is called before the first frame update
    protected async UniTaskVoid Start()
    {
        var myClass = new MyClass();

        var clone = DeepCopy.Copy( myClass );

		var result = await RESTAPIManager.INSTANCE.Request( 
            uri:"https://www.google.co.jp/", 
            cancleKey: "", 
            "client_id", "testRESTAPI",
            "client_secret", "",
            "token", "token" );

        if( true == result.Item1 )
		{
            DebugExtensions.Log( $"result.Item2 = {result.Item2}", Color.white );
        }
	}

	// Update is called once per frame
	void Update()
    {
        
    }

    public struct MyStruct
	{
        public int      index;
        public string   name;
	}

    public class MyClass
	{
        List<MyStruct> myStructs = new List<MyStruct>();

        public MyClass()
		{
            for(int i = 0; i < 10; ++i)
			{
                myStructs.Add( new MyStruct() );
			}
		}
    }
}
