using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using Marlboro.AI;


[UnityEditor.CustomEditor(typeof(AiAgent))]
public class AiAgentInspector : Editor
{
    public AiAgent agent 
    {
        get { return target as AiAgent; }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /// AI 정보 출력
        string aiID = agent.newData != null ? agent.newData.ID.ToString() : "Invalid";
        EditorGUILayout.LabelField("AI ID", aiID);
        
        string fuzzyName = agent.fuzzyName;
        EditorGUILayout.LabelField("Fuzzy Name", fuzzyName);

        AiModuleType module = agent != null ? agent.currentModule : AiModuleType.Invalid;
        EditorGUILayout.LabelField("Module", module.ToString());

        float block = agent.defensiveMeter;
        float opponentBlock = 0f;
        if (AiManager.available)
        {
            if (agent.target != null)
            {
                Character opponent = GameManager.GetOpponent(agent.target);
                if (opponent != null)
                {
                    opponentBlock = AiManager.instance.GetBlockMeter(opponent);
                }
            }
        }

        EditorGUILayout.LabelField("Fuzzy Values");
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Damage", agent.fuzzyDamageValue.ToString());
        EditorGUILayout.LabelField("Evade", agent.fuzzyEvadeValue.ToString());
        EditorGUILayout.LabelField("Block", block.ToString());
        EditorGUILayout.LabelField("Block(Opponent)", opponentBlock.ToString());
        EditorGUI.indentLevel--;


        // 자동 갱신을 위하여 실행해줍니다.
        Repaint();
    }
}
