using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Newtonsoft.Json.Linq;

public class ReactiveProperty_Send : MonoBehaviour
{
    private RPData _rpData = new RPData();
    public RPData PR_DATA => _rpData;

	/// <summary>
	/// 일반 클레스를 ReactiveProperty 로 할당 하는 경우
    /// 멤버 변수에 대한 추적이 가능하다.
	/// </summary>
	private ReactiveProperty<NormalData> _norData = new ReactiveProperty<NormalData>( new NormalData() );
    public ReactiveProperty<NormalData> NORMAL_DATA => _norData;

	private IEnumerator Start()
    {
        var wait = new WaitForSeconds(1f);

        yield return wait;

        for( int i = 0; i < 10; ++i )
        {
            // 여기서 값이 변경되기 때문에 이떄 구독이 일어난다.
			PR_DATA.SetValue( PR_DATA.VALUE.Value + 5 );
            _norData.Value.VALUE = i;

			yield return wait;
		}

	}

}//ReactiveProperty_Send

public class RPData
{

    private readonly ReactiveProperty<int> _value = new ReactiveProperty<int>( initialValue:5 );

    public ReadOnlyReactiveProperty<int> VALUE => _value.ToReadOnlyReactiveProperty();

    public void SetValue( int val )
    {
        _value.Value = val;
    }

}//ReactivePropertyData


public class NormalData
{
	private int _value = 0;
	public int VALUE 
    {
        get { return _value; }
        set { _value = value; }
    }

	private int _value2 = 0;
	public int VALUE2
	{
		get { return _value2; }
		set { _value2 = value; }
	}

}//NormalData