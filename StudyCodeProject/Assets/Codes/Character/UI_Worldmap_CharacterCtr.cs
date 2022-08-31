using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Worldmap_CharacterCtr : MonoBehaviour
{
    //@@-------------------------------------------------------------------------------------------------------------------------
    //@@-------------------------------------------------------------------------------------------------------------------------

    public enum EState
    {
        Idle,
        Run,
        Battle_Idle,

        Attack,
        Skill,
        Damaged,
    }

    public enum EModel
    {
        // 월드맵에서 뛰어다니는 캐릭터
        Worldmap,

        // 영입 전투 모델
        Recruit,

        // 케루빔 전투 모델
        Cherubim,
    }

    // 애니메이터
    [SerializeField]
    private Animator m_Animator = null;

    // 런 애니메이션 이펙트 루트
    [SerializeField]
    private GameObject m_Root_RunEfx = null;

    /// <summary>
    /// 오리지널 캐릭터 머티리얼
    /// </summary>
    [SerializeField]
    private Material m_Material_Origin = null;

    /// <summary>
    /// 케루빔 캐릭터 전용 머티리얼
    /// </summary>
    [SerializeField]
    private Material m_Material_Cherubim = null;

    /// <summary>
    /// 머티리얼 적용 대상 렌더러 리스트
    /// </summary>
    [SerializeField]
    private List<SkinnedMeshRenderer> m_RenderList = null;

    //@@-------------------------------------------------------------------------------------------------------------------------
    //@@-------------------------------------------------------------------------------------------------------------------------

    public void Play(EState _state, Vector3 _lookDir)
    {
        if (null == m_Animator)
            return;

        if (false == System.Enum.IsDefined(typeof(EState), _state))
            return;

        if (m_Animator.GetCurrentAnimatorStateInfo(0).Equals(_state.ToString()))
            return;

        if( _lookDir != Vector3.zero )
            transform.localRotation = Quaternion.LookRotation(_lookDir);

        m_Animator.Play(_state.ToString());

        if( null != m_Root_RunEfx )
    		m_Root_RunEfx.SetActive( EState.Run == _state );
	}

	public void SetMaterial(EModel _type)
    {
        if (false == System.Enum.IsDefined(typeof(EModel), _type))
            return;

        switch (_type)
        {
            case EModel.Worldmap:
            case EModel.Recruit:

                m_RenderList.ForEach(_renderer =>
                {
                    // 오리지널 머티리얼을 적용
                    _renderer.material = m_Material_Origin;
                });

                break;

            case EModel.Cherubim:

                m_RenderList.ForEach(_renderer =>
                {
                    // 케루빔 캐릭터 전용 머티리얼 (초록초록 물렁물렁)
                    _renderer.material = m_Material_Cherubim;
                });

                break;
        }
    }

	//@@-------------------------------------------------------------------------------------------------------------------------
	//@@-------------------------------------------------------------------------------------------------------------------------

	[ContextMenu("STATE/ATTACK")]
    public void Attack()
	{
        if( false == Application.isPlaying )
            return;

        Play( EState.Attack, Vector3.zero );
	}

	[ContextMenu( "STATE/SKILL" )]
	public void Skill()
	{
		if( false == Application.isPlaying )
			return;

		Play( EState.Skill, Vector3.zero );
	}

	[ContextMenu( "STATE/Damaged" )]
	public void Damaged()
	{
		if( false == Application.isPlaying )
			return;

		Play( EState.Damaged, Vector3.zero );
	}

}//UI_Worldmap_CharacterCtr
