using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(DCSceneObjectManager))]
public class ED_SceneObjectManager : Editor
{
    private DCSceneObjectManager m_Data;

    void OnEnable()
    {
        m_Data = (DCSceneObjectManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Rebuild List") == true)
        {
            for (int i = m_Data.List.Count - 1; i >= 0; i--)
            {
                if (m_Data.List[i] == null)
                    m_Data.List.RemoveAt(i);
            }

            m_Data.Dic.Clear();

            for (int i = 0; i < m_Data.List.Count; i++)
                m_Data.Dic.Add(m_Data.List[i].name, m_Data.List[i]);
        }

        GUILayout.BeginVertical("box");
        m_Data.m_BuildScenePath = EditorGUILayout.TextField("Build Scene Path", m_Data.m_BuildScenePath);
        m_Data.m_BuildSceneName = EditorGUILayout.TextField("Build Scene Name", m_Data.m_BuildSceneName);

        if (GUILayout.Button("Save Scene As") == true)
        {
            if (m_Data.m_BuildSceneName.Length > 0)
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(),
                    m_Data.m_BuildScenePath + "/" + m_Data.m_BuildSceneName + ".unity", false);
            }
        }
        GUILayout.EndVertical();

        if (GUI.changed)
            EditorUtility.SetDirty(m_Data);
    }
}
