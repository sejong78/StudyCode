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
