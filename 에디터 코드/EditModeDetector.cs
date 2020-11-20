// 에디터가 실행중인경우 컴파일을 멈추고자 만들어짐
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditModeDetectorScript
{
    static bool waitingForStop = false;

    static EditModeDetectorScript()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    static void OnEditorUpdate()
    {
        if (!waitingForStop
            && EditorApplication.isCompiling
            && EditorApplication.isPlaying)
        {
            EditorApplication.LockReloadAssemblies();
            EditorApplication.playModeStateChanged  += PlaymodeChanged;
            waitingForStop = true;
        }
    }

    static void PlaymodeChanged(PlayModeStateChange state)
    {
        if (EditorApplication.isPlaying)
            return;

        EditorApplication.UnlockReloadAssemblies();
        EditorApplication.playModeStateChanged
             -= PlaymodeChanged;
        waitingForStop = false;
    }
}