using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraMode))]
public class CameraModeInspector : Editor
{
    public CameraMode data
    {
        get { return target as CameraMode; }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        NGUIEditorTools.DrawProperty("기본 카메라 모드 이름", serializedObject, "defaultCameraModeName");

        if (Application.isPlaying && GameModeManager.available)
        {
            EditorGUILayout.LabelField("현재 게임모드", GameModeManager.instance.curGameMode.ModeId.ToString());
            EditorGUILayout.LabelField("현재 카메라모드", GameModeManager.instance.curGameMode.cameraModeName);
        }
        GUILayout.Space(5f);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("새로운 카메라모드 추가"))
        {
            ArrayUtility.Add<CameraModeInfo>(ref data.cameraModeList, new CameraModeInfo());
        }
        if (GUILayout.Button("마지막 카메라모드 제거"))
        {
            ArrayUtility.RemoveAt<CameraModeInfo>(ref data.cameraModeList, data.cameraModeList.Length - 1);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);

        for (int i = 0 ; i < data.cameraModeList.Length; ++i)
        {
            if (NGUIEditorTools.DrawHeader(string.Format("[{0}] 카메라 모드", data.cameraModeList[i].name)))
            {
                NGUIEditorTools.BeginContents();
                data.cameraModeList[i].name = EditorGUILayout.TextField(new GUIContent("카메라 모드 이름"), data.cameraModeList[i].name);
                data.cameraModeList[i].usePlayer = EditorGUILayout.Toggle(new GUIContent("플레이어 정보 사용여부"), data.cameraModeList[i].usePlayer);
                if (data.cameraModeList[i].usePlayer)
                {
                    data.cameraModeList[i].playerPosition = EditorGUILayout.Vector3Field("플레이어 위치값", data.cameraModeList[i].playerPosition);
                    data.cameraModeList[i].playerRotation = EditorGUILayout.Vector3Field("플레이어 회전값", data.cameraModeList[i].playerRotation);
                }

                data.cameraModeList[i].useEnemy = EditorGUILayout.Toggle(new GUIContent("NPC 정보 사용여부"), data.cameraModeList[i].useEnemy);
                if (data.cameraModeList[i].useEnemy)
                {
                    data.cameraModeList[i].enemyPosition = EditorGUILayout.Vector3Field("NPC 위치값", data.cameraModeList[i].enemyPosition);
                    data.cameraModeList[i].enemyRotation = EditorGUILayout.Vector3Field("NPC 회전값", data.cameraModeList[i].enemyRotation);
                }

                data.cameraModeList[i].useAnchorRotateCamera = EditorGUILayout.Toggle(new GUIContent("회전용 카메라 사용여부"),
                    data.cameraModeList[i].useAnchorRotateCamera);
                if (data.cameraModeList[i].useAnchorRotateCamera == false)
                {
                    data.cameraModeList[i].useFollowing = EditorGUILayout.Toggle(new GUIContent("카메라 강제고정(인게임)"),
                    data.cameraModeList[i].useFollowing);
                }
                else
                {
                    data.cameraModeList[i].useDepthOfField = EditorGUILayout.Toggle(new GUIContent("DOF 효과 적용"),
                    data.cameraModeList[i].useDepthOfField);
                }

                data.cameraModeList[i].fov = EditorGUILayout.FloatField("Field of View", data.cameraModeList[i].fov);

                if (data.cameraModeList[i].useAnchorRotateCamera || data.cameraModeList[i].useFollowing == false)
                {
                    data.cameraModeList[i].cameraPosition = EditorGUILayout.Vector3Field("카메라 위치값", data.cameraModeList[i].cameraPosition);
                    data.cameraModeList[i].cameraRotation = EditorGUILayout.Vector3Field("카메라 회전값", data.cameraModeList[i].cameraRotation);
                    data.cameraModeList[i].cameraScale = EditorGUILayout.Vector3Field("카메라 크기값", data.cameraModeList[i].cameraScale);
                    data.cameraModeList[i].mainCameraPos = EditorGUILayout.Vector3Field("메인카메라 위치값", data.cameraModeList[i].mainCameraPos);
                }
                NGUIEditorTools.EndContents();
                GUILayout.Space(5f);
            }
        }

        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
        {
            OnValueChanged();
        }

        if (Application.isPlaying && GameModeManager.available && GUILayout.Button("새로운 카메라 데이터 적용"))
        {
            GUILayout.Space(5f);
            GameModeManager.instance.RefreshCameraModeList();
        }
    }

    void OnValueChanged()
    {
        UnityEngine.Object prefab = PrefabUtility.GetPrefabObject(data);
        EditorUtility.SetDirty(prefab);
    }
}
