using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]// 스크립트 내에 있는 콜백 함수들을 실행 (Play) 이 아닌 수정 (Edit) 모드에서도 동작하도록 하는 속성이다. 성능저하가 있으므로 UNITY_EDITOR 로 한정한다.
#endif//UNITY_EDITOR
public class Study_Inspector : MonoBehaviour
{
	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	[Header("float 값의 범위를 지정할 수 있다.")]
	[Range(0, 1)]
	[Tooltip("0 ~ 1사이의 값을 설정 할 수 있다.")]
	[SerializeField]
	private float testRange = 0f;

	[Space( 20 )]
	[Header("Multiline 으로 문자열의 라인수를 늘려줄 수 있다.")]
	[Multiline(3)]
	[Tooltip("3줄 까지입력")]
	[SerializeField]
	private string multiline = "Multiline 초기값";

	[Space( 20 )]
	[Header("TextArea 으로 문자열의 라인수를 늘려줄 수 있다.")]
	[TextArea(3,5)]
	[Tooltip("3줄 에서 5줄까지 까지입력")]
	[ContextMenuItem( "문자열을 초기화 할 함수를 부를 수 있다.", "Reset")]
	[SerializeField]
	private string textArea = "TextArea 초기값";


	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	[ContextMenu( "스트링 데이터 초기화" )]
	public void Reset()
	{
		multiline = "Multiline 초기값";
		textArea = "TextArea 초기값";
	}

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

}//Study_Inspector
